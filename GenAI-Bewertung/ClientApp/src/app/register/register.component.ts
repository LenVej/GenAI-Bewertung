import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html'
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
        this.message = res;
        setTimeout(() => this.router.navigate(['/login']), 1500);
      },
      error: err => {
        console.error('Registrierung fehlgeschlagen:', err);

        if (err.status === 0) {
          this.error = 'Server nicht erreichbar – läuft das Backend?';
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
