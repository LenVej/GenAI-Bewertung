import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Question } from '../../questions/questions.model';
import { QuestionService } from '../../questions/questions.service';
import { environment } from '../../../environments/environment.local';

@Component({
  selector: 'app-exam-edit',
  templateUrl: './exam-edit.component.html',
  styleUrls: ['./exam-edit.component.scss']
})
export class ExamEditComponent implements OnInit {
  examId!: number;
  title: string = '';
  description: string = '';
  timeLimitMinutes: number | null = null;

  questions: Question[] = [];
  selectedQuestionIds: number[] = [];

  filterText: string = '';
  filterSubject: string = '';

  examLoaded: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router,
    private questionService: QuestionService
  ) {}

  ngOnInit(): void {
    this.examId = parseInt(this.route.snapshot.paramMap.get('id') || '0');
    this.loadExam();
    this.loadQuestions();
  }

  loadExam() {
    this.http.get<any>(`${environment.apiBaseUrl}/api/exams/${this.examId}`).subscribe({
      next: (exam) => {
        this.title = exam.title;
        this.description = exam.description;
        this.timeLimitMinutes = exam.timeLimitMinutes;
        this.selectedQuestionIds = exam.questions.map((q: any) => q.questionId);
        this.examLoaded = true; // ✅ mark as loaded
      },
      error: err => console.error('Fehler beim Laden der Prüfung', err)
    });
  }


  loadQuestions() {
    this.questionService.getQuestions().subscribe({
      next: data => this.questions = data,
      error: err => console.error('Fehler beim Laden der Fragen', err)
    });
  }

  toggleQuestion(questionId: number, event: Event) {
    const checked = (event.target as HTMLInputElement).checked;
    if (checked) {
      this.selectedQuestionIds.push(questionId);
    } else {
      this.selectedQuestionIds = this.selectedQuestionIds.filter(id => id !== questionId);
    }
  }

  updateExam() {
    const payload = {
      title: this.title,
      description: this.description,
      timeLimitMinutes: this.timeLimitMinutes,
      questionIds: this.selectedQuestionIds
    };

    this.http.put(`${environment.apiBaseUrl}/api/exams/${this.examId}`, payload).subscribe({
      next: () => this.router.navigate(['/exams']),
      error: err => console.error('Fehler beim Aktualisieren der Prüfung', err)
    });
  }

  get filteredQuestions(): Question[] {
    return this.questions.filter(q =>
      (this.filterText ? q.questionText.toLowerCase().includes(this.filterText.toLowerCase()) : true) &&
      (this.filterSubject ? q.subject.toLowerCase().includes(this.filterSubject.toLowerCase()) : true)
    );
  }
}
