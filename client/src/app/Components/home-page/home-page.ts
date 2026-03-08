import { Component, inject } from '@angular/core';
import { NavigationEnd, Router, RouterLink, RouterOutlet } from '@angular/router';
import { HttpHelper } from '../../Services/http-helper';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { filter, interval } from 'rxjs';

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

  toastr = inject(ToastrService);

  isOpen = false;

  handleAside() {
    this.isOpen = !this.isOpen;
  }

  selectNav(index: number) {
    this.selectedIndex = index;
    this.isOpen = false;
  }

  ngOnInit() {

    this.updateSlider();

    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.updateSlider();
      });

  }

  updateSlider() {

    const url = this.router.url;

    if (url.includes('books')) {
      this.selectedIndex = 0;
    }
    else if (url.includes('orders')) {
      this.selectedIndex = 1;
    }
    else if (url.includes('carts')) {
      this.selectedIndex = 2;
    }

  }

  logout()
  {
    var apiUrl = 'http://localhost:5227/api'; 
    this.httpHelper.post(apiUrl, 'auth/logout', {})
    .subscribe({
      next: (response) => {
        console.log('Logout successful:', response);
        localStorage.removeItem('token');
        this.router.navigate(['/login'],{ replaceUrl: true })
      },
      error: (error) => {
        console.error('Logout failed:', error);
        this.toastr.error(error.error.message,'Error');
      }
    });
  }

  addBook()
  {
    this.router.navigate(['/home/addBook'],{
      state:{book:"", type:0}, replaceUrl: true 
    })
  }

  profile()
  {
    this.selectedIndex = 4;
    this.isOpen = false;
    this.router.navigate(['/home/profile'],{
      state: {type: 1}
    });
  }

}
