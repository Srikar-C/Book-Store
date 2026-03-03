import { Component } from '@angular/core';
import { HttpHelper } from '../../Services/http-helper';
import { Router, RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-home-page',
  imports: [RouterLink, RouterOutlet],
  templateUrl: './home-page.html',
  styleUrl: './home-page.css',
  standalone: true,
})
export class HomePage {

  constructor(private httpHelper: HttpHelper, private router: Router){}

  userid : string = "";

  logout()
  {
    this.httpHelper.logout().subscribe({
      next: (response) => {
        console.log('Logout Successful:', response);
        localStorage.removeItem('token');
        this.router.navigate(['/login']);
      },
      error: (error)=>{
        console.error('Error:', error.error.message,error);

      }
    })
  }
}
