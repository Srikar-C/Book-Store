import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { HttpHelper } from '../../Services/http-helper';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-change-password',
  imports: [CommonModule, FormsModule],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css'
})
export class ChangePasswordComponent {

  constructor(private httpHelper: HttpHelper, private router: Router, private cdr: ChangeDetectorRef){}

  passwordError: string = "";
  cfnpasswordError: string = "";

  password: string = "";
  cfnpassword: string = "";

  showPassword: boolean = false;
  showCfnPassword: boolean = false;

  passwordBorderColor = '2px solid black';
  cfnPasswordBorderColor = '2px solid black';

  email: string = "";
  loader: boolean = false;

  toastr = inject(ToastrService);

  ngOnInit()
  {
    this.email = history.state.email;

    if(this.email==null)
    {
      this.router.navigate(['/login']);
      return;
    }
  }
  
  togglePasswordView()
  {
    this.showPassword = !this.showPassword;
  }
  
  toggleCfnPasswordView()
  {
    this.showCfnPassword = !this.showCfnPassword;
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

  onCfnPasswordFocus()
  {
    this.cfnPasswordBorderColor = '2px solid blue';
  }

  onCfnPasswordBlur()
  {
    if(this.cfnpassword.length<=0){
      this.cfnpasswordError = "Confirm Password is mandatory"
      this.cfnPasswordBorderColor = '2px solid red';
    }
    else {
      this.cfnpasswordError = "";
      this.cfnPasswordBorderColor = '2px solid green';
    }
  }

  changePassword()
  {
    if(this.cfnpassword.length<=0 || this.password.length<=0){
      this.toastr.warning('Please Fill the required fields', 'Warning');
      return;
    }

    console.log("password: "+this.password,this.cfnpassword,this.email);
    if(this.password===this.cfnpassword)
    {
      this.loader = true;
      var apiUrl = 'http://localhost:5227/api'; 
      var payload = { Email: this.email, Password: this.password };
      this.httpHelper.post(apiUrl,'auth/changePassword',payload)
      .subscribe({
        next: (response: any)=>{
          console.log('Password changed Successfully');
          this.loader = false;
          this.cdr.detectChanges();
          this.toastr.success('Password changed','Success');

          setTimeout(()=>{
            this.router.navigate(['/login'],{replaceUrl:true});
          })
        },
        error: (error)=>{
          this.loader = false;
          this.cdr.detectChanges();
          console.error('Failed to change password:', error);
          this.toastr.error(error.error.message, 'Error');
        }
      })
    }
    else{
      this.toastr.error("Password not match",'Error');
    }
  }

}
