import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DialogService {
  
  private dialogVisible = new BehaviorSubject<boolean>(false);
  dialogVisible$ = this.dialogVisible.asObservable();

  openDialog() {
    this.dialogVisible.next(true);
  }

  closeDialog() {
    this.dialogVisible.next(false);
  }


  private cartItems = new BehaviorSubject<any[]>([]);
  cartItems$ = this.cartItems.asObservable();


  addToCart(item: any) {
    let currentCart = this.cartItems.getValue();
    const existingItem = currentCart.find(cartItem => cartItem.id === item.id);

    if (existingItem) {
      existingItem.quantity += 1;
    } else {
      currentCart = [...currentCart, { ...item, quantity: 1 }];
    }

    this.cartItems.next(currentCart);
  }

  getTotalPrice(): number {
    return this.cartItems.getValue().reduce((acc, item) => acc + (item.price * item.quantity), 0);
  }

  getItemQuantity(itemId: number): number {
    const item = this.cartItems.getValue().find(cartItem => cartItem.id === itemId);
    return item ? item.quantity : 0;
  }
}
