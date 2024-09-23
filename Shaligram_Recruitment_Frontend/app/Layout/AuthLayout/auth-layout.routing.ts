import { Routes } from '@angular/router';

import { AuthLayoutComponent } from './auth-layout/auth-layout.component';
// import { LoginComponent } from '../../Pages/AuthPages/Login/login/login.component';
import { ForgotPassComponent } from '../../Pages/AuthPages/ForgotPasswordPages/forgot-pass/forgot-pass/forgot-pass.component';
import { OtpCheckComponent } from '../../Pages/AuthPages/ForgotPasswordPages/otp-check/otp-check/otp-check.component';
import { nonAuthGuard } from '../../Auth/non-auth.guard';
import { ResetPassComponent } from '../../Pages/AuthPages/ForgotPasswordPages/reset-pass/reset-pass/reset-pass.component';
import { RegisterComponent } from '../../Pages/AuthPages/Register/register/register.component';
import { LoginComponent } from '../../Pages/AuthPages/Login/login/login.component';



export const AuthRoutes: Routes = [

    {
        path: '',
        redirectTo: "login",
        pathMatch: "full"
    },
    {
        path:"",
        component:AuthLayoutComponent,
        children: [
          {path:'login', component:LoginComponent}, 
          {path:"forgotpassword" , component:ForgotPassComponent},
          {path:"otpverify" , component:OtpCheckComponent,canActivate:[nonAuthGuard]},
          {path:"resetpassword" , component:ResetPassComponent,canActivate:[nonAuthGuard]},
          {path:"register" , component:RegisterComponent},
          {path:"register-otp" , component:RegisterComponent},
     
        ]
      }
];  