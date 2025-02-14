import { Component, inject, Input, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Menubar } from 'primeng/menubar';
import { BadgeModule } from 'primeng/badge';
import { AvatarModule } from 'primeng/avatar';
import { InputTextModule } from 'primeng/inputtext';
import { CommonModule } from '@angular/common';
import { Ripple } from 'primeng/ripple';
import { DrawerModule } from 'primeng/drawer';
import { ButtonModule } from 'primeng/button';
import { HttpClient } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { CartService } from '../../../services/shared/cart.service';
import { DialogService } from '../../../services/shared/dialog.service';
import { UserService } from '../../../services/shared/user.service';
import { RabbitMQService } from '../../../services/rabbitmq.service';
import { LoginService } from '../../../services/login.service';


interface CartItem {
  itemId: number;
  name: string;
  price: number;
  quantity: number;
  image: string;
}

interface Order {
  orderId: number;
  userId: number;
  orderDate: string;
  totalPrice: number;
  orderdetails: OrderDetail[];
  orderstatuses: OrderStatus[];
}

interface OrderDetail {
  orderId: number;
  itemId: number;
  itemsDetails: string;
  totalQuantity: number;
  totalPrice: number;
  orderdetailId: number;
}

interface OrderStatus {
  statusId: number;
  orderId: number;
  status: string;
}

interface Item {
  itemId: number;
  name: string;
  price: number;
  quantity: number;
  image: string;
}

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    Menubar,
    BadgeModule,
    AvatarModule,
    InputTextModule,
    Ripple,
    CommonModule,
    DrawerModule,
    ButtonModule,
    ToastModule,
  ],
  providers: [MessageService],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  cartItems: CartItem[] = [];
  visible2 = false;
  visible3: boolean = false;
  private apiUrl = 'http://10.13.106.18/api/User/createOrder';

  constructor(
    private cartService: CartService,
    private dialogService: DialogService,
    private messageService: MessageService,
    private http: HttpClient,
    private userService: UserService,
    private rabbitMQService: RabbitMQService,
    private loginService : LoginService,
  ) {}

  items: MenuItem[] | undefined;
  totalPrice: number = 0;

  orders: Order[] = [];
  userId = 0;
  itemMap: { [key: number]: string } = {};

  ngOnInit() {
    this.userService.userId$.subscribe((id) => {
      if (id) {
        this.userId = id;
        console.log('User ID received in navbar:', this.userId);
      }
    });

    this.cartService.getCartItems().subscribe((items) => {
      this.cartItems = items;
    });

    this.items = [{ label: 'Home', icon: 'pi pi-home' }];

    this.dialogService.dialogVisible$.subscribe((isVisible) => {
      this.visible2 = isVisible;
    });

    this.dialogService.cartItems$.subscribe((cartItems) => {
      this.totalPrice = cartItems.reduce(
        (acc, item) => acc + item.price * item.quantity,
        0
      );
    });

    this.rabbitMQService.orderStatus$.subscribe((update) => {
      if (update) {
        this.updateOrderStatus(update.orderId, update.status);
      }
    });
  }

  updateOrderStatus(orderId: number, newStatus: string) {
    const order = this.orders.find((o) => o.orderId === orderId);
    if (order) {
      console.log(`Updating order ${orderId} to status: ${newStatus}`);
      order.orderstatuses[order.orderstatuses.length - 1].status = newStatus;
    }
  }

  openDialog() {
    this.dialogService.openDialog();
  }

  updateQuantity(itemId: number, change: number) {
    this.cartService.updateQuantity(itemId, change);
  }

  removeItem(itemId: number) {
    this.cartService.removeItem(itemId);
  }

  getTotalPrice(): number {
    return this.cartItems.reduce(
      (acc, item) => acc + item.price * item.quantity,
      0
    );
  }

  placeOrder() {
    if (this.cartItems.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Warning',
        detail: 'Cart is empty!',
      });
      return;
    }

    const orderdetails = {
      userId: this.userId, // Use dynamic userId
      items: this.cartItems.map((item) => ({
        itemId: item.itemId,
        quantity: item.quantity,
      })),
    };

    this.http.post(this.apiUrl, orderdetails).subscribe({
      next: (response) => {
        console.log('Order placed successfully:', response);
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Order placed successfully!',
        });
        this.cartService.clearCart();
        this.visible2 = false;
      },
      error: (error) => {
        console.error('Error placing order:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to place order. Try again!',
        });
        this.visible2 = false;
      },
    });
  }

  openProfile() {
    this.visible3 = true;
    this.fetchOrders();
    this.fetchItemDetails();
  }

  getTotalQuantity(order: Order): number {
    return order.orderdetails.reduce((acc, d) => acc + d.totalQuantity, 0);
  }

  fetchOrders() {
    this.http
      .get<Order[]>(`http://10.13.106.18/api/User/orders/${this.userId}`)
      .subscribe({
        next: (response) => {
          this.orders = response.map((order) => ({
            ...order,
            orderdetails: order.orderdetails.map((detail) => {
              try {
                const extractediteams = JSON.parse(detail.itemsDetails); // Parse JSON string
                return {
                  ...detail,
                  itemId: extractediteams.ItemId, // Extract ItemId
                  quantity: extractediteams.Quantity, // Extract Quantity
                  itemsDetails:
                    this.itemMap[extractediteams.ItemId] || 'Unknown Item', // Map ItemId to name
                };
              } catch (error) {
                console.error('Error parsing itemsDetails:', error);
                return { ...detail, itemsDetails: 'Invalid Data' };
              }
            }),
          }));
        },
        error: (error) => {
          console.error('Error fetching orders:', error);
        },
      });
  }

  fetchItemDetails() {
    this.http.get<Item[]>('http://10.13.106.18/api/Admin/menu').subscribe({
      next: (items) => {
        this.itemMap = items.reduce((map, item) => {
          map[item.itemId] = item.name;
          return map;
        }, {} as { [key: number]: string });

        this.mapItemNames();
      },
      error: (error) => console.error('Error fetching item details:', error),
    });
  }

  mapItemNames() {
    if (this.orders.length > 0 && Object.keys(this.itemMap).length > 0) {
      this.orders.forEach((order) => {
        order.orderdetails.forEach((detail) => {
          if (detail.itemId in this.itemMap) {
            detail.itemsDetails = this.itemMap[detail.itemId];
          } else {
            detail.itemsDetails = 'Unknown Item';
          }
        });
      });
    }
  }
  onLogout() {
    this.loginService.onLogout();
  }
}
