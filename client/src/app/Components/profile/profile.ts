import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpHelper } from '../../Services/http-helper';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile {

  constructor(private httpHelper: HttpHelper, private router: Router){}

  type: number = 0;

  name: string = "";
  email: string = "";
  phone: string = "";
  password: string = "";

  toastr = inject(ToastrService);

  ngOnInit()
  {
    this.type = history.state.type;
    this.getProfile();
  }

  getProfile()
  {
    var apiUrl = 'http://localhost:5227/api'; 
    const token = localStorage.getItem('token')?.trim();
    const userId = localStorage.getItem('userId');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
    console.log("token", token,headers);
    this.httpHelper.get(apiUrl,'auth/getProfile',{ headers: headers })
    .subscribe({
      next: (response)=>{
        console.log("response-> ",response);
      },
      error: (error)=>{
        console.log("error-> ",error); 
        this.toastr.error(error.error?.message,'Error');
      }
    })
  }

  onSubmit()
  {

  }

  onEdit()
  {

  }
}
