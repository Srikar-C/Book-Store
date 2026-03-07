import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { HttpHelper } from '../../../Services/http-helper';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {

  constructor(private httpHelper : HttpHelper, private router: Router) {}
  
  username: string = "";
  email : string = "";
  password : string = "";
  url: string = "";

  showPassword : boolean = false;

  toggleView()
  {
    this.showPassword = !this.showPassword;
  } 

  register()
  {
    alert('clicked');
    var apiUrl = 'http://localhost:5227/api'; 
    var payload = { Username: this.username, Email: this.email, Password: this.password };
    this.httpHelper.post(apiUrl, 'auth/register', payload)
    .subscribe({
      next: (response) => {
        console.log('Registration successful:', response);
        localStorage.setItem('userEmail', this.email);
        this.router.navigate(['/login'],{ replaceUrl: true })
      },
      error: (error) => {
        console.error('Registration failed:', error);
      }
    });
  }
}
