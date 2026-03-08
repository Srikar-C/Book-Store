import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { tap } from 'rxjs';

export const authInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('token');
  const router = inject(Router);
  const toastr = inject(ToastrService);

  // Add Authorization header if token exists
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    });
  }

  return next(req).pipe(
    tap({
      error: (err: any) => {
        if (err.status === 401) {
          // Clear token and redirect to login
          localStorage.removeItem('token');
          toastr.error('Session expired. Please login again.', 'Unauthorized');
          router.navigate(['/login']);
        }
      }
    })
  );
};