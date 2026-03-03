import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { HttpHelper } from '../../Services/http-helper';
import { ActivatedRoute, Router, RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-carts',
  imports: [CommonModule, RouterLink, RouterOutlet],
  templateUrl: './carts.html',
  styleUrl: './carts.css',
})
export class Carts {

  constructor(private httpHelper: HttpHelper,private route: ActivatedRoute, private router: Router){}

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
    const orderload = {
      UserId: this.userid,
      Orders: this.carts,
    }
    this.httpHelper.setOrders(orderload).subscribe({
      next: (response)=>{
        console.log("data-> ",response);
        this.carts = response;
        this.carts = [];
        this.router.navigate(['/home/orders']);
      },
      error: (error)=>{
        console.log("Error in fetching all carts",error);
      }
    })
  }

}
