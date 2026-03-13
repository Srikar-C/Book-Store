import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
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

  constructor(private httpHelper: HttpHelper, private router: Router, private cdr: ChangeDetectorRef){}

  mailOtp: string = "";
  email: string = "";
  type: number = 2;
  username: string = "";

  loader: boolean = false;

  toastr = inject(ToastrService);

  ngOnInit()
  {
    this.mailOtp = history.state.otp;
    this.email = history.state.email;
    this.type = history.state.type;
    this.username = history.state.username;

    console.log("call",this.email,this.mailOtp,this.type);
    
    if(this.otp==null || this.mailOtp==null || this.email==null)
    {
      this.router.navigate(['/login']);
      return;
    }
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
    if(this.otp.length<=0){
      this.toastr.warning('Please Fill the required fields', 'Warning');
      return;
    }

    if(this.otp===this.mailOtp)
    {
      var apiUrl = 'http://localhost:5227/api'; 
      if(this.type == 0){
        this.httpHelper.post(apiUrl,'auth/verify',{Password: this.otp, Email: this.email})
        .subscribe({
          next: (response)=>{
            console.log('response fro verification-> ',response);
            this.router.navigate(['/change-password'],{
              state: {email: this.email}, replaceUrl: true
            });
          },
          error: (error)=>{
            console.log('error',error);
          }
        });
      }
      else if(this.type==1)
      {
        console.log("payload-> ",this.username);
        this.httpHelper.post(apiUrl,'auth/completeRegistration',{Email:this.username})
        .subscribe({
          next: (response)=>{
            console.log('response fro verification-> ',response);
            this.loader = false;
            this.cdr.detectChanges();
            setTimeout(()=>{
              this.router.navigate(['/login'],{ replaceUrl: true});
            });
          },
          error: (error)=>{
            
        this.loader = false;
        this.cdr.detectChanges();
            console.log('error',error);
          }
        });
      }
    }
    else{
      this.toastr.error("OTP is incorrect",'Error');
    }
  }

}
