import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root' // Makes this service globally available
})

export class LoginService {
  private apiUrl='http://10.13.106.17:5158/api/account/login';
  constructor(private http:HttpClient, private router: Router) { }

  // Function to make a POST request to login API
  // The login() function uses HttpClient to send a POST request to the backend API.
  // Returns an Observable which is subscribed to in loginpage.component.ts.
  login(loginData: any):Observable<any> {
    return this.http.post(this.apiUrl, loginData);
  }

  // Function to store token and role in local storage
  saveAuthData(token:string, role:string){
    localStorage.setItem('token', token);
    localStorage.setItem('role', role);
  }

  // Function to retrieve user role from local storage
  getUserRole():string|null{
    return localStorage.getItem('role');  
  }

}
