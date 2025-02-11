import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import {Router} from '@angular/router';
import {jwtDecode} from 'jwt-decode';
import { JwtPayload } from 'jwt-decode';
import {  HostListener } from '@angular/core';
import { LoginService } from '../../../services/login.service';

interface CustomJwtPayload extends JwtPayload {
  role: string;
}
 
function decodeToken(token: string): CustomJwtPayload {
  return jwtDecode<CustomJwtPayload>(token);
}

@Component({
  selector: 'app-loginpage',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './loginpage.component.html',
  styleUrls: ['./loginpage.component.css']
})

// Object to hold login form data
export class LoginpageComponent {
  loginData: any={
    name: '',
    password: '',
  };

  //submit form on pressing enter
  @HostListener('document:keydown.enter', ['$event'])
  handleEnter(event: KeyboardEvent) {
    this.onSubmitLoginData();
  }
  // Injecting LoginService and Router so that we can use them in this component
  constructor( private loginService: LoginService,private router : Router) { }

  // Function called when login button is clicked
  onSubmitLoginData(){    
    console.log(this.loginData);

    //Calls loginService.login(loginData), which sends a request to the backend.
    this.loginService.login(this.loginData).subscribe({
      next:(response:any)=>{
        const token=response.token;
        const decoded = decodeToken(token);
        console.log('details are',decoded);
        console.log('Role is', decoded.role);
        const role=decoded.role;
        if(response && response.token){
          // Save token and role to local storage
          this.loginService.saveAuthData(response.token, role);
              // Redirect based on role
          switch(role){
            case 'admin':
              console.log('Navigating to /admin');
              this.router.navigate(['/admin']);
              break;
            case 'chef':
                console.log('Navigating to /kitchen');
                this.router.navigate(['/kitchen']);
                break;
            default:
              console.log('Navigating to /users');
              this.router.navigate(['/users']);             
          }
          
        }
      },
      error: (err: any) => {
        alert('Error: Invalid Credentials');
        console.error('Login Failed', err);
      }
    });
  }

  
}
