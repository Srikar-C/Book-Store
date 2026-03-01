import { Component } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { HttpHelper } from '../../Services/http-helper';

@Component({
  selector: 'app-home-page',
  imports: [RouterLink, RouterOutlet],
  templateUrl: './home-page.html',
  styleUrl: './home-page.css',
})
export class HomePage { 

  constructor(private httpHelper: HttpHelper, private router: Router){}

  userid : string = "";

  isOpen = true;

  handleAside() {
    this.isOpen = !this.isOpen;
  }


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
