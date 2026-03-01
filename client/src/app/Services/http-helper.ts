import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class HttpHelper {
  private baseUrl = 'http://localhost:5175/api'; 

  constructor(private http: HttpClient, private router:Router) {}

  get<T>(endpoint: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${endpoint}`);
  }

  post<T>(endpoint: string, body: any): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/${endpoint}`, body);
  }

  login(data: any): Observable<any> {
    return this.post<any>('auth/login', data);
  }

  register(data: any): Observable<any>
  {
    return this.post<any>('auth/register',data);
  }

  reset(data: any): Observable<any>
  {
    return this.post<any>('auth/reset', data);
  }

  saveToken(data: string)
  {
    localStorage.setItem('token',data);
  }

  getToken(): string | null 
  {
    return localStorage.getItem('token');
  }

  isLogged(): boolean
  {
    return !!this.getToken();
  }

  logout(): Observable<any>
  {
    return this.post<any>('auth/logout',{});
  }

  getProfile(): Observable<any>
  {
    return this.post<any>('/auth/profile',{});
  }

}
