import { Component } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { HttpHelper } from '../../Services/http-helper';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home-page',
  imports: [RouterLink, RouterOutlet, CommonModule],
  templateUrl: './home-page.html',
  styleUrl: './home-page.css',
})
export class HomePage {

  selectedIndex: number = 0;
  kindOfUser : string = localStorage.getItem('userEmail') === 'admin' ? 'admin' : 'user';

  constructor(private httpHelper : HttpHelper, private router: Router) {}

  selectNav(index: number) {
    this.selectedIndex = index;
  }

  ngOnInit() {
    const url = this.router.url;

    if (url.includes('orders')) {
      this.selectedIndex = 1;
    }
    else if (url.includes('carts')) {
      this.selectedIndex = 2;
    }
    else {
      this.selectedIndex = 0;
    }
  }

  logout()
  {
    alert('clicked');
    var url = 'http://localhost:5227/api'; 
    this.httpHelper.post(url, 'auth/logout', {})
    .subscribe({
      next: (response) => {
        console.log('Logout successful:', response);
        localStorage.removeItem('token');
        this.router.navigate(['/login'],{ replaceUrl: true })
      },
      error: (error) => {
        console.error('Logout failed:', error);
      }
    });
  }

  addBook()
  {
    this.router.navigate(['/home/addBook'],{ replaceUrl: true })
  }

}
