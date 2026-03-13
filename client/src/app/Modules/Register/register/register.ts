import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { HttpHelper } from '../../../Services/http-helper';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-register',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {

  constructor(private httpHelper : HttpHelper, private router: Router, private spinner: NgxSpinnerService, private cdr: ChangeDetectorRef) {}
  
  username: string = "";
  email : string = "";
  password : string = "";

  usernameBorderColor = '2px solid black';
  emailBorderColor = '2px solid black';
  passwordBorderColor = '2px solid black';

  usernameError: string = "";
  emailError: string = "";
  passwordError: string = "";

  loader: boolean = false;


  showPassword : boolean = false;

  toastr = inject(ToastrService);

  toggleView()
  {
    this.showPassword = !this.showPassword;
  } 

  onUsernameFocus()
  {
    this.usernameBorderColor = '2px solid blue';
  }

  onUsernameBlur()
  {
    if(this.username.length<=0){
      this.usernameError = "Email/Username is mandatory"
      this.usernameBorderColor = '2px solid red';
    }
    else {
      this.usernameError = "";
      this.usernameBorderColor = '2px solid green';
    }
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

  register()
  {
    if(this.usernameError.length<0 || this.emailError.length<0 || this.passwordError.length<0){
      this.toastr.warning('Please Fill the required fields', 'Warning');
      return;
    }

    if(this.username.length<=0 || this.email.length<=0 || this.password.length<=0){
      this.toastr.warning('Please Fill the required fields', 'Warning');
      return;
    }

    this.loader = true;
    var apiUrl = 'http://localhost:5227/api'; 
    var payload = { Username: this.username, Email: this.email, Password: this.password };
    this.httpHelper.post(apiUrl, 'auth/register', payload)
    .subscribe({
      next: (response) => {
        console.log('Registration stored in cache successful:', response);
        this.loader = false;
        this.cdr.detectChanges();
        localStorage.setItem('userEmail', this.email);
        setTimeout(() => {
          this.router.navigate(['/verify'],{ state:{type:1,email:this.email,otp:response.user.password,username :response.user.username}, replaceUrl: true })
        });
      },
      error: (error) => {
        this.loader = false;
        this.cdr.detectChanges();
        console.error('Registration failed:', error);
        setTimeout(() => {
          this.toastr.error(error.error.message, 'Error');
        });
      }
    });
  }
}
