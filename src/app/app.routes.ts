import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { DashboardComponent } from './components/kitchendashboard/dashboard/dashboard.component';
import { LoginpageComponent } from './components/login/loginpage/loginpage.component';
import { AdmindashboardComponent } from './components/admindashboard/admindashboard/admindashboard.component';
import { UserdashboardComponent } from './components/userdashboard/userdashboard/userdashboard.component';
import { KitchendashboardComponent } from './components/kitchendashboard/kitchendashboard/kitchendashboard.component';

export const routes: Routes = [
    {   path: '',
        redirectTo: 'login',
        pathMatch: 'full',
    },{
        path: 'login',
        component: LoginpageComponent,
    },{
        path: 'kitchen',
        component: KitchendashboardComponent,
    },
    {
        path: 'admin',
        component : AdmindashboardComponent,
    },
    {
        path:'user',
        component: UserdashboardComponent
    }
    
];


@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })
  export class AppRoutingModule { }
  

