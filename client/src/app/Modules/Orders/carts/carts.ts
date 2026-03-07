import { ChangeDetectorRef, Component } from '@angular/core';
import { HttpHelper } from '../../../Services/http-helper';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-carts',
  imports: [CommonModule],
  templateUrl: './carts.html',
  styleUrl: './carts.css',
})
export class Carts {

  carts: any[] = [];

  kindOfUser : string = localStorage.getItem('userEmail') === 'admin' ? 'admin' : 'user';

  constructor(private httpHelper: HttpHelper, private router: Router, private cd: ChangeDetectorRef) { }

  ngOnInit() {
    this.getCarts();
  }

  getCarts()
  {
    var apiUrl = 'http://localhost:5284/api';
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
    this.httpHelper.post(apiUrl, 'cart/getCart', {}, { headers : headers })
    .subscribe({
      next: (response: any) => {
        console.log('Carts retrieved successfully:', response);
        this.carts = response.data;
        console.log('Carts array:', this.carts);
        this.cd.detectChanges();
      },
      error: (error) => {
        console.error('Failed to retrieve carts:', error);
      }
    });
  }

  placeOrder()
  {
    console.log("placed order");
    var url = 'http://localhost:5284/api'; 
    const token = localStorage.getItem('token');
    console.log('Carts to checkout:', this.carts);
    var payload = this.carts;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
    this.httpHelper.post(url, 'order/placeOrder', payload, { headers: headers })
    .subscribe({
      next: (response) => {
        console.log('Order placed successfully:', response);
        this.getCarts();
      },
      error: (error) => {
        console.error('Failed to place order:', error);
      }
    });
  }

  removeFromCart(book: any) {
    var url = 'http://localhost:5284/api'; 
    const token = localStorage.getItem('token');
    console.log('Removing from cart:', book);
    var payload = { bookId: book.id };
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
    this.httpHelper.delete(url, `cart/removeFromCart/${book.id}`, payload, { headers: headers })
    .subscribe({
      next: (response) => {
        console.log('Book removed from cart successfully:', response);
        this.getCarts();
      },
      error: (error) => {
        console.error('Failed to remove book from cart:', error);
      }
    });
  }
}
