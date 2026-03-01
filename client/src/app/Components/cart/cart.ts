import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { HttpHelper } from '../../Services/http-helper';
import { ActivatedRoute, RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-cart',
  imports: [CommonModule, RouterLink, RouterOutlet],
  templateUrl: './cart.html',
  styleUrl: './cart.css',
})
export class Cart {

  constructor(private httpHelper: HttpHelper,private route: ActivatedRoute){}

  carts: any[] = [];

  username: string | null = "";
  userid: string | null = "";
  
  removeCart(book: any)
  {
    this.httpHelper.removeOne(book).subscribe({
      next: (response)=>{
        console.log("response after delete-> ",response);
        this.carts = response;
      },
      error: (error)=>{
        console.log("error in removing from cart",error);
      }
    })
  }

  ngOnInit()
  {
    this.route.paramMap.subscribe((params) => {
      this.username = params.get('username');
      this.getCarts();
    });
  }

  getCarts()
  {
    this.userid = localStorage.getItem('userid');
    console.log("userid in frontend-> "+this.userid);
    this.httpHelper.getCarts(this.userid).subscribe({
      next: (response)=>{
        console.log("data-> ",response);
        this.carts = response;
      },
      error: (error)=>{
        console.log("Error in fetching all carts",error);
      }
    })
  }

  orderCart()
  {
    this.userid = localStorage.getItem('userid');
    console.log("userid in frontend-> "+this.userid);
    this.httpHelper.setOrders(this.carts).subscribe({
      next: (response)=>{
        console.log("data-> ",response);
        this.carts = response;
      },
      error: (error)=>{
        console.log("Error in fetching all carts",error);
      }
    })
  }

}
