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

  newQuestion: Partial<Question> = {
    questionText: '',
    questionType: 'MultipleChoice',
    subject: ''
  };

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
        this.newQuestion = { questionText: '', questionType: 'MultipleChoice', subject: '' };
      },
      error: (err) => console.error('Fehler beim Erstellen', err)
    });
  }

}
