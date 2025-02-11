import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { RippleModule } from 'primeng/ripple';

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
export class ItemCardComponent {
  constructor(private messageService: MessageService, private http: HttpClient) {}

  @Input() product!: Product;
  @Output() delete = new EventEmitter<Product>();

  displayDialog: boolean = false;

  menuItem: Product = { 
    itemId: 0,
    name: '', 
    price: 0,
    quantity: 0, 
    image: '' 
  };

  openDialog(product: Product) {
    this.menuItem = { ...product }; 
    this.displayDialog = true;
  }

  updateProduct() {
    this.http.put(`http://10.13.106.18/api/Admin/menu/${this.menuItem.itemId}`, this.menuItem)
      .subscribe({
        next: () => {
          this.product = { ...this.menuItem };
          this.displayDialog = false;
          this.showSuccess(`${this.menuItem.name} has been updated successfully.`);
        },
        error: (error) => {
          console.error('Error updating product:', error);
        }
      });
  }

  handleDelete() {
    this.delete.emit(this.product);
    this.showSuccess(`${this.menuItem.name} has been Deleted successfully.`);
  }

  showSuccess(detail: string) {
    this.messageService.add({ severity: 'success', summary: 'Success', detail });
  }
}
