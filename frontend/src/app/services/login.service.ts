import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root', // Makes this service globally available
})
export class LoginService {
  private apiUrl = 'http://10.13.106.17:5158/api/account/login';
  private tokenKey = 'authToken';  // Ensure consistency in token storage
  private roleKey = 'userRole';

  constructor(private http: HttpClient, private router: Router) {}

  // Function to send login request to API
  login(loginData: any): Observable<any> {
    return this.http.post(this.apiUrl, loginData);
  }

  // Store authentication data in local storage
  saveAuthData(token: string, role: string) {
    localStorage.setItem(this.tokenKey, token);
    localStorage.setItem(this.roleKey, role);
  }

  // Get stored token
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  // Get stored user role
  getUserRole(): string | null {
    return localStorage.getItem(this.roleKey);
  }

  // Check if user is logged in
  isLoggedIn(): boolean {
    return !!this.getToken(); // Returns true if token exists
  }

  // Logout function
  logout() {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.roleKey);
    this.router.navigate(['/login']); // Redirect to login after logout
  }
}
