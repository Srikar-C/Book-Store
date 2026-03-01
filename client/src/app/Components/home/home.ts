import { Component } from '@angular/core';
import { HttpHelper } from '../../Services/http-helper';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {

  constructor(private httpHelper: HttpHelper){}

  ngOnInit()
  {
    this.httpHelper.getProfile().subscribe({
      next: (response) => {
        console.log("getting user details"+ response);
      },
      error: (error)=>{
        console.log("error-> "+error);
      }
    })
  }

}
