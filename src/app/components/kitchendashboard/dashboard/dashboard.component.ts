import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { OrderCardComponent } from '../order-card/order-card.component';
import { CommonModule } from '@angular/common';
// import { Order, OrderItem, MenuItem, ParsedOrderItem } from '../../interface/interface';
import { forkJoin, of } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { HttpClientModule } from '@angular/common/http';
import { OrderService } from '../../../services/order.service';
import { Order, OrderItem, MenuItem, ParsedOrderItem } from '../../../interfaces/interface';


@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [ NavbarComponent, OrderCardComponent,CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})

export class DashboardComponent implements OnInit {
 

  getStatus(status: string): string {


    if (status === 'completed') {

      return 'completed-class';

    } else if (status === 'pending') {

      return 'pending-class';

    } else {

      return 'default-class';

    }

  }
  
  title(title: any) {
    throw new Error('Method not implemented.');
  } 
  orderWithItems: any[] = [];
    loading = true;
    error = '';
  
    constructor(private orderService: OrderService) {}
  
    ngOnInit(): void {
      this.loadOrders();
    }
  
    loadOrders(): void {
      this.orderService.getOrders().pipe(
        switchMap(orders => {
          if (orders.length === 0) {
            return of([[], [], []]);
          }
          
          const menuItems$ = this.orderService.getMenuItems();
          const ordersWithItems$ = orders.map(order => 
            this.orderService.getOrderItems(order.orderId).pipe(
              map(orderItems => ({
                order,
                orderItems
              }))
            )
          );
  
          return forkJoin([
            of(orders),
            menuItems$,
            ...ordersWithItems$
          ]);
        })
      ).subscribe({
        next: (results) => {
          const [orders, menuItems, ...ordersWithItemsArray] = results as [Order[], MenuItem[], ...any[]];
          
          this.orderWithItems = ordersWithItemsArray.map((orderWithItems: any) => {
            const items = orderWithItems.orderItems.map((item: OrderItem) => {
              const parsedDetails: ParsedOrderItem = JSON.parse(item.itemsDetails);
              const menuItem = menuItems?.find((m: MenuItem) => m.itemId === parsedDetails.ItemId);
              return {
                name: menuItem?.name || 'Unknown Item',
                quantity: parsedDetails.Quantity
              };
            });
  
            return {
              order: orderWithItems.order,
              items
            };
          });
          
          this.loading = false;
        },
        error: (err) => {
          console.error('Error loading orders:', err);
          this.error = 'Failed to load orders';
          this.loading = false;
        }
      });
    }
  }
 