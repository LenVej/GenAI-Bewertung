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

  constructor(private auth: AuthService, private router: Router) {}

  login() {
    this.auth.login({ email: this.email, password: this.password }).subscribe({
      next: res => {
        this.auth.saveTokens(res.accessToken, res.refreshToken);
        this.router.navigate(['/profile']);
      },
      error: err => {
        if (err.status === 0) {
          this.error = 'Server nicht erreichbar – läuft das Backend?';
        } else if (err.error && typeof err.error === 'string') {
          this.error = err.error;
        } else {
          this.error = 'Unbekannter Fehler beim Login';
        }
      }

    });
  }
}
