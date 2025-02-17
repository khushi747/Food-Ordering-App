import { Component, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { OrderService } from '../../../services/order.service';



interface Order {
  orderId: number;
  userName: string;
  orderStatuses: { status: string }[];
}

interface OrderWithItems {
  order: Order;
  items: { name: string; quantity: number }[];
}

@Component({
  selector: 'app-order-card',
  standalone: true,
  imports: [CommonModule, ButtonModule, CardModule],
  templateUrl: './order-card.component.html',
  styleUrls: ['./order-card.component.css'],
})
export class OrderCardComponent {
  @Input() orderWithItem!: OrderWithItems;

  constructor(private http: HttpClient, private orderService: OrderService) {}

  getStatusClassTop(): string {
    if (
      !this.orderWithItem?.order?.orderStatuses ||
      this.orderWithItem.order.orderStatuses.length === 0
    ) {
      return 'ordertop-status-pending';
    }

    const status = this.orderWithItem.order.orderStatuses[0].status;
    switch (status) {
      case 'Completed':
        return 'ordertop-status-completed';
      case 'In Progress':
        return 'ordertop-status-inprep';
      default:
        return 'ordertop-status-pending';
    }
  }

  getStatusClass(): string {
    if (
      !this.orderWithItem?.order?.orderStatuses ||
      this.orderWithItem.order.orderStatuses.length === 0
    ) {
      return 'orderbtn-status-pending';
    }

    const status = this.orderWithItem.order.orderStatuses[0].status;
    switch (status) {
      case 'Completed':
        return 'orderbtn-status-completed';
      case 'In Progress':
        return 'orderbtn-status-inprep';
      default:
        return 'orderbtn-status-pending';
    }
  }

  updateStatus() {
    if (this.orderWithItem.order.orderStatuses.length > 0) {
      let currentStatus = this.orderWithItem.order.orderStatuses[0].status;
      let newStatusdb = '';
      let newStatus = '';

      if (currentStatus === 'Pending') {
        newStatusdb = 'In Progress';
        newStatus = 'In Progress';
      } else if (currentStatus === 'In Progress') {
        newStatusdb = 'Completed';
        newStatus = 'Completed';
      } else {
        return;
      }

      this.orderWithItem.order.orderStatuses[0].status = newStatus;
      let orderId = this.orderWithItem.order.orderId;

      const updatedOrder = {
        status: newStatusdb,
      };

      this.orderService.updateOrderStatus(orderId, updatedOrder).subscribe({
        next: (response) =>
          console.log('Order status updated successfully:', response),
        error: (error) => console.error('Error updating order status:', error),
      });
    }
  }
}

