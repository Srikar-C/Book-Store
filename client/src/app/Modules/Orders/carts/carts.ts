import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { HttpHelper } from '../../../Services/http-helper';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpHeaders } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-carts',
  imports: [CommonModule],
  templateUrl: './carts.html',
  styleUrl: './carts.css',
})
export class Carts {

  carts: any[] = [];

  kindOfUser : string = localStorage.getItem('userEmail') === 'admin' ? 'admin' : 'user';

  toastr = inject(ToastrService);

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
        this.toastr.error(error.error.message,'Error');
      }
    });
  }

  decrement(carts: any)
  {
    if(carts.count>0){
      carts.count = carts.count - 1;
      var apiUrl = 'http://localhost:5284/api'; 
      const token = localStorage.getItem('token');
      console.log('Removing from cart:', carts);
      var payload = { bookId: carts.id };
      const headers = new HttpHeaders({
        Authorization: `Bearer ${token}`
      });
      this.httpHelper.delete(apiUrl, `cart/decrementFromCart/${carts.id}`, payload, { headers: headers })
      .subscribe({
        next: (response) => {
          console.log('Book removed from cart successfully:', response);
          this.getCarts();
        },
        error: (error) => {
          console.error('Failed to remove book from cart:', error);
          this.toastr.error(error.error.message,'Error');
        }
      });
    }
    else
    {
      this.removeFromCart(carts);
    }
  }

  placeOrder()
  {
    console.log("placed order");
    var apiUrl = 'http://localhost:5284/api'; 
    const token = localStorage.getItem('token');
    console.log('Carts to checkout:', this.carts);
    var payload = this.carts;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
    this.httpHelper.post(apiUrl, 'order/placeOrder', payload, { headers: headers })
    .subscribe({
      next: (response) => {
        console.log('Order placed successfully:', response);
        this.getCarts();
        this.router.navigate(['/home/orders']);
      },
      error: (error) => {
        console.error('Failed to place order:', error);
        this.toastr.error(error.error.message,'Error');
      }
    });
  }

  removeFromCart(carts: any) {
    var apiUrl = 'http://localhost:5284/api'; 
    const token = localStorage.getItem('token');
    console.log('Removing from cart:', carts);
    var payload = { bookId: carts.id };
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
    this.httpHelper.delete(apiUrl, `cart/removeFromCart/${carts.id}`, payload, { headers: headers })
    .subscribe({
      next: (response) => {
        console.log('Book removed from cart successfully:', response);
        this.getCarts();
      },
      error: (error) => {
        console.error('Failed to remove book from cart:', error);
        this.toastr.error(error.error.message,'Error');
      }
    });
  }
}
