import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

interface CartItem {
  itemId: number;
  name: string;
  price: number;
  quantity: number;
  image: string;
}

@Injectable({
  providedIn: 'root',
})

export class CartService {
  private cartItems: CartItem[] = [];
  private cartItemsSubject = new BehaviorSubject<CartItem[]>([]);

  cartItems$ = this.cartItemsSubject.asObservable();

  addToCart(product: CartItem) {
    const existingItem = this.cartItems.find((item) => item.itemId === product.itemId);

    if (existingItem) {
      existingItem.quantity++;
    } else {
      this.cartItems.push({ ...product, quantity: 1 });
    }

    this.cartItemsSubject.next([...this.cartItems]);
  }

  getCartItems() {
    return this.cartItems$;
  }

  updateQuantity(itemId: number, change: number) {
    const item = this.cartItems.find((i) => i.itemId === itemId);
    if (item) {
      item.quantity += change;
      if (item.quantity <= 0) {
        this.cartItems = this.cartItems.filter((i) => i.itemId !== itemId);
      }
      this.cartItemsSubject.next([...this.cartItems]);
    }
  }

  removeItem(itemId: number) {
    this.cartItems = this.cartItems.filter((i) => i.itemId !== itemId);
    this.cartItemsSubject.next([...this.cartItems]);
  }

  clearCart() {
    this.cartItems = [];
    this.cartItemsSubject.next([]);
  }
  
}
