import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpHelper } from '../../Services/http-helper';

@Component({
  selector: 'app-orders',
  imports: [CommonModule],
  templateUrl: './orders.html',
  styleUrl: './orders.css',
})
export class Orders {

  constructor(private httpHelper: HttpHelper,private route: ActivatedRoute, private router: Router){}

  orders: any[] = [];

  userid: string | null = "";

  removeorder(data: any)
  {

  }

  ngOnInit()
  {
    this.route.paramMap.subscribe((params) => {
      this.getOrders();
    });
  }

  getOrders()
  {
    this.userid = localStorage.getItem('userid');
    console.log("userid in frontend-> "+this.userid);
    this.httpHelper.getOrders(this.userid).subscribe({
      next: (response)=>{
        console.log("data-> ",response);
        this.orders = response;
      },
      error: (error)=>{
        console.log("Error in fetching all orders",error);
      }
    })
  }

}
