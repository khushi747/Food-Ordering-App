import { RouterModule, Routes } from '@angular/router';
import { LoginpageComponent } from './components/login/loginpage/loginpage.component';
import { AdmindashboardComponent } from './components/admindashboard/admindashboard/admindashboard.component';
import { UserdashboardComponent } from './components/userdashboard/userdashboard/userdashboard.component';
import { KitchendashboardComponent } from './components/kitchendashboard/kitchendashboard/kitchendashboard.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginpageComponent },

  // Kitchen Dashboard (For kitchen staff only)
  {
    path: 'kitchen',
    component: KitchendashboardComponent,
    canActivate: [AuthGuard],
    data: { role: 'chef' },
  },

  // Admin Dashboard (For admins only)
  {
    path: 'admin',
    component: AdmindashboardComponent,
    canActivate: [AuthGuard],
    data: { role: 'admin' },
  },

  // User Dashboard (For normal users only)
  {
    path: 'user',
    component: UserdashboardComponent,
    canActivate: [AuthGuard],
    data: { role: 'user' },
  },
];

// Export RouterModule instance to be imported in app.component.ts
export const AppRouting = RouterModule.forRoot(routes);
