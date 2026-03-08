import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { NgOtpInputComponent } from 'ng-otp-input';
import { HttpHelper } from '../../../Services/http-helper';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-verify',
  imports: [RouterLink, CommonModule, NgOtpInputComponent],
  templateUrl: './verify.html',
  styleUrl: './verify.css',
})
export class Verify {

  constructor(private httpHelper: HttpHelper, private router: Router){}

  mailOtp: string = "";
  email: string = "";

  toastr = inject(ToastrService);

  ngOnInit()
  {
    this.mailOtp = history.state.otp;
    this.email = history.state.email;

    console.log("call",this.email,this.mailOtp);
    
  }

  otp: string = "";

  config = {
    length: 6,
    inputClass: 'otp-input',
    disableAutoFocus: false,
    inputStyles : {
      'width': '35px',
      'height': '35px'
    }
  };

  onOtpChange(value: string)
  {
    this.otp = value;
    if(this.otp.length==6)
    {
      return;
    }
  }

  verifyOTP()
  {
    console.log("OTP-> ",this.otp,this.mailOtp,this.email);
    if(this.otp===this.mailOtp)
    {
      this.router.navigate(['/change-password'],{
        state: {email: this.email}, replaceUrl: true
      });
    }
    else{
      this.toastr.error("OTP is incorrect",'Error');
    }
  }

}
