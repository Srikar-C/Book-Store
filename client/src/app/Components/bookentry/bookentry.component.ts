import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { HttpHelper } from '../../Services/http-helper';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-bookentry',
  imports: [CommonModule, FormsModule],
  templateUrl: './bookentry.component.html',
  styleUrl: './bookentry.component.css'
})
export class BookentryComponent {

  constructor(private httpHelper: HttpHelper, private router: Router) { }

  title: string = "";
  author: string = "";
  url: string = "";
  price: number = 0;


  onAddBook() {
    var payload = { Title: this.title, Author: this.author, Url: this.url, Price: this.price };
    console.log("Payload-> ", payload);
    var apiUrl = 'http://localhost:5128/api';
    this.httpHelper.post(apiUrl, "books/addBooks", payload)
    .subscribe({
      next: (response) => {
        console.log("Book added successfully!", response);
        alert('Book added successfully!');
        this.router.navigate(['/home/books']);
      },
      error: (error) => {
        console.error("Error adding book:", error);
        alert('Error adding book. Please try again.');
      }
    });
  }

}
