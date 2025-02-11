import { ActivatedRouteSnapshot, CanActivate, GuardResult, MaybeAsync, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from '../services/login.service';

@Injectable ({
  providedIn: 'root'
})

export class loginGuard implements  CanActivate{
  constructor(private loginService: LoginService, private router: Router) { }
  canActivate(): boolean {
    const role = this.loginService.getUserRole();
    this.loginService.getUserRole();
    if(role){
      return true;
    }else{
      this.router.navigate(['/login']);
      return false;
    }
  }
}