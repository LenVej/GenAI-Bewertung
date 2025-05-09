import { Component, OnInit } from '@angular/core';
import { ExamService } from '../exam.service';
import { AuthService} from "../../auth.service";
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent} from "../../confirm-dialog/confirm-dialog.component";

@Component({
  selector: 'app-exam-list',
  templateUrl: './exam-list.component.html',
  styleUrls: ['./exam-list.component.css']
})
export class ExamListComponent implements OnInit {
  exams: any[] = [];

  constructor(
    private examService: ExamService,
    public auth: AuthService,
    private router: Router,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.examService.getAll().subscribe({
      next: (data) => this.exams = data,
      error: (err) => console.error('Failed to load exams', err)
    });
  }

  confirmAndStartExam(examId: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px',
      data: {
        title: 'Prüfung starten',
        message: 'Möchtest du die Prüfung wirklich starten?'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.router.navigate(['/exams/attempt', examId]);
      }
    });
  }


}
