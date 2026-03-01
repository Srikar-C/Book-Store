import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpHelper } from '../../../Services/http-helper';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { featherAirplay } from '@ng-icons/feather-icons';
import { heroUsers } from '@ng-icons/heroicons/outline';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-forgot-password',
  imports: [RouterLink, FormsModule, NgIcon, CommonModule],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.css',
  standalone: true,
  viewProviders: [provideIcons({ featherAirplay, heroUsers })]
})
export class ForgotPassword {

  constructor(private httpHelper: HttpHelper, private router: Router){}

  username : string = "";
  password : string = "";
  cfn_password : string = "";

  showPassword1 : boolean = false;
  showPassword2 : boolean = false;

  toggleMtd1()
  {
    console.log(this.showPassword1);
    this.showPassword1 = !this.showPassword1;
  }
  
  toggleMtd2()
  {
    console.log(this.showPassword2);
    this.showPassword2 = !this.showPassword2;
  }

  onReset()
  {
    this.httpHelper.reset({username:this.username, password:this.password, cfn_password: this.cfn_password})
    .subscribe({
      next: (response) => {
        console.log("successfully reset",response);
        this.router.navigate(['/login']);
      },
      error: (error)=>{
        console.log("Error in resetting",error.error.message);
        var msg = error.error.message;
        alert(msg);
      }
    })
  }


}
