import { Component, inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../services/api.service';
import { NavbarComponent } from '../navbar/navbar.component';
import { ItemCardComponent } from '../item-card/item-card.component';

@Component({
  selector: 'app-root',
  standalone : true,
  imports: [NavbarComponent, ItemCardComponent,CommonModule],
  templateUrl: './userdashboard.component.html',
  styleUrl: './userdashboard.component.css'
})
export class UserdashboardComponent implements OnInit{

  constructor(private apiser:ApiService) {}

  ngOnInit() {
    this.apiser.fetchProducts().subscribe({
      next: (data) => this.products = data,
      error: (error)=>console.error(error)
    })
  }

  products:any[] =[];

  http = inject(HttpClient);
  title = 'Users';



  handleDelete(product: any) {
    if (confirm(`Are you sure you want to delete ${product.name}?`)) {
      this.http.delete(`http://10.13.106.18/api/Admin/menu/${product.itemId}`)
      .subscribe({
        next: (data) => this.products.filter(p => p.itemId !== product.itemId),
        error: (error)=>console.error(error),
        complete: () => console.log(`${product.name} has been deleted successfully.`)
      });
        
    }
  }
}
