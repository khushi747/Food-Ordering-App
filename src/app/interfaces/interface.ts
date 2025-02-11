export interface OrderStatus {
    statusId: number;
    status: string;
}
  
  export interface Order {
    status: string;
    orderId: number;
    userName: string;
    orderDate: string;
    totalPrice: number;
    orderStatuses: OrderStatus[];
  }
  
  export interface OrderItem {
    orderId: number;
    itemsDetails: string;
    totalQuantity: number;
  }
  
  export interface MenuItem {
    itemId: number;
    name: string;
    price: number;
    quantity: number;
    image: string;
  }
  
  export interface ParsedOrderItem {
    ItemId: number;
    Quantity: number;
  }

  
  