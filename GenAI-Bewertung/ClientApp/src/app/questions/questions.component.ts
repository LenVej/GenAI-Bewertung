import { Component, OnInit } from '@angular/core';
import { QuestionService } from './questions.service';
import { Question } from './questions.model';

@Component({
  selector: 'app-questions-component',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.scss']
})

export class QuestionsComponent implements OnInit {
  questions: Question[] = [];

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

  constructor(private questionService: QuestionService) {}

  ngOnInit(): void {
    this.questionService.getQuestions().subscribe({
      next: (data) => this.questions = data,
      error: (err) => console.error('Failed to load questions', err)
    });
  }

  createQuestion() {
    this.questionService.createQuestion(this.newQuestion).subscribe({
      next: (q) => {
        this.questions.push(q);
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
          gaps: [
            { index: 0, solutions: [] }
          ],
          expectedKeywords: ''
        };
      },
      error: (err) => console.error('Fehler beim Erstellen', err)
    });
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
}
