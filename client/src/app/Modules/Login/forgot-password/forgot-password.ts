import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpHelper } from '../../../Services/http-helper';

@Component({
  selector: 'app-forgot-password',
  imports: [RouterLink, FormsModule],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.css',
})
export class ForgotPassword {


  constructor(private httpHelper: HttpHelper, private router: Router){}

  username : string = "";
  password : string = "";
  cfn_password : string = "";

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
        alert(msg.substring(2));
      }
    })
  }


}
