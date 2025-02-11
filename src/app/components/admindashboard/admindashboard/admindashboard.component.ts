import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
 import { error } from 'console';
 import { response } from 'express';
import { ApiService } from '../../../services/api.service';
import { NavbarComponent } from '../navbar/navbar.component';
import { ItemCardComponent } from '../item-card/item-card.component';
import { AddComponent } from '../add/add.component';
 

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, NavbarComponent, ItemCardComponent, AddComponent],
  templateUrl: './admindashboard.component.html',
  styleUrls: ['./admindashboard.component.css']
})
export class AdmindashboardComponent implements OnInit {
  products:any[] =[];
  

  http = inject(HttpClient);

  constructor(private apiser:ApiService) {}

  ngOnInit() {
    this.apiser.fetchProducts().subscribe({
      next: (data) => this.products = data,
      error: (error)=>console.error(error)
    })
  }

  // data => {
  //   this.products = data;
  // }, error => {
  //   console.error('Error fetching products:', error);
 
  // fetchProducts() {
  //   this.http.get<any[]>('http://10.13.106.18/api/Admin/menu').subscribe({
  //     next: (data) => this.products = data,
  //     error: (error)=>console.error(error)
  //   });
  // }

  // Handle Delete Event from ItemCardComponent
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
  // handleDelete(product: any) {
  //   this.apiser.handleDelete(product)
  //     .subscribe({
  //       next: (data) => this.products.filter(p => p.itemId !== product.itemId),
  //       error: (error)=>console.error(error),
  //       complete: () => console.log(`${product.name} has been deleted successfully.`)
  //     });
        
  //   }
  // }
  // Handle Update Event from ItemCardComponent
  // handleUpdate(product: any) {
  //   const newPrice = prompt(`Enter new price for ${product.name}:`, product.price);
  //   const newQuantity = prompt(`Enter new quantity for ${product.name}:`, product.quantity);
  
  //   if (newPrice !== null && newQuantity !== null) {
  //     const updatedProduct = {
  //       ...product,
  //       price: parseFloat(newPrice),         // Ensure price is a number
  //       quantity: parseInt(newQuantity, 10)  // Ensure quantity is an integer
  //     };
  
  //     this.http.put(`http://10.13.106.18/api/Admin/menu/${product.itemId}`, updatedProduct)
  //       .subscribe({
  //         next: (data) => {
  //           // Update local products list after successful API call
  //           this.products = this.products.map(p => 
  //             p.itemId === product.itemId ? updatedProduct : p
  //           );
  //         },
  //         error: (error) => console.error('Error updating product:', error),
  //         complete: () => console.log(`${product.name} has been updated successfully.`)
  //       });
  //   }
  // }
  
}

