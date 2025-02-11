import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { RippleModule } from 'primeng/ripple';
import { Subscription } from 'rxjs';
import { DialogService } from '../../../services/shared/dialog.service';
import { CartService } from '../../../services/shared/cart.service';

interface Product {
  itemId: number;
  name: string;
  price: number;
  quantity: number;
  image: string;
}

@Component({
  standalone: true,
  selector: 'app-item-card',
  templateUrl: './item-card.component.html',
  styleUrl: './item-card.component.css',
  imports: [CardModule, ButtonModule, CommonModule, DialogModule, FormsModule, RippleModule, ToastModule],
  providers: [MessageService]
})
export class ItemCardComponent implements OnInit, OnDestroy {

  constructor(private messageService: MessageService, private http: HttpClient,private dialogService: DialogService,private cartService: CartService) {}

  quantity: number = 0;
  private cartSubscription: Subscription = new Subscription();


  ngOnDestroy() {
    this.cartSubscription.unsubscribe();
  }
  ngOnInit() {
    // Subscribe to cart updates and fetch item quantity
    this.cartSubscription = this.cartService.cartItems$.subscribe((cartItems) => {
      const cartItem = cartItems.find((cart) => cart.itemId === this.product?.itemId);
      this.quantity = cartItem ? cartItem.quantity : 0;
    });
  }

  @Input() product!: Product;
  @Output() delete = new EventEmitter<Product>();

  // displayDialog: boolean = false;

  // menuItem: Product = { 
  //   itemId: 0,
  //   name: '', 
  //   price: 0,
  //   quantity: 0, 
  //   image: '' 
  // };

  // openDialog(product: Product) {
  //   this.menuItem = { ...product }; 
  //   this.displayDialog = true;
  // }

  addToCart() {
    this.cartService.addToCart({
      itemId: this.product.itemId,
      name: this.product.name,
      price: this.product.price,
      quantity: 1,
      image: this.product.image,
    });
    this.dialogService.openDialog();  
    console.log('Opening dialog');
    
  }


  showSuccess(detail: string) {
    this.messageService.add({ severity: 'success', summary: 'Success', detail });
  }

  updateQuantity( change: number) {
    this.cartService.updateQuantity(this.product.itemId, change);
  }
}
