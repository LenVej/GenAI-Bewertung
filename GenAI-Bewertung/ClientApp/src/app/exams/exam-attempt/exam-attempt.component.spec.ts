import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ExamAttemptComponent } from './exam-attempt.component';
import { ActivatedRoute } from '@angular/router';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ReactiveFormsModule } from '@angular/forms';

describe('ExamAttemptComponent', () => {
  let component: ExamAttemptComponent;
  let fixture: ComponentFixture<ExamAttemptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ExamAttemptComponent],
      imports: [
        HttpClientTestingModule,
        MatSnackBarModule,
        ReactiveFormsModule
      ],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: () => '1' // Mocked examId
              }
            }
          }
        },
        {
          provide: Router,
          useValue: {
            navigate: jasmine.createSpy('navigate')
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ExamAttemptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should start the timer if timeLimitMinutes is set', () => {
    component.timeLimitMinutes = 1;
    component['timeLeft'] = 60;
    const startTimerSpy = spyOn<any>(component, 'startTimer');
    component.ngOnInit();
    expect(startTimerSpy).toHaveBeenCalled();
  });
});
