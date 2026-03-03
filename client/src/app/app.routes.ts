import { Routes } from '@angular/router';
import { Login } from './Modules/Login/login/login';
import { ForgotPassword } from './Modules/Login/forgot-password/forgot-password';
import { Signup } from './Modules/Register/signup/signup';
import { authGuard } from './Components/auth-guard';
import { HomePage } from './Components/home-page/home-page';
import { Profile } from './Modules/profile/profile';
import { Orders } from './Modules/orders/orders';
import { Carts } from './Modules/carts/carts';
import { Books } from './Modules/books/books';

export const routes: Routes = [

    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: '', component: Login },
    { path: 'forgot-password', component: ForgotPassword },
    { path: 'signup', component: Signup },
    { path: 'login', component: Login },
    {
        path: 'home',
        component: HomePage,
        children: [
            { path: 'books', component: Books },
            { path: 'profile', component: Profile },
            { path: 'orders', component: Orders },
            { path: 'cart', component: Carts },
        ]
    },
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    

];
