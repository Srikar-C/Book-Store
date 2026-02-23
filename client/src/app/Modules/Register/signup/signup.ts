import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpHelper } from '../../../Services/http-helper';

@Component({
  selector: 'app-signup',
  imports: [RouterLink, FormsModule],
  templateUrl: './signup.html',
  styleUrl: './signup.css',
})
export class Signup {

  constructor(private httpHelper : HttpHelper, private router : Router){}

  username : string = "";
  email : string = "";
  password : string = "";
  cfn_password : string = "";

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
        alert(msg.substring(2));

      }
    })
  }

}
