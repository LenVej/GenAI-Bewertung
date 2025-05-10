import { Component, OnInit } from '@angular/core';
import { QuestionService } from '../../questions/questions.service';
import { Question } from '../../questions/questions.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment.local';

@Component({
  selector: 'app-exam-create',
  templateUrl: './exam-create.component.html',
  styleUrls: ['./exam-create.component.css']
})
export class ExamCreateComponent implements OnInit {
  allQuestions: Question[] = [];
  selectedQuestionIds: number[] = [];
  userQuestionIds: number[] = [];
  title: string = '';
  description: string = '';
  timeLimitMinutes: number | null = null;
  questions: Question[] = [];

  filterText: string = '';
  filterSubject: string = '';
  filterOwnQuestions: boolean = false;

  exam = {
    title: '',
    description: '',
    timeLimitMinutes: 10
  };

  constructor(
    private questionService: QuestionService,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.questionService.getQuestions().subscribe({
      next: (data) => (this.questions = data),
      error: (err) => console.error('Fehler beim Laden der Fragen', err)
    });

    this.http.get<any[]>(`${environment.apiBaseUrl}/api/questions/by-user`).subscribe({
      next: data => this.userQuestionIds = data.map(q => q.questionId),
      error: err => console.error('Fehler beim Laden eigener Fragen', err)
    });
  }

  toggleQuestion(questionId: number, event: Event): void {
    const checked = (event.target as HTMLInputElement).checked;
    if (checked) {
      this.selectedQuestionIds.push(questionId);
    } else {
      this.selectedQuestionIds = this.selectedQuestionIds.filter(id => id !== questionId);
    }
  }

  createExam(): void {
    const payload = {
      title: this.title,
      description: this.description,
      timeLimitMinutes: this.timeLimitMinutes,
      questionIds: this.selectedQuestionIds
    };

    this.http.post(`${environment.apiBaseUrl}/api/exams`, payload).subscribe({
      next: () => this.router.navigate(['/exams']),
      error: (err) => console.error('Fehler beim Erstellen der Prüfung', err)
    });
  }

  get filteredQuestions(): Question[] {
    return this.questions.filter(q =>
      (!this.filterText || q.questionText.toLowerCase().includes(this.filterText.toLowerCase())) &&
      (!this.filterSubject || q.subject.toLowerCase().includes(this.filterSubject.toLowerCase())) &&
      (!this.filterOwnQuestions || this.userQuestionIds.includes(q.questionId))
    );
  }
}
