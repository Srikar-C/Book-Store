import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-verify',
  imports: [RouterLink, CommonModule],
  templateUrl: './verify.html',
  styleUrl: './verify.css',
})
export class Verify {

  cfnpassword : string = "";
  password : string = "";

  showPassword : boolean = false;
  showCfnPassword : boolean = false;

  constructor() {}

  toggleView1()
  {
    this.showPassword = !this.showPassword;
  }
  
  toggleView2()
  {
    this.showCfnPassword = !this.showCfnPassword;
  }
}
