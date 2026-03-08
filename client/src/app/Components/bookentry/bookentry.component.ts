import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { HttpHelper } from '../../Services/http-helper';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-bookentry',
  imports: [CommonModule, FormsModule],
  templateUrl: './bookentry.component.html',
  styleUrl: './bookentry.component.css'
})
export class BookentryComponent {

  constructor(private httpHelper: HttpHelper, private router: Router) { }

  toastr = inject(ToastrService);

  title: string = "";
  author: string = "";
  url: string = "";
  price: number = 0;
  quantity: number = 0;
  type: number = 0;
  id: string = "";

  book: any = "";

  ngOnInit()
  {
    this.book = history.state.book;
    this.type = history.state.type;
    
    this.title = this.book.title;
    this.author = this.book.author;
    this.url = this.book.url;
    this.price = this.book.price;
    this.quantity = this.book.quantity;
    this.id = this.book.id;

    
  }


  onAddBook() {
    var payload = { Title: this.title, Author: this.author, Url: this.url, Price: this.price, Quantity: this.quantity };
    console.log("Payload-> ", payload);
    var apiUrl = 'http://localhost:5128/api';
    this.httpHelper.post(apiUrl, "books/addBooks", payload)
    .subscribe({
      next: (response) => {
        console.log("Book added successfully!", response);
        this.toastr.success('Book added successfully!', 'Success');
        this.router.navigate(['/home/books']);
      },
      error: (error) => {
        console.error("Error adding book:", error);
        this.toastr.error('Error adding book. Please try again.', 'Error');
      }
    });
  }

  onEditBook()
  {
    var payload = { Title: this.title, Author: this.author, Url: this.url, Price: this.price, Quantity: this.quantity, Id: this.id };
    console.log("Payload-> ", payload);
    var apiUrl = 'http://localhost:5128/api';
    this.httpHelper.post(apiUrl, "books/editBook", payload)
    .subscribe({
      next: (response) => {
        console.log("Book added successfully!", response);
        this.toastr.success('Book edited successfully!', 'Success');
        this.router.navigate(['/home/books']);
      },
      error: (error) => {
        console.error("Error adding book:", error);
        this.toastr.error('Error editing book. Please try again.', 'Error');
      }
    });
  }

}
