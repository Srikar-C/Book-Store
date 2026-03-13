import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HttpHelper } from '../../../Services/http-helper';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-forgot',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './forgot.html',
  styleUrl: './forgot.css',
})
export class Forgot {

  constructor(private httpHelper : HttpHelper, private router: Router, private cdr: ChangeDetectorRef) {}

  email : string = "";

  emailBorderColor = '2px solid black';
  emailError: string = "";

  toastr = inject(ToastrService);

  loader: boolean = false;

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

  sendOtp()
  {if(this.email.length<=0){
      this.toastr.warning('Please Fill the required fields', 'Warning');
      return;
    }

    var apiUrl = 'http://localhost:5227/api'; 
    var payload = {Email: this.email};
    console.log('Payload: ',payload);
    this.httpHelper.post(apiUrl,'auth/sendOtp',payload)
    .subscribe({
      next: (response)=>{
        console.log("otp-> :",response);
        this.loader = false;
        this.cdr.detectChanges();
        this.toastr.info('OTP sent to your Email','Info');
        setTimeout(()=>{
          this.router.navigate(['/verify'],{
            state: {otp: response.otp, email: response.email, type: 0},replaceUrl:true
          });
        })
      },
      error: (error)=>{
        this.loader = false;
        this.cdr.detectChanges();
        console.log("error-> ",error);
        this.toastr.error(error.error.message,'Error');
      }
    })
  }

}
