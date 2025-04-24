import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormControl, FormGroup} from "@angular/forms";
import {Question} from "../../questions/questions.model";
import {ActivatedRoute, Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment.local";


@Component({
  selector: 'app-exam-attempt',
  templateUrl: './exam-attempt.component.html',
  styleUrls: ['./exam-attempt.component.scss']
})
export class ExamAttemptComponent implements OnInit {
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

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private fb: FormBuilder,
    private router: Router
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

  initForm(): void {
    const group: any = {};
    this.questions.forEach(q => {
      group[q.questionId] = this.fb.control('');
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
    const val = this.form.get(q.questionId.toString())?.value;
    return val !== null && val !== '';
  }

  submit(): void {
    const answers = this.questions.map(q => {
      const value = this.form.get(q.questionId.toString())?.value;
      return {
        questionId: q.questionId,
        textAnswer: typeof value === 'string' ? value : null,
        selectedIndices: Array.isArray(value) ? value : null
      };
    });

    this.http.post(`${environment.apiBaseUrl}/api/ExamAttempts/submit`, {
      attemptId: this.attemptId,
      answers
    }).subscribe(() => {
      this.router.navigate([`/exams/${this.examId}/result`]);
    });
  }

  startTimer(): void {
    this.timerInterval = setInterval(() => {
      this.timeLeft--;
      if (this.timeLeft <= 0) {
        clearInterval(this.timerInterval);
        this.submit();
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

}
