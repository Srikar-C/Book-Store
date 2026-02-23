import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { HttpHelper } from '../../../Services/http-helper';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [RouterLink, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {

  constructor(private httpHelper: HttpHelper, private router: Router){}

  username: string = "";
  pass: string = ""; 

  onLogin()
  {
    console.log(this.username+" "+this.pass);

    this.httpHelper.login({username:this.username,password:this.pass})
    .subscribe({
        next: (response) => {
          console.log('Login Successful:', response);
          this.router.navigate(['/home-page'])
        },
        error: (error) => {
          console.error('Error:', error.error.message,error);
          var msg = error.error.message;
          alert(msg.substring(2));
        }
      });
  }

}
