import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { LoginService } from '../services/login.service';

@Injectable({
  providedIn: 'root',
})
export class LoginGuard implements CanActivate {
  constructor(private loginService: LoginService, private router: Router) {}

  canActivate(): boolean {
    const role = this.loginService.getUserRole();

    if (role) {
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
}
