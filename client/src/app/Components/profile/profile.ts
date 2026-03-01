import { Component } from '@angular/core';
import { HttpHelper } from '../../Services/http-helper';

@Component({
  selector: 'app-profile',
  imports: [],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile {

  constructor(private httpHelper : HttpHelper){}

  username : string = "";
  email : string = "";
  password : string = "";

}
