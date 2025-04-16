import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  username = '';
  email = '';
  password = '';
  message = '';
  error = '';

  constructor(private auth: AuthService, private router: Router) {}

  register() {
    this.auth.register({ username: this.username, email: this.email, password: this.password }).subscribe({
      next: res => {
        this.auth.login({ email: this.email, password: this.password }).subscribe({
          next: loginRes => {
            this.auth.saveTokens(loginRes.accessToken, loginRes.refreshToken);
            this.router.navigate(['/profile']);
          },
          error: loginErr => {
            this.message = res;
            this.error = 'Registrierung war erfolgreich, aber Login schlug fehl.';
            console.error('Login nach Registrierung fehlgeschlagen:', loginErr);
          }
        });
      },
      error: err => {
        console.error('Registrierung fehlgeschlagen:', err);

        if (err.status === 0) {
          this.error = 'Server nicht erreichbar! Läuft das Backend?';
        } else if (typeof err.error === 'string') {
          this.error = err.error;
        } else if (err.error?.message) {
          this.error = err.error.message;
        } else {
          this.error = 'Registrierung fehlgeschlagen – Details siehe Konsole.';
        }
      }
    });
  }
}
