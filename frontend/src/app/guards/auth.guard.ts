import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { LoginService } from '../services/login.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  
  constructor(private loginService: LoginService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const token = this.loginService.getToken(); // Get stored token
    const userRole = this.loginService.getUserRole(); // Get stored role

    if (!token) {
      // No token, redirect to login
      this.router.navigate(['/login']);
      return false;
    }

    // Role-based protection
    const requiredRole = route.data['role'];
    if (requiredRole && requiredRole !== userRole) {
      alert('Unauthorized Access');
      this.router.navigate(['/login']);
      return false;
    }

    return true;
  }
}
