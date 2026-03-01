import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { HttpHelper } from '../../../Services/http-helper';
import { FormsModule } from '@angular/forms';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { featherAirplay } from '@ng-icons/feather-icons';
import { heroUsers } from '@ng-icons/heroicons/outline';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [RouterLink, FormsModule, NgIcon, CommonModule],
  templateUrl: './login.html', 
  styleUrl: './login.css',
  standalone: true,
  viewProviders: [provideIcons({ featherAirplay, heroUsers })]
})
export class Login {

  constructor(private httpHelper: HttpHelper, private router: Router){}

  username: string = "";
  pass: string = "";
  role: string = ""; 

  showPassword : boolean = false;

  toggleMtd()
  {
    console.log(this.showPassword);
    this.showPassword = !this.showPassword;
  }

  onLogin()
  {
    this.httpHelper.login({Username:this.username,Password:this.pass})
    .subscribe({
        next: (response) => {
          console.log('Login Successful:', response);
          localStorage.setItem('token',response.token);
          localStorage.setItem('userid', response.id);
          localStorage.setItem('username',this.username);
          this.httpHelper.saveToken(response.token);
          console.log("token--> :"+response.token);
          this.router.navigate(['/home'],{ replaceUrl: true })
        },
        error: (error) => {
          console.error('Error:', error.error.message,error);
          var msg = error.error.message;
          alert(msg);
        }
      });
  }

}
