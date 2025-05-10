import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import {environment} from "../../environments/environment.local";
import { TranslateService } from '@ngx-translate/core';


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})

export class ProfileComponent {
  user: any = null;
  userQuestions: any[] = [];
  error: string = '';
  tab = 'progress';
  currentLang = 'de'; // Default
  showConfirmModal = false;
  confirmMessage = '';
  private confirmCallback: () => void = () => {};
  profileStats: any = null;

  userExams: any[] = [];
  showQuestions = true;
  showExams = true;

  examProgress: any[] = [];

  stats = {
    questionsAnswered: 25,
    correctAnswers: 19
  };

  settings = {
    tolerance: 'medium',
    caseSensitive: false,
    estimateTolerance: 10
  };

  constructor(
    private http: HttpClient,
    private auth: AuthService,
    private router: Router,
    private translate: TranslateService
  ) {
    this.loadProfile();
    this.loadMyQuestions();
    this.loadMyExams();
    this.loadExamProgress();
    this.loadStats();
    this.currentLang = this.translate.currentLang || this.translate.getDefaultLang();
  }

  loadProfile() {
    this.http.get<any>(`${environment.apiBaseUrl}/api/auth/profile`).subscribe({
      next: data => {
        this.user = data;
        this.settings.tolerance = data.tolerance || 'medium';
        this.settings.caseSensitive = data.caseSensitive ?? false;
        this.settings.estimateTolerance = data.estimateTolerance ?? 10;
      },
      error: err => {
        this.error = 'Fehler beim Laden des Profils.';
        console.error(err);
      }
    });
  }


  loadMyQuestions() {
    this.http.get<any[]>(`${environment.apiBaseUrl}/api/questions/by-user`).subscribe({
      next: data => this.userQuestions = data,
      error: err => console.error('Fehler beim Laden der Fragen', err)
    });
  }

  loadExamProgress() {
    this.http.get<any[]>(`${environment.apiBaseUrl}/api/ExamAttempts/my-progress`).subscribe({
      next: data => this.examProgress = data,
      error: err => console.error('Fehler beim Laden des Fortschritts', err)
    });
  }

  loadStats() {
    this.http.get(`${environment.apiBaseUrl}/api/stats/profile-stats`).subscribe({
      next: data => this.profileStats = data,
      error: err => console.error('Fehler beim Laden der Statistiken', err)
    });
  }

  logout() {
    this.auth.logout();
  }

  deleteUser() {
    this.openConfirm('Willst du dein Konto wirklich löschen?', () => {
      this.auth.deleteAccount().subscribe(() => this.auth.logout());
    });
  }

  changeLanguage(lang: string) {
    this.translate.use(lang);
    this.currentLang = lang;
    localStorage.setItem('lang', lang);
    console.log(this.translate.currentLang)
  }

  deleteQuestion(id: number) {
    this.openConfirm('Möchtest du diese Frage wirklich löschen?', () => {
      this.http.delete(`${environment.apiBaseUrl}/api/questions/${id}`).subscribe({
        next: () => {
          this.userQuestions = this.userQuestions.filter(q => q.questionId !== id);
        },
        error: err => {
          console.error('Fehler beim Löschen der Frage', err);

          if (err.status === 500 && err.error?.toString().includes('FK_ExamQuestions_Questions_QuestionId')) {
            alert('Diese Frage kann nicht gelöscht werden, da sie noch einer Prüfung zugeordnet ist.');
          } else if (err.status === 500 && err.error?.toString().includes('FK_ExamAnswers_Questions_QuestionId')) {
            alert('Diese Frage kann nicht gelöscht werden, da sie in bereits beantworteten Prüfungen vorkommt.');
          } else {
            alert('Fehler beim Löschen der Frage. Bitte versuche es später erneut.');
          }
        }
      });
    });
  }



  openConfirm(message: string, callback: () => void) {
    this.confirmMessage = message;
    this.confirmCallback = callback;
    this.showConfirmModal = true;
  }

  confirmAction() {
    this.showConfirmModal = false;
    this.confirmCallback();
  }

  cancelAction() {
    this.showConfirmModal = false;
  }

  loadMyExams() {
    this.http.get<any[]>(`${environment.apiBaseUrl}/api/exams/by-user`).subscribe({
      next: data => this.userExams = data,
      error: err => console.error('Fehler beim Laden der Prüfungen', err)
    });
  }

  deleteExam(id: number) {
    this.openConfirm('Möchtest du diese Prüfung wirklich löschen?', () => {
      this.http.delete(`${environment.apiBaseUrl}/api/exams/${id}`).subscribe({
        next: () => {
          this.userExams = this.userExams.filter(e => e.examId !== id);
        },
        error: err => console.error('Fehler beim Löschen der Prüfung', err)
      });
    });
  }

  saveSettings() {
    this.http.put(`${environment.apiBaseUrl}/api/auth/profile/settings`, this.settings).subscribe({
      next: () => console.log('✅ Einstellungen gespeichert'),
      error: err => console.error('❌ Fehler beim Speichern', err)
    });
  }
}

