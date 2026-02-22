import { Routes } from '@angular/router';
import { Login } from './Modules/Login/login/login';
import { ForgotPassword } from './Modules/Login/forgot-password/forgot-password';
import { Signup } from './Modules/Register/signup/signup';
import { HomePage } from './Components/home-page/home-page';

export const routes: Routes = [
    { path: '', component: Login },
    { path: 'forgot-password', component: ForgotPassword },
    { path: 'signup', component: Signup },
    { path: 'login', component: Login },
    { path: 'home-page', component: HomePage },

];
