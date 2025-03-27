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

  constructor(private questionService: QuestionService) {}

  ngOnInit(): void {
    this.questionService.getQuestions().subscribe({
      next: (data) => this.questions = data,
      error: (err) => console.error('Failed to load questions', err)
    });
  }
}
