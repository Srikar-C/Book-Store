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

  name: string = "";
  pass: string = ""; 

  onLogin()
  {
    console.log(this.name+" "+this.pass);

    this.httpHelper.login({user:this.name,password:this.pass})
    .subscribe({
        next: (response) => {
          console.log('Success:', response);
          this.router.navigate(['/home-page'])
        },
        error: (error) => {
          console.error('Error:', error);
        }
      });
  }

}
