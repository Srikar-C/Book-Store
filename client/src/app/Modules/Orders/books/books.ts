import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { HttpHelper } from '../../../Services/http-helper';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpHeaders } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-books',
  imports: [CommonModule],
  templateUrl: './books.html',
  styleUrl: './books.css',
  standalone: true
})
export class Books {

  books: any[] = [];
  carts: any[] = [];


  num: number = 0;

  toastr = inject(ToastrService);

  kindOfUser : string = localStorage.getItem('userEmail') === 'admin' ? 'admin' : 'user';


  constructor(private httpHelper: HttpHelper, private router: Router, private cd: ChangeDetectorRef) { }

  
  ngOnInit() {
    console.log(this.kindOfUser);
    this.getBooks();
  }

  getBooks()
  {
    var apiUrl = 'http://localhost:5128/api';
    this.httpHelper.post(apiUrl, 'books/getBooks', {})
    .subscribe({
      next: (response: any) => {
        console.log('Books retrieved successfully:', response);
        this.books = response.data;
        console.log('Books array:', this.books);
        this.cd.detectChanges();
      },
      error: (error) => {
        console.error('Failed to retrieve books:', error);
        this.toastr.error(error.error.message,'Error');
      }
    });
  }

  increment(book: any)
  {
    if(book.count+1<=book.quantity){
      book.count = book.count + 1;
    }
    else{
      this.toastr.warning((book.count+1)+'Books are not available','Warning');
    }
  }

  decrement(book: any)
  {
    if(book.count>0){
      book.count = book.count - 1;
    }
    else
    {
      this.removeCart(book);
    }
  }

  removeCart(book: any) {
    book.selected = false;
    
    this.carts = this.carts.filter(b=> b.id !== book.id);

    console.log('Removing from cart:', book);
  }

  addToCart(book: any) {
    book.selected = true;
    
    this.num = this.num+1;

    if(!this.carts.find(b=>b.id === book.id)) {
      this.carts.push(book);
      book.count = book.count + 1;
    }

    console.log('Adding to cart:', book);
  }

  checkOut()
  {
    var apiUrl = 'http://localhost:5284/api'; 
    const token = localStorage.getItem('token');
    console.log('Carts to checkout:', this.carts);
    var payload = this.carts;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
    this.httpHelper.post(apiUrl, 'cart/addToCart', payload, { headers: headers })
    .subscribe({
      next: (response) => {
        console.log('Carts added successfully:', response);
        this.router.navigate(['/home/carts'])
      },
      error: (error) => {
        console.error('Failed to add carts:', error);
        this.toastr.error(error.error.message,'Error');
      }
    });
  }

  editBook(book: any)
  {
    this.router.navigate(['home/editBook'],{
      state: {book: book, type: 1}, replaceUrl: true
    })
  }
}
