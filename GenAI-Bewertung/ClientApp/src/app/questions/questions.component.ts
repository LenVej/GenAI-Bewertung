import { Component, OnInit } from '@angular/core';
import { QuestionService } from './questions.service';
import { Question } from './questions.model';
import {BlankGap} from "./blank-gap.model";
import {environment} from "../../environments/environment.local";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-questions-component',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.scss']
})

export class QuestionsComponent implements OnInit {
  questions: Question[] = [];
  filterSubject: string = '';
  filterType: string = '';
  filterText: string = '';
  validationMessage: string = '';
  filterOwn: boolean = false;
  userId: number | null = null;


  newQuestion: any = {
    questionText: '',
    questionType: 'MultipleChoice',
    subject: '',
    choices: [''],
    correctIndices: [],
    optionA: '',
    optionB: '',
    correctAnswer: '',
    expectedAnswer: '',
    expectedResult: null,
    correctValue: null,
    tolerancePercent: null,
    clozeText: '',
    gaps: [],
    expectedKeywords: ''
  };

  newSolution: string = '';

  questionTypes = [
    'MultipleChoice',
    'EitherOr',
    'OneWord',
    'Math',
    'Estimation',
    'FillInTheBlank',
    'FreeText'
  ];

  questionTypeMap: { [key: string]: string } = {
    MultipleChoice: 'Ankreuzfrage',
    EitherOr: 'Entweder/Oder',
    OneWord: 'Ein-Wort',
    Math: 'Rechenfrage',
    Estimation: 'Schätzfrage',
    FillInTheBlank: 'Lückentext',
    FreeText: 'Freitext'
  };

  constructor(private questionService: QuestionService, private http: HttpClient) {}

  ngOnInit(): void {
    this.questionService.getQuestions().subscribe({
      next: (data) => this.questions = data,
      error: (err) => console.error('Failed to load questions', err)
    });

    this.http.get<any>(`${environment.apiBaseUrl}/api/auth/profile`).subscribe({
      next: (profile) => {
        console.log('Profil geladen:', profile); // ✅ Gültig
        this.userId = profile.userId;
      },
      error: (err) => console.error('Fehler beim Laden des Profils', err)
    });
  }

  createQuestion() {
    const error = this.getValidationError(this.newQuestion);
    if (error) {
      this.validationMessage = error;
      return;
    }

    this.validationMessage = '';
    this.questionService.createQuestion(this.newQuestion).subscribe({
      next: (created) => {
        this.questions.push(created);
        this.resetNewQuestion();
      },
      error: (err) => console.error('Fehler beim Erstellen', err)
    });
  }


  resetNewQuestion() {
    this.newQuestion = {
      questionText: '',
      questionType: 'MultipleChoice',
      subject: '',
      choices: [''],
      correctIndices: [],
      optionA: '',
      optionB: '',
      correctAnswer: '',
      expectedAnswer: '',
      expectedResult: null,
      correctValue: null,
      clozeText: '',
      gaps: [{ index: 0, solutions: [] }],
      expectedKeywords: ''
    };
  }

  addChoice() {
    this.newQuestion.choices.push('');
  }

  toggleCorrect(index: number) {
    const idx = this.newQuestion.correctIndices.indexOf(index);
    if (idx > -1) {
      this.newQuestion.correctIndices.splice(idx, 1);
    } else {
      this.newQuestion.correctIndices.push(index);
    }
  }

  addGap() {
    const nextIndex = this.newQuestion.gaps.length;
    this.newQuestion.gaps.push({ index: nextIndex, solutions: [] });
  }

  addSolutionToGap(gapIndex: number) {
    this.newQuestion.gaps[gapIndex].solutions.push('');
  }

  trackByIndex(index: number, item: any): number {
    return index;
  }

  removeSolutionFromGap(gapIndex: number, solutionIndex: number) {
    this.newQuestion.gaps[gapIndex].solutions.splice(solutionIndex, 1);
  }

  removeGap(gapIndex: number) {
    this.newQuestion.gaps.splice(gapIndex, 1);
    this.newQuestion.gaps.forEach((g: any, i: number) => g.index = i);
  }

  removeChoice(index: number) {
    this.newQuestion.choices.splice(index, 1);
    // Auch aus den korrekten Indices entfernen, wenn vorhanden
    this.newQuestion.correctIndices = this.newQuestion.correctIndices
      .filter((i: number) => i !== index)
      .map((i: number) => (i > index ? i - 1 : i));
  }

  get filteredQuestions(): Question[] {
    return this.questions.filter(q => {
      const matchesSubject = this.filterSubject ? q.subject.toLowerCase().includes(this.filterSubject.toLowerCase()) : true;
      const matchesType = this.filterType ? q.questionType === this.filterType : true;
      const matchesText = this.filterText ? q.questionText.toLowerCase().includes(this.filterText.toLowerCase()) : true;
      const matchesUser = this.filterOwn ? q.createdBy === this.userId : true;

      console.log('Frage:', q.questionId, 'Erstellt von:', q.createdBy, 'Dein User:', this.userId);

      return matchesSubject && matchesType && matchesText && matchesUser;
    });
  }

  getValidationError(q: any): string | null {
    if (!q.questionText.trim()) return '❗️Bitte gib einen Fragetext ein.';
    if (!q.subject.trim()) return '❗️Bitte gib ein Thema an.';

    switch (q.questionType) {
      case 'MultipleChoice':
        if (q.choices.length < 2) return '❗️Mindestens zwei Antwortmöglichkeiten erforderlich.';
        if (!q.choices.every((c: string) => c.trim())) return '❗️Alle Antwortmöglichkeiten müssen ausgefüllt sein.';
        if (q.correctIndices.length === 0) return '❗️Mindestens eine richtige Antwort markieren.';
        break;

      case 'EitherOr':
        if (!q.optionA.trim() || !q.optionB.trim()) return '❗️Option A und B müssen ausgefüllt sein.';
        if (q.correctAnswer !== 'A' && q.correctAnswer !== 'B') return '❗️Bitte gib A oder B als richtige Antwort an.';
        break;

      case 'OneWord':
        if (!q.expectedAnswer.trim()) return '❗️Bitte gib die erwartete Antwort ein.';
        break;

      case 'Math':
        if (q.expectedResult === null) return '❗️Bitte gib ein erwartetes Ergebnis ein.';
        break;

      case 'Estimation':
        if (q.correctValue === null) return '❗️Bitte gib den richtigen Wert ein.';
        break;

      case 'FillInTheBlank':
        if (!q.clozeText.trim()) return '❗️Bitte gib den Lückentext ein.';
        if (q.gaps.length === 0) return '❗️Mindestens eine Lücke hinzufügen.';
        if (!q.gaps.every((g: BlankGap) => g.solutions.length > 0 && g.solutions.every((s: string) => s.trim())))
          return '❗️Alle Lücken müssen mindestens eine Lösung enthalten.';
        break;

      case 'FreeText':
        if (!q.expectedKeywords.trim()) return '❗️Bitte gib Schlüsselwörter ein.';
        break;

      default:
        return '❗️Ungültiger Fragetyp.';
    }

    return null; // alles ok
  }
}
