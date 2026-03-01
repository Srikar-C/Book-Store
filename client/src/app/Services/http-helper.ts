import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class HttpHelper {
  private authUrl = 'http://localhost:5175/api'; 

  private bookUrl = 'http://localhost:5248/api'; 

  private orderUrl = 'http://localhost:5180/api'; 

  constructor(private http: HttpClient, private router:Router) {}

  get<T>(endpoint: string, port: string): Observable<T> {
    return this.http.get<T>(`${port}/${endpoint}`);
  }

  post<T>(endpoint: string, body: any, port: string, options?: any): Observable<T> {
    return this.http.post<T>(`${port}/${endpoint}`, body);
  }

  login(data: any): Observable<any> {
    return this.post<any>('auth/login', data, this.authUrl);
  }

  register(data: any): Observable<any>
  {
    return this.post<any>('auth/register',data, this.authUrl);
  }

  reset(data: any): Observable<any>
  {
    return this.post<any>('auth/reset', data, this.authUrl);
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
    return this.post<any>('auth/logout',{}, this.authUrl);
  }

  getProfile(): Observable<any>
  {
    const token = this.getToken();
    return this.post<any>('auth/profile',{}, this.authUrl,{
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  getBooks(data: string | null): Observable<any>
  {
    console.log("heleper data-> :" +data);
    return this.post<any>('book/getbooks', {UserId: data}, this.bookUrl);
  }

  addToCarts(data: any): Observable<any>
  { 
    const token = this.getToken();
    console.log("helper data for carts-> ",data);
    return this.post<any>('book/addCarts', data, this.bookUrl,{
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    });
  }

  getCarts(data: string | null): Observable<any>
  {
    console.log("heleper data-> :" +data);
    return this.post<any>('book/getCarts', {UserId: data}, this.bookUrl);
  }

  removeOne(data: string | null): Observable<any>
  {
    const token = this.getToken();
    console.log("helper data in remove-> : "+data);
    return this.post<any>('book/removeCart', data, this.bookUrl,{
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    });
  }

  setOrders(data: any): Observable<any>
  {
    const token = this.getToken();
    console.log("helper data in remove-> : "+data);
    return this.post<any>('order/orders', data, this.orderUrl,{
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    });
  }

}
