//(order.service.ts) is responsible for handling API requests
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderItem, Order, OrderStatus, MenuItem } from '../interfaces/interface';
 
@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseUrl = 'http://10.13.106.18/api';

  constructor(private http: HttpClient) {}

  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.baseUrl}/chef/orders`);
  }

  getOrderItems(orderId: number): Observable<OrderItem[]> {
    return this.http.get<OrderItem[]>(`${this.baseUrl}/chef/orders/${orderId}/items`);
  }

  getMenuItems(): Observable<MenuItem[]> {
    return this.http.get<MenuItem[]>(`${this.baseUrl}/admin/menu`);
  }

  updateOrderStatus(orderId: number, updatedOrder:any): Observable<Order> {
    return this.http.put<Order>(`${this.baseUrl}/chef/orders/status/${orderId}`, updatedOrder);
  }
}
