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
    ExamEditComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'questions', component: QuestionsComponent },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'profile', component: ProfileComponent },
      {
        path: 'exams',
        children: [
          { path: '', component: ExamListComponent },
          { path: 'create', component: ExamCreateComponent },
          { path: 'attempt/:id', component: ExamAttemptComponent },
          { path: 'result/:id', component: ExamResultComponent },
          { path: 'edit/:id', component: ExamEditComponent }
        ]
      }
    ]),

    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers: [
    AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
