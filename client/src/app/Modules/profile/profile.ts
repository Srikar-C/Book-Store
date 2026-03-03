import { Component, OnInit } from '@angular/core';
import { HttpHelper } from '../../Services/http-helper';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-profile',
  imports: [CommonModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
  standalone: true
})
export class Profile implements OnInit {

  constructor(private httpHelper : HttpHelper,private route: ActivatedRoute){}

  username : string = "";
  email : string = "";
  password : string = "";

  ngOnInit(): void
  {

    this.route.url.subscribe(() => {
      this.loadProfile();
    });
  }

  loadProfile() {
    this.httpHelper.getProfile().subscribe({
      next: (response) => {
        console.log("getting user details", response.user);
        var userdtls = response.user;
        console.log("getting userdtls details", userdtls.username,userdtls.email,userdtls.password);
        this.username = userdtls.username;
        this.email = userdtls.email;
        this.password = userdtls.password;
        console.log("updated userdtls details", userdtls.username,userdtls.email,userdtls.password);
      },
      error: (error)=>{
        console.log("error-> ",error);
      }
    })
  }
}
