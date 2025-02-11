// import { HttpClient } from '@angular/common/http';
// import { Injectable } from '@angular/core';
// import { Router } from '@angular/router';
// import { CommonModule } from '@angular/common';
// import { Observable } from 'rxjs';

// @Injectable({
//   providedIn: 'root' // Makes this service globally available
// })

// export class LoginService {
//   private apiUrl='http://10.13.106.17:5158/api/account/login';
//   private tokenKey = 'authToken';
//   private roleKey = 'userRole';

//   constructor(private http:HttpClient, private router: Router) { }

//   // Function to make a POST request to login API
//   // The login() function uses HttpClient to send a POST request to the backend API.
//   // Returns an Observable which is subscribed to in loginpage.component.ts.
//   login(loginData: any):Observable<any> {
//     return this.http.post(this.apiUrl, loginData);
//   }

//   // Function to store token and role in local storage
//   saveAuthData(token:string, role:string){
//     localStorage.setItem('token', token);
//     localStorage.setItem('role', role);
//   }

//   // Function to retrieve user role from local storage
//   getUserRole():string|null{
//     return localStorage.getItem('role');  
//   }

//   getToken(): string | null {
//     return localStorage.getItem(this.tokenKey);
//   }

//   getRole(): string | null {
//     return localStorage.getItem(this.roleKey);
//   }

//   logout() {
//     localStorage.removeItem(this.tokenKey);
//     localStorage.removeItem(this.roleKey);
//   }


// }


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
