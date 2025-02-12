// import { Injectable } from '@angular/core';
// import { Client, IMessage } from '@stomp/stompjs';

// @Injectable({
//   providedIn: 'root',
// })
// export class RabbitmqService {
//   private stompClient: Client;
//   private isConnected: boolean = false;
//   private subscriptionCallback: ((message: string) => void) | null = null;

//   constructor() {
//     this.stompClient = new Client({
//       brokerURL: 'ws://localhost:15674/ws', // Web STOMP URL
//       reconnectDelay: 5000, // Auto-reconnect
//     });

//     this.stompClient.onConnect = () => {
//       console.log('✅ Connected to RabbitMQ via Web STOMP');
//       this.isConnected = true;

//       // Subscribe immediately if a callback was set before connection
//       if (this.subscriptionCallback) {
//         this.subscribeToOrderUpdates(this.subscriptionCallback);
//       }
//     };

//     this.stompClient.onStompError = (frame) => {
//       console.error('❌ STOMP Broker error:', frame.headers['message']);
//     };

//     this.stompClient.activate();
//   }

//   public subscribeToOrderUpdates(callback: (message: string) => void) {
//     this.subscriptionCallback = callback; // Store callback for later use

//     if (!this.isConnected) {
//       console.warn('⚠️ STOMP client not connected yet. Will subscribe after connection.');
//       return; // Wait until the connection is established
//     }

//     this.stompClient.subscribe('/queue/order-status-updates', (message: IMessage) => {
//       console.log('📩 Received order update:', message.body);
//       this.subscriptionCallback?.(message.body);
//     });
//   }
// }

import { Injectable } from '@angular/core';
import { Client, Message } from '@stomp/stompjs';



@Injectable({
  providedIn: 'root',
})
export class RabbitmqService {
  private stompClient!: Client;
  private isConnected = false;
  private subscriptionCallback: ((message: string) => void) | null = null;

  constructor() {
    this.initializeWebSocketConnection();
  }

  private initializeWebSocketConnection() {
    console.log('🚀 Initializing RabbitMQ STOMP connection...');
    
    this.stompClient = new Client({
      brokerURL: 'ws://localhost:15674/ws', // Web STOMP connection URL
      connectHeaders: {
        login: 'guest',
        passcode: 'guest',
      },
      debug: (msg: string) => console.log(`📡 STOMP: ${msg}`),
      reconnectDelay: 5000, // Auto-reconnect after 5 seconds
      heartbeatIncoming: 4000,
      heartbeatOutgoing: 4000,
    });

    this.stompClient.onConnect = () => {
      console.log('✅ Connected to RabbitMQ via Web STOMP');
      this.isConnected = true;
      if (this.subscriptionCallback) {
        this.subscribeToOrderUpdates(this.subscriptionCallback);
      }
    };

    this.stompClient.onStompError = (error) => {
      console.error('❌ STOMP Error:', error);
      this.isConnected = false;
      this.retrySubscription();
    };

    this.stompClient.activate();
  }

  subscribeToOrderUpdates(callback: (message: string) => void) {
    if (!this.isConnected) {
      console.warn('⚠️ STOMP client not connected yet. Retrying subscription...');
      this.subscriptionCallback = callback;
      this.retrySubscription();
      return;
    }

    console.log('📬 Subscribing to order status updates...');
    this.subscriptionCallback = callback;

    this.stompClient.subscribe('/queue/order-status-updates', (message: Message) => {
      console.log('📩 Received message:', message.body);
      callback(message.body);
    });
  }

  private retrySubscription() {
    setTimeout(() => {
      if (this.isConnected && this.subscriptionCallback) {
        console.log('🔄 Retrying subscription...');
        this.subscribeToOrderUpdates(this.subscriptionCallback);
      } else {
        console.warn('⏳ Still waiting for connection...');
        this.retrySubscription();
      }
    }, 3000); // Retry every 3 seconds
  }
}
