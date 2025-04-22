import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {environment} from "../../environments/environment.local";
import { Observable } from 'rxjs';
import {Exam} from "./exam.model";
import {CreateExamDto} from "./create-exam.dto";

@Injectable({
  providedIn: 'root'
})
export class ExamService {
  private apiUrl = `${environment.apiBaseUrl}/api/exams`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<Exam[]> {
    return this.http.get<Exam[]>(this.apiUrl);
  }

  getById(id: number): Observable<Exam> {
    return this.http.get<Exam>(`${this.apiUrl}/${id}`);
  }

  create(exam: CreateExamDto): Observable<Exam> {
    return this.http.post<Exam>(this.apiUrl, exam);
  }
}
