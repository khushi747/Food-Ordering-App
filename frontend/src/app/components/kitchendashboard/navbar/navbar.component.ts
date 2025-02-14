import { ButtonModule } from 'primeng/button';
import { Input, Output, EventEmitter } from '@angular/core';
import { MenubarModule } from 'primeng/menubar';
import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { BadgeModule } from 'primeng/badge';
import { AvatarModule } from 'primeng/avatar';
import { InputTextModule } from 'primeng/inputtext';
import { CommonModule } from '@angular/common';
import { Ripple } from 'primeng/ripple';
import { LoginService } from '../../../services/login.service';
@Component({
  selector: 'app-navbar',
  imports: [
    MenubarModule,
    BadgeModule,
    AvatarModule,
    InputTextModule,
    Ripple,
    CommonModule,
  ],
  standalone: true,
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit {
  items: MenuItem[] | undefined;
  constructor(private loginService: LoginService) {}
  ngOnInit() {}
  onLogout() {
    this.loginService.onLogout();
  }
}
