import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpHelper } from '../../../Services/http-helper';
import { featherAirplay } from '@ng-icons/feather-icons';
import { heroUsers } from '@ng-icons/heroicons/outline';
import { CommonModule } from '@angular/common';
import { NgIcon, provideIcons } from '@ng-icons/core';

@Component({
  selector: 'app-signup',
  imports: [RouterLink, FormsModule, NgIcon, CommonModule],
  templateUrl: './signup.html',
  styleUrl: './signup.css',
  standalone: true,
  viewProviders: [provideIcons({ featherAirplay, heroUsers })]
})
export class Signup {

  constructor(private httpHelper : HttpHelper, private router : Router){}

  username : string = "";
  email : string = "";
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

  onRegister()
  {
    console.log(this.username+" "+this.email+" "+this.password+" "+this.cfn_password);
    
    this.httpHelper.register({username:this.username,email:this.email,password:this.password,cfn_password:this.cfn_password})
    .subscribe({
      next : (resp)=>{
        console.log("Registration successful", resp);
        this.router.navigate(['/login']);
      },
      error: (error)=>{
        console.error("Error in registration",error.error.message, error);
        let msg = error.error.message;
        alert(msg);
      }
    })
  }

}
