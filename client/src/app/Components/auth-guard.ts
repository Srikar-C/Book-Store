import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { HttpHelper } from '../Services/http-helper';

export const authGuard: CanActivateFn = (route, state) => {

  const auth = inject(HttpHelper);
  const router = inject(Router);


  if(auth.isLogged())
  {
    return true;
  } 
  else 
  {
    router.navigate(['/login']);
    return false;
  }
};
