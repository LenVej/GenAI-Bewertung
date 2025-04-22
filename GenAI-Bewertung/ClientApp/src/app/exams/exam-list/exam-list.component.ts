import { Component, OnInit } from '@angular/core';
import { ExamService } from '../exam.service';
import { AuthService} from "../../auth.service";

@Component({
  selector: 'app-exam-list',
  templateUrl: './exam-list.component.html',
  styleUrls: ['./exam-list.component.css']
})
export class ExamListComponent implements OnInit {
  exams: any[] = [];

  constructor(
    private examService: ExamService,
    public auth: AuthService
  ) { }

  ngOnInit(): void {
    this.examService.getAll().subscribe({
      next: (data) => this.exams = data,
      error: (err) => console.error('Failed to load exams', err)
    });
  }

}
