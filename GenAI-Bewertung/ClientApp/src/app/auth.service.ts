import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import {environment} from "../environments/environment.local";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private api = `${environment.apiBaseUrl}/api/auth`;

  constructor(private http: HttpClient, private router: Router) {}

  login(credentials: { email: string, password: string }) {
    return this.http.post<any>(`${this.api}/login`, credentials);
  }

  register(data: { username: string, email: string, password: string }) {
    return this.http.post<any>(`${this.api}/register`, data);
  }

  saveTokens(access: string, refresh: string, remember: boolean = true) {
    const storage = remember ? localStorage : sessionStorage;
    storage.setItem('access_token', access);
    storage.setItem('refresh_token', refresh);
  }

  getAccessToken() {
    return localStorage.getItem('access_token') || sessionStorage.getItem('access_token');
  }

  logout() {
    localStorage.clear();
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    const token = this.getAccessToken();
    return !!token && !this.isTokenExpired(token);
  }


  deleteAccount() {
    return this.http.delete(`${this.api}/delete`);
  }

  isTokenExpired(token: string): boolean {
    if (!token) return true;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const expiry = payload.exp;
      const now = Math.floor(Date.now() / 1000);
      return expiry < now;
    } catch (e) {
      console.error('Invalid token:', e);
      return true;
    }
  }

}
