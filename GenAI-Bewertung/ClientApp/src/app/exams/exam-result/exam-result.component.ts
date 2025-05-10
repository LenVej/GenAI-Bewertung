import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment.local';


@Component({
  selector: 'app-exam-result',
  templateUrl: './exam-result.component.html',
  styleUrls: ['./exam-result.component.scss']
})
export class ExamResultComponent implements OnInit {
  result: any;
  loading = true;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    const attemptId = this.route.snapshot.paramMap.get('id');
    if (attemptId) {
      this.http.get(`${environment.apiBaseUrl}/api/ExamAttempts/result/${attemptId}`).subscribe({
        next: (res: any) => {
          this.result = res;
          this.loading = false;
        },
        error: () => {
          this.loading = false;
        }
      });
    }
  }

  getSelectedAnswerTexts(result: any): string {
    if (!result.answerChoices || !result.selectedIndices) return result.textAnswer || '—';

    return result.selectedIndices
      .map((i: number) => result.answerChoices[i] ?? `[?${i}]`)
      .join(', ');
  }
}
