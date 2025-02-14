import { Injectable, inject } from '@angular/core';
import { Client } from '@stomp/stompjs';
import { UserService } from './shared/user.service';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RabbitMQService {
  private stompClient!: Client;
  private readonly rabbitMqUrl = 'ws://127.0.0.1:15674/ws';

  private orderStatusSubject = new BehaviorSubject<{
    orderId: number;
    status: string;
  } | null>(null);
  orderStatus$ = this.orderStatusSubject.asObservable();

  private orderUpdatesSubject = new BehaviorSubject<any | null>(null);
  orderUpdates$ = this.orderUpdatesSubject.asObservable();

  userService = inject(UserService);
  userId: number = 0;
  // role : string ='';

  constructor() {
    this.userService.userId$.subscribe((id) => {
      if (id) {
        this.userId = id;
        this.connect();
      }
    });

    // this.userService.role$.subscribe((role)=>{
    //   if(role==="chef"){
    //     this.role = role;
    //     this.connect
    //   }
    // })
  }

  private connect() {
    this.stompClient = new Client({
      brokerURL: this.rabbitMqUrl,
      connectHeaders: { login: 'guest', passcode: 'guest' },
      debug: (str) => console.log(str),
      reconnectDelay: 1000,
    });

    this.stompClient.onConnect = () => {
      console.log('Connected to RabbitMQ');

      // Subscribe based on user role
      this.userService.role$.subscribe((role) => {
        if (role === 'user') {
          this.subscribeToStatusQueue(); // Users get order status updates
        } else if (role === 'chef') {
          this.subscribeToOrderQueue(); // Chefs get new order notifications
        }
      });
    };

    this.stompClient.onStompError = (frame) => {
      console.error('STOMP Error:', frame);
    };

    this.stompClient.activate();
  }

  private subscribeToStatusQueue() {
    const queueName = `/queue/order-status-updates-user-${this.userId}`;

    if (!this.stompClient.connected) {
      console.error('STOMP Client is not connected');
      return;
    }

    console.log('Subscribing to:', queueName);

    this.stompClient.subscribe(queueName, (message) => {
      const receivedMessage = JSON.parse(message.body);
      const updatedStatus = receivedMessage.message.status;
      const orderId = receivedMessage.message.orderId;

      console.log(
        `Received status update: Order ${orderId} - ${updatedStatus}`
      );
      this.orderStatusSubject.next({ orderId, status: updatedStatus });
    });
  }

  private subscribeToOrderQueue() {
    const queueName = `/queue/order-create-user`;

    if (!this.stompClient.connected) {
      console.error('STOMP Client is not connected');
      return;
    }

    console.log('Subscribing to:', queueName);

    this.stompClient.subscribe(queueName, (message) => {
      const receivedMessage = JSON.parse(message.body);
      console.log('Received new order:', receivedMessage);

      const orderData = receivedMessage.message;

      const formattedOrder = {
        orderId: orderData.orderId,
        userId: orderData.userId,
        orderDate: orderData.orderDate,
        totalPrice: orderData.totalPrice,
        status: 'Pending', // Default to 'Pending'
        items: orderData.orderdetails.map((item: any) => {
        const parsedDetails = JSON.parse(item.itemsDetails);
          return {
            itemId: parsedDetails.ItemId,
            quantity: parsedDetails.Quantity,
            totalPrice: item.totalPrice,
          };
        }),
      };

      // Emit the formatted order to update the UI
      this.orderUpdatesSubject.next(formattedOrder);
    });
  }
}
