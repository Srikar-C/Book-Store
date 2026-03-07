import { ChangeDetectorRef, Component } from '@angular/core';
import { HttpHelper } from '../../../Services/http-helper';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-orders',
  imports: [CommonModule],
  templateUrl: './orders.html',
  styleUrl: './orders.css',
})
export class Orders {

  orders : any[] = [];

  constructor(private httpHelper : HttpHelper, private router:Router, private cd: ChangeDetectorRef) {}

  ngOnInit()
  {
    this.getOrders();
  }

  getOrders()
  {
    var url = 'http://localhost:5284/api';
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
    this.httpHelper.get(url, 'order/getOrders', { headers : headers })
    .subscribe({
      next: (response: any) => {
        console.log('Orders retrieved successfully:', response);
        this.orders = response.data;
        console.log('Orders array:', this.orders);
        this.cd.detectChanges();
      },
      error: (error) => {
        console.error('Failed to retrieve orders:', error);
      }
    });
  }
}
