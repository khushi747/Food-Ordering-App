import { Component, inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../services/api.service';
import { NavbarComponent } from '../navbar/navbar.component';
import { ItemCardComponent } from '../item-card/item-card.component';
import { Subscription } from 'rxjs';
import { UserService } from '../../../services/shared/user.service';
import { Title } from '@angular/platform-browser';
import { RabbitMQService } from '../../../services/rabbitmq.service';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [NavbarComponent, ItemCardComponent, CommonModule],

  templateUrl: './userdashboard.component.html',
  styleUrl: './userdashboard.component.css',
})
export class UserdashboardComponent implements OnInit {
 
  products: any[] = [];
  // userId = 0;

  http = inject(HttpClient);
  title = 'Users';

  constructor(
    private apiser: ApiService,
    private rabbitMQService: RabbitMQService,
    private titleService: Title,
    // private userService: UserService
  ) {}


  
   ngOnInit() {

    // this.userService.userId$.subscribe((id) => {
    //   if (id) {
    //     this.userId = id;
    //     console.log('User ID received in navbar:', this.userId);
    //   }
    // });

    this.apiser.fetchProducts().subscribe({
      next: (data) => (this.products = data),
      error: (error) => console.error(error),
    });
    this.titleService.setTitle('User Screen');
    
  }


  handleDelete(product: any) {
    if (confirm(`Are you sure you want to delete ${product.name}?`)) {
      this.http
        .delete(`http://10.13.106.18/api/Admin/menu/${product.itemId}`)
        .subscribe({
          next: (data) =>
            this.products.filter((p) => p.itemId !== product.itemId),
          error: (error) => console.error(error),
          complete: () =>
            console.log(`${product.name} has been deleted successfully.`),
        });
    }
  }
}
