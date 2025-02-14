import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { OrderCardComponent } from '../order-card/order-card.component';
import { CommonModule } from '@angular/common';
import { forkJoin, of } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { Title } from '@angular/platform-browser';
import { OrderService } from '../../../services/order.service';
import { RabbitMQService } from '../../../services/rabbitmq.service';
import {
  Order,
  OrderItem,
  MenuItem,
  ParsedOrderItem,
} from '../../../interfaces/interface';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [NavbarComponent, OrderCardComponent, CommonModule],
  templateUrl: './kitchendashboard.component.html',
  styleUrl: './kitchendashboard.component.css',
})
export class KitchendashboardComponent implements OnInit {
   getStatus(status: string): string {
      if (status === 'completed') {
        return 'completed-class';
      } else if (status === 'pending') {
        return 'pending-class';
      } else {
        return 'default-class';
      }
    }
  orderWithItems: any[] = [];
  reversedOrderWithItems: any[] = [];
  loading = true;
  error = '';
  

  constructor(
    private titleService: Title,
    private orderService: OrderService,
    private rabbitMQService: RabbitMQService
  ) {}

  ngOnInit(): void {
    this.loadOrders();
    this.titleService.setTitle('Chef Screen');

    this.rabbitMQService.orderUpdates$.subscribe((newOrder) => {
      if (newOrder) {
        console.log('New order received:', newOrder);
        this.addNewOrder(newOrder);
      }
    });
  }

 loadOrders(): void {
  this.orderService
    .getOrders()
    .pipe(
      switchMap((orders) => {
        if (!orders || orders.length === 0) {
          return of([[], []]); // Ensure fallback
        }

        const menuItems$ = this.orderService.getMenuItems();
        const ordersWithItems$ = forkJoin(
          orders.map((order) =>
            this.orderService.getOrderItems(order.orderId).pipe(
              map((orderItems) => ({
                order,
                orderItems,
              }))
            )
          )
        );

        return forkJoin([of(orders), menuItems$, ordersWithItems$]);
      })
    )
    .subscribe({
      next: ([orders, menuItems, ordersWithItemsArray]) => {
        this.orderWithItems = ordersWithItemsArray.map((orderWithItems) => {
          if (!orderWithItems.orderItems) {
            return { order: orderWithItems.order, items: [] };
          }

          const items = orderWithItems.orderItems.map((item: OrderItem) => {
            let parsedDetails: ParsedOrderItem;
            try {
              parsedDetails = JSON.parse(item.itemsDetails);
            } catch (error) {
              console.error('Error parsing item details:', error);
              return { name: 'Unknown Item', quantity: 0 };
            }

            const menuItem = menuItems.find(
              (m: MenuItem) => m.itemId === parsedDetails.ItemId
            );

            return {
              name: menuItem?.name || 'Unknown Item',
              quantity: parsedDetails.Quantity,
            };
          });

          return {
            order: orderWithItems.order,
            items,
          };
        });

        this.reversedOrderWithItems = [...this.orderWithItems].reverse();
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading orders:', err);
        this.error = 'Failed to load orders';
        this.loading = false;
      },
    });
}


  addNewOrder(newOrder: any) {
    const formattedItems = newOrder.items.map((item: any) => ({
      name: 'Fetching...', // Will update once menu items are fetched
      quantity: item.quantity,
    }));

    const newOrderWithItems = {
      order: {
        orderId: newOrder.orderId,
        userId: newOrder.userId,
        orderDate: newOrder.orderDate,
        totalPrice: newOrder.totalPrice,
        status: newOrder.status,
      },
      items: formattedItems,
    };

    // Fetch menu item names
    this.orderService.getMenuItems().subscribe((menuItems) => {
      newOrderWithItems.items = newOrder.items.map((item: any) => {
        const menuItem = menuItems.find(
          (m: MenuItem) => m.itemId === item.itemId
        );
        return {
          name: menuItem ? menuItem.name : 'Unknown Item',
          quantity: item.quantity,
        };
      });

      // Add the new order at the beginning of the list
      this.orderWithItems.unshift(newOrderWithItems);
      this.reversedOrderWithItems = [...this.orderWithItems];
    });
  }
}
