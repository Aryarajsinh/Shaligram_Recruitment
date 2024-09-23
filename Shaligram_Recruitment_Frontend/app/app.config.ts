import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { AuthRoutes } from './Layout/AuthLayout/auth-layout.routing'; 
import {  provideHttpClient, withFetch } from '@angular/common/http';
import { provideToastr } from 'ngx-toastr';
import { AdminRoutes } from './Layout/AdminLayout/admin/admin.routing';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';



export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(AuthRoutes),
    provideRouter(AdminRoutes), 
    provideHttpClient(withFetch()),
    provideToastr(), provideAnimationsAsync(),
  ]
};
