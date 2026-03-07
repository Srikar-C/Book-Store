import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpHelper } from '../../../Services/http-helper';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login { 

  constructor(private httpHelper : HttpHelper, private router: Router) {}

  email : string = "";
  password : string = "";
  url : string = "";

  showPassword : boolean = false;

  toggleView()
  {
    this.showPassword = !this.showPassword;
  }

  login()
  {
    alert('clicked');
    this.url = 'http://localhost:5227/api'; 
    var payload = { Email: this.email, Password: this.password };
    console.log("Payload-> ", payload);
    this.httpHelper.post(this.url, 'auth/login', payload)
    .subscribe({
      next: (response: any) => {
        console.log('Login successful:', response);
        localStorage.setItem('token', response.token);
        localStorage.setItem('userEmail', this.email);
        this.router.navigate(['/home/books'],{ replaceUrl: true })
      },
      error: (error) => {
        console.error('Login failed:', error);
        alert('Login failed: ' + (error.error?.message || 'Unknown error'));
      }
    });
  }


}
