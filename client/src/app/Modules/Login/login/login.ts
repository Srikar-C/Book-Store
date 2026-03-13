import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpHelper } from '../../../Services/http-helper';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login { 

  constructor(private httpHelper : HttpHelper, private router: Router, private cdr: ChangeDetectorRef) {}

  email : string = "";
  password : string = "";
  emailBorderColor = '2px solid black';
  passwordBorderColor = '2px solid black';

  emailError: string = "";
  passwordError: string = "";

  toastr = inject(ToastrService);

  showPassword : boolean = false;

  loader: boolean = false;

  toggleView()
  {
    this.showPassword = !this.showPassword;
  }

  onEmailFocus()
  {
    this.emailBorderColor = '2px solid blue';
  }

  onEmailBlur()
  {
    if(this.email.length<=0){
      this.emailError = "Email/Username is mandatory"
      this.emailBorderColor = '2px solid red';
    }
    else {
      this.emailError = "";
      this.emailBorderColor = '2px solid green';
    }
  }

  onPasswordFocus()
  {
    this.passwordBorderColor = '2px solid blue';
  }

  onPasswordBlur()
  {
    if(this.password.length<=0){
      this.passwordError = "Password is mandatory"
      this.passwordBorderColor = '2px solid red';
    }
    else {
      this.passwordError = "";
      this.passwordBorderColor = '2px solid green';
    }
  }

  login()
  {
    if(this.emailError.length<0 || this.passwordError.length<0){
      this.toastr.warning('Please Fill the required fields', 'Warning');
      return;
    }

    if(this.email.length<=0 || this.password.length<=0){
      this.toastr.warning('Please Fill the required fields', 'Warning');
      return;
    }
    this.loader = true;
    var apiUrl = 'http://localhost:5227/api'; 
    var payload = { Email: this.email, Password: this.password };
    console.log("Payload-> ", payload);
    this.httpHelper.post(apiUrl, 'auth/login', payload)
    .subscribe({
      next: (response: any) => {
        console.log('Login successful:', response);
        localStorage.setItem('token', response.token);
        localStorage.setItem('userEmail', this.email);
        this.loader = false;
        this.cdr.detectChanges();
        setTimeout(()=>{
          this.router.navigate(['/home/books'],{ replaceUrl: true })
        })
      },
      error: (error) => {
        this.loader = false;
        this.cdr.detectChanges();
        console.error('Login failed:', error);
        this.toastr.error(error.error.message, 'Error');
      }
    });
  }


}
