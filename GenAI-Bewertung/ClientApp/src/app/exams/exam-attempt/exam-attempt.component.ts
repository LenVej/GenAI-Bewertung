import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormControl, FormGroup} from "@angular/forms";
import {Question} from "../../questions/questions.model";
import {ActivatedRoute, Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment.local";
import { HostListener } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CanExitComponent } from '../../guards/confirm-exit.guard';
import { ConfirmDialogComponent} from "../../confirm-dialog/confirm-dialog.component";
import {MatDialog} from "@angular/material/dialog";

@Component({
  selector: 'app-exam-attempt',
  templateUrl: './exam-attempt.component.html',
  styleUrls: ['./exam-attempt.component.scss']
})
export class ExamAttemptComponent implements OnInit, CanExitComponent  {
  attemptId!: number;
  examId!: number;
  title = '';
  timeLimitMinutes: number | null = null;
  questions: Question[] = [];
  currentIndex = 0;
  form!: FormGroup;
  answers: { [questionId: number]: any } = {};
  timeLeft = 0;
  timerInterval: any;
  protected readonly FormControl = FormControl;
  private hasSubmitted = false;
  loading = false;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private fb: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.http.post(`${environment.apiBaseUrl}/api/ExamAttempts/start`, { examId: id }).subscribe((res: any) => {
      this.attemptId = res.attemptId;
      this.examId = res.examId;
      this.title = res.examTitle;
      this.timeLimitMinutes = res.timeLimitMinutes;
      this.questions = res.questions;

      if (this.timeLimitMinutes) {
        this.timeLeft = this.timeLimitMinutes * 60;
        this.startTimer();
      }

      this.initForm();
    });
  }

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any): void {
    if (!this.hasSubmitted) {
      $event.preventDefault();
      $event.returnValue = true;
    }
  }

  canExit(): boolean {
    return this.hasSubmitted;
  }

  initForm(): void {
    const group: any = {};
    this.questions.forEach(q => {
      if (q.questionType === 'FillInTheBlank') {
        const gapCount = (q.clozeText?.match(/{{\d+}}/g) || []).length;
        for (let i = 0; i < gapCount; i++) {
          group[`${q.questionId}_${i}`] = this.fb.control('');
        }
      } else {
        group[q.questionId] = this.fb.control('');
      }
    });
    this.form = this.fb.group(group);
  }


  next(): void {
    if (this.currentIndex < this.questions.length - 1) this.currentIndex++;
  }

  prev(): void {
    if (this.currentIndex > 0) this.currentIndex--;
  }

  isAnswered(index: number): boolean {
    const q = this.questions[index];

    if (q.questionType === 'FillInTheBlank') {
      const gapCount = (q.clozeText?.match(/{{\d+}}/g) || []).length;

      for (let i = 0; i < gapCount; i++) {
        const control = this.form.get(`${q.questionId}_${i}`);
        const value = control?.value;

        if (typeof value !== 'string' || value.trim() === '') {
          return false;
        }
      }

      return true; // alle Lücken sind ausgefüllt
    }

    const control = this.form.get(q.questionId.toString());
    const value = control?.value;

    if (q.questionType === 'MultipleChoice') {
      return Array.isArray(value) && value.length > 0;
    }

    if (q.questionType === 'Math' || q.questionType === 'Estimation') {
      return value !== null && value !== undefined && value !== '';
    }

    return typeof value === 'string' && value.trim() !== '';
  }




  submit(skipConfirm = false): void {
    if (!skipConfirm) {
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        width: '350px',
        data: {
          title: 'Prüfung abschließen?',
          message: 'Willst du die Prüfung wirklich abschließen?'
        }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.submit(true);
        }
      });

      return;
    }

    const answers = this.questions.map(q => {
      if (q.questionType === 'FillInTheBlank') {
        const gapCount = (q.clozeText?.match(/({{\d+}})/g) || []).length;
        const textAnswer = Array.from({ length: gapCount }, (_, i) =>
          this.form.get(`${q.questionId}_${i}`)?.value
        );
        return {
          questionId: q.questionId,
          textAnswer: JSON.stringify(textAnswer)
        };
      }

      const value = this.form.get(q.questionId.toString())?.value;
      return {
        questionId: q.questionId,
        textAnswer: value !== null && value !== undefined ? value.toString().replace(',', '.') : null,
        selectedIndices: Array.isArray(value) ? value : null
      };
    });

    this.loading = true;

    this.http.post(`${environment.apiBaseUrl}/api/ExamAttempts/submit`, {
      attemptId: this.attemptId,
      answers
    }).subscribe({
      next: () => {
        clearInterval(this.timerInterval);
        this.hasSubmitted = true;
        this.snackBar.open('✅ Prüfung erfolgreich abgegeben!', 'OK', { duration: 3000 });
        this.router.navigate([`/exams/result/${this.attemptId}`]);
      },
      error: (err) => {
        console.error('Submit failed:', err);
        this.snackBar.open('❌ Fehler beim Abschicken!', 'OK', { duration: 3000 });
        this.loading = false;
        this.hasSubmitted = false;
      }
    });
  }



  startTimer(): void {
    this.timerInterval = setInterval(() => {
      this.timeLeft--;

      if (this.timeLeft <= 0) {
        clearInterval(this.timerInterval);
        this.snackBar.open('⏱️ Zeit abgelaufen – deine Prüfung wird jetzt abgegeben...', '', {
          duration: 3000
        });

        setTimeout(() => this.submit(true), 1000); // ✅ skipConfirm = true
      }
    }, 1000);
  }

  get timerDisplay(): string {
    const min = Math.floor(this.timeLeft / 60);
    const sec = this.timeLeft % 60;
    return `${min}:${sec.toString().padStart(2, '0')}`;
  }

  onCheckboxChange(event: Event, questionId: number): void {
    const checkbox = event.target as HTMLInputElement;
    const control = this.form.get(questionId.toString());

    if (!control) return;

    let selected: number[] = control.value || [];

    if (!Array.isArray(selected)) {
      selected = [];
    }

    const selectedIndex = +checkbox.value;

    if (checkbox.checked) {
      if (!selected.includes(selectedIndex)) {
        selected.push(selectedIndex);
      }
    } else {
      selected = selected.filter(v => v !== selectedIndex);
    }

    control.setValue(selected);
  }

  isSelected(questionId: number, index: number): boolean {
    const value = this.form.get(questionId.toString())?.value;
    return Array.isArray(value) && value.includes(index);
  }


  getControl(questionId: number): FormControl {
    return this.form.get(questionId.toString()) as FormControl;
  }

  getGapControl(questionId: number, gapIndex: number): FormControl {
    return this.form.get(`${questionId}_${gapIndex}`) as FormControl;
  }

}
