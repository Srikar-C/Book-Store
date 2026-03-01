import { Routes } from '@angular/router';
import { Login } from './Modules/Login/login/login';
import { ForgotPassword } from './Modules/Login/forgot-password/forgot-password';
import { Signup } from './Modules/Register/signup/signup';
import { HomePage } from './Components/home-page/home-page';
import { Profile } from './Components/profile/profile';
import { Orders } from './Components/orders/orders';
import { Cart } from './Components/cart/cart';
import { Home } from './Components/home/home';
import { authGuard } from './Components/auth-guard';

export const routes: Routes = [

    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: '', component: Login },
    { path: 'forgot-password', component: ForgotPassword },
    { path: 'signup', component: Signup },
    { path: 'login', component: Login },


    { path: ':username', component: HomePage, canActivate: [authGuard], 
         children:[
            { path: 'home', component: Home },
            { path: 'profile', component: Profile, runGuardsAndResolvers: 'always' },
            { path: 'orders', component: Orders },
            { path: 'cart', component: Cart },
            { path: '', redirectTo: 'home', pathMatch: 'full' },
         ] 
    },
    

];
