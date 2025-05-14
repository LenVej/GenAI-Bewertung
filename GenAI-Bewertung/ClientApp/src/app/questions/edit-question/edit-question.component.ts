import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionService } from '../questions.service';
import { Question } from '../questions.model';
import { BlankGap } from '../blank-gap.model';

@Component({
  selector: 'app-edit-question',
  templateUrl: './edit-question.component.html',
  styleUrls: ['./edit-question.component.scss']
})
export class EditQuestionComponent implements OnInit {
  questionId!: number;
  question: Question | null = null;
  error = '';
  saving = false;

  constructor(
    private route: ActivatedRoute,
    private questionService: QuestionService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.questionId = parseInt(this.route.snapshot.paramMap.get('id')!, 10);
    this.questionService.getQuestionById(this.questionId).subscribe({
      next: q => {
        if (q.questionType === 'FillInTheBlank') {
          q.gaps ??= [];
        }
        this.question = q;
      },
      error: () => this.error = 'Frage konnte nicht geladen werden.'
    });
  }

  save(): void {
    if (!this.question) return;
    this.saving = true;

    this.questionService.updateQuestion(this.questionId, this.question).subscribe({
      next: () => this.router.navigate(['/profile']),
      error: () => {
        this.error = 'Fehler beim Speichern.';
        this.saving = false;
      }
    });
  }

  trackByIndex(index: number): number {
    return index;
  }

  addChoice(): void {
    if (!this.question) return;
    this.question.choices ??= [];
    this.question.choices.push('');
  }


  removeChoice(index: number): void {
    if (!this.question?.choices) return;
    this.question.choices.splice(index, 1);
    if (this.question.correctIndices) {
      this.question.correctIndices = this.question.correctIndices
        .filter(i => i !== index)
        .map(i => (i > index ? i - 1 : i));
    }
  }

  toggleCorrect(index: number): void {
    if (!this.question) return;
    this.question.correctIndices ??= [];
    const i = this.question.correctIndices.indexOf(index);
    if (i > -1) {
      this.question.correctIndices.splice(i, 1);
    } else {
      this.question.correctIndices.push(index);
    }
  }


  addGap(): void {
    if (!this.question) return;
    this.question.gaps ??= [];
    const nextIndex = this.question.gaps.length;
    this.question.gaps.push({ index: nextIndex, solutions: [''] });
  }


  removeGap(index: number): void {
    if (!this.question?.gaps) return;
    this.question.gaps.splice(index, 1);
    this.question.gaps.forEach((g, i) => g.index = i);
  }

  addSolutionToGap(gapIndex: number): void {
    if (!this.question?.gaps?.[gapIndex]) return;
    this.question.gaps[gapIndex].solutions ??= [];
    this.question.gaps[gapIndex].solutions.push('');
  }

  removeSolutionFromGap(gapIndex: number, solutionIndex: number): void {
    if (!this.question?.gaps?.[gapIndex]?.solutions) return;
    this.question.gaps[gapIndex].solutions.splice(solutionIndex, 1);
  }
}
