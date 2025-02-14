import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
 
@Injectable({
  providedIn: 'root', // Makes this service globally available
})
export class LoginService {
  private apiUrl = 'http://10.13.106.17:5158/api/account/login';
  private tokenKey = 'authToken'; // Ensure consistency in token storage
  private roleKey = 'userRole';
 
  constructor(private http: HttpClient, private router: Router) {}
 
  login(loginData: any): Observable<any> {
    return this.http.post(this.apiUrl, loginData);
  }
 
  saveAuthData(token: string, role: string) {
    if (typeof window !== 'undefined' && localStorage) {
      localStorage.setItem(this.tokenKey, token);
      localStorage.setItem(this.roleKey, role);
    }
  }
 
  getToken(): string | null {
    if (typeof window !== 'undefined' && localStorage) {
      return localStorage.getItem(this.tokenKey);
    }
    return null;
  }
 
  getUserRole(): string | null {
    if (typeof window !== 'undefined' && localStorage) {
      return localStorage.getItem(this.roleKey);
    }
    return null;
  }
 
  isLoggedIn(): boolean {
    return !!this.getToken();
  }
 
  // Logout function
   onLogout() {
     localStorage.removeItem(this.tokenKey);
     localStorage.removeItem(this.roleKey);
     this.router.navigate(['/login']); // Redirect to login after logout
   }
}
 