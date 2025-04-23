import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email = '';
  password = '';
  error = '';
  keepLoggedIn = true;

  constructor(private auth: AuthService, private router: Router) {}

  login() {
    this.auth.login({ email: this.email, password: this.password }).subscribe({
      next: res => {
        this.auth.saveTokens(res.accessToken, res.refreshToken, this.keepLoggedIn);
        this.router.navigate(['/profile']);
      },
      error: err => {
        this.error = err.error?.toString() ?? 'Unbekannter Fehler beim Login';
      }
    });
  }
}
