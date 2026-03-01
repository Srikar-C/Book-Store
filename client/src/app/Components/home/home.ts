import { Component } from '@angular/core';
import { HttpHelper } from '../../Services/http-helper';
import { ActivatedRoute, Router, RouterLink, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  imports: [CommonModule, RouterLink, RouterOutlet],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {

  constructor(private httpHelper: HttpHelper, private route: ActivatedRoute, private router: Router){}

  books: any[] = [];

  carts: any[] = [];

  userid: string | null = "";
  username: string | null = "";

  removeCart(book: any)
  {
    book.bookSelect = false;

    this.carts = this.carts.filter(b=> b.id !== book.id);
  }

  addCart(book: any)
  {
    console.log("book.id-> ",book.id);
    book.bookSelect = true;

    if(!this.carts.find(b=>b.id === book.id)) {
      this.carts.push(book);
    }
  }

  ngOnInit()
  {
    this.route.paramMap.subscribe((params) => {
      this.username = params.get('username');
      this.getBooks();
    });
  }


  getBooks()
  {
    this.userid = localStorage.getItem('userid');
    console.log("userid in frontend-> "+this.userid);
    this.httpHelper.getBooks(this.userid).subscribe({
      next: (response)=>{
        console.log("data-> ",response);
        this.books = response;
      },
      error: (error)=>{
        console.log("Error in fetching all books",error);
      }
    })
  }

  checkOut()
  {
    this.userid = localStorage.getItem('userid');
    this.username = localStorage.getItem('username');
    const cartdata = {
      userId: this.userid,
      Carts: this.carts
    };
    console.log("userid in frontend-> ",cartdata);
    this.httpHelper.addToCarts(cartdata).subscribe({
      next: (response)=>{
        console.log("data-> ",response, this.books);
        if (Array.isArray(this.books)) {
          this.books.forEach(b=> b.bookSelect = false);
        }
        this.carts = [];
        this.router.navigate([this.username,'cart']);
      },
      error: (error)=>{
        console.log("Error in checkout cart",error);
      }
    })
  }

}
