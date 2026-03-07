import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HttpHelper {

  constructor(private http: HttpClient, private router:Router) {}

  get(url: string, endpoint: string, options?: any): Observable<any> {
    return this.http.get<any>(`${url}/${endpoint}`, options);
  }

  post( url: string, endpoint: string, body: any, options?: any): Observable<any> {
    return this.http.post<any>(`${url}/${endpoint}`, body, options);
  }

  delete( url: string, endpoint: string, body: any, options?: any): Observable<any> {
    const httpOptions = {
      ...options,
      body: body
    };
    return this.http.delete<any>(`${url}/${endpoint}`, httpOptions);
  }
  
}
