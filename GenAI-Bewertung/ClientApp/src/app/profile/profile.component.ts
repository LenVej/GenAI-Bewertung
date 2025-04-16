import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import {environment} from "../../environments/environment.local";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent {
  user: any = null;
  error: string = '';

  constructor(private http: HttpClient, private auth: AuthService, private router: Router) {
    this.loadProfile();
  }

  loadProfile() {
    this.http.get(`${environment.apiBaseUrl}/api/auth/profile`).subscribe({
      next: data => this.user = data,
      error: err => {
        this.error = 'Fehler beim Laden des Profils.';
        console.error(err);
      }
    });
  }

  logout() {
    this.auth.logout();
  }

  deleteUser() {
    if (confirm('Willst du dein Konto wirklich löschen?')) {
      this.auth.deleteAccount().subscribe(() => {
        this.auth.logout();
      });
    }
  }
}
