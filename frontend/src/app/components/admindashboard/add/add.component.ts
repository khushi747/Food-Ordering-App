import { Component } from '@angular/core';
import { DialogModule } from 'primeng/dialog';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-add',
  imports: [FormsModule, DialogModule,ButtonModule],
  templateUrl: './add.component.html',
  styleUrl: './add.component.css'
})
export class AddComponent {
  displayDialog: boolean = false;  


  menuItem = {
    name: '',
    price: null,
    quantity: null,
    image: ''
  };

  constructor(private http: HttpClient) {}

  openDialog() {
    this.displayDialog = true;
  }

  createMenu() {
    const apiUrl = 'http://10.13.106.18/api/Admin/menu';
    
    this.http.post(apiUrl, this.menuItem).subscribe({
      next: (response) => {
        console.log('Menu created successfully:', response);
        this.displayDialog = false; 
      },
      error: (error) => {
        console.error('Error creating menu:', error);
      }
    });
  }

}
