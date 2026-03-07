import { Routes } from '@angular/router';
import { Login } from './Modules/Login/login/login';
import { Forgot } from './Modules/Login/forgot/forgot';
import { Register } from './Modules/Register/register/register';
import { Books } from './Modules/Orders/books/books';
import { HomePage } from './Components/home-page/home-page';
import { Profile } from './Components/profile/profile';
import { Orders } from './Modules/Orders/orders/orders';
import { Carts } from './Modules/Orders/carts/carts';
import { Verify } from './Modules/Register/verify/verify';
import { BookentryComponent } from './Components/bookentry/bookentry.component';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: '', component: Login },
    { path: 'forgot-password', component: Forgot },
    { path: 'signup', component: Register },
    { path: 'login', component: Login },
    { path: 'verify', component: Verify },
    {
        path: 'home',
        component: HomePage, 
        children: [
            { path: 'books', component: Books },
            { path: 'profile', component: Profile },
            { path: 'orders', component: Orders },
            { path: 'carts', component: Carts },
            { path: 'addBook', component: BookentryComponent }
        ]
    },
];
