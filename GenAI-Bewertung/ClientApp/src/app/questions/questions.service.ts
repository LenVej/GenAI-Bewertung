import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Question } from './questions.model';
import {environment} from "../../environments/environment.local";

@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  private apiUrl = `${environment.apiBaseUrl}/api/questions`;

  constructor(private http: HttpClient) {}

  getQuestions(): Observable<Question[]> {
    return this.http.get<Question[]>(this.apiUrl);
  }

  createQuestion(q: Partial<Question>) {
    return this.http.post<Question>(this.apiUrl, q);
  }

  getQuestionById(id: number) {
    return this.http.get<Question>(`${this.apiUrl}/${id}`);
  }

  updateQuestion(id: number, updated: Partial<Question>) {
    return this.http.put(`${this.apiUrl}/${id}`, updated);
  }


}
