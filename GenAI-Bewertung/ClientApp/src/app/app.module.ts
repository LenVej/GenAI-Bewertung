import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { QuestionsComponent } from './questions/questions.component';
import { ProfileComponent } from './profile/profile.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';

import { AuthGuard } from './auth.guard';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptor } from './jwt.interceptor';

import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { ExamListComponent } from './exams/exam-list/exam-list.component';
import { ExamCreateComponent } from "./exams/exam-create/exam-create.component";
import { ExamAttemptComponent } from './exams/exam-attempt/exam-attempt.component';
import { ExamResultComponent } from './exams/exam-result/exam-result.component';
import { ExamEditComponent } from './exams/exam-edit/exam-edit.component';
import { FillInTheBlanksPipe } from './exams/exam-attempt/fill-in-the-blanks.pipe';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ConfirmExitGuard } from './guards/confirm-exit.guard';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { MatDialogModule } from "@angular/material/dialog";
import { EditQuestionComponent } from './questions/edit-question/edit-question.component';



export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    QuestionsComponent,
    ProfileComponent,
    LoginComponent,
    RegisterComponent,
    ExamListComponent,
    ExamCreateComponent,
    ExamAttemptComponent,
    ExamResultComponent,
    ExamEditComponent,
    FillInTheBlanksPipe,
    ConfirmDialogComponent,
    EditQuestionComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatSnackBarModule,
    MatDialogModule,
    RouterModule.forRoot([
      {path: '', component: HomeComponent, pathMatch: 'full'},
      {path: 'questions', component: QuestionsComponent},
      {path: 'questions/edit/:id', component: EditQuestionComponent},
      {path: 'login', component: LoginComponent},
      {path: 'register', component: RegisterComponent},
      {path: 'profile', component: ProfileComponent},
      {
        path: 'exams',
        children: [
          {path: '', component: ExamListComponent},
          {path: 'create', component: ExamCreateComponent},
          {path: 'attempt/:id', component: ExamAttemptComponent, canDeactivate: [ConfirmExitGuard]},
          {path: 'result/:id', component: ExamResultComponent},
          {path: 'edit/:id', component: ExamEditComponent}
        ]
      }
    ]),

    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),

    BrowserAnimationsModule,
    MatDialogModule
  ],
  providers: [
    AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
