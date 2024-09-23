import { inject, Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { CommonService } from '../Service/Common/common.service';

@Injectable({
  providedIn: 'root',
})

class PermissionService {
  constructor( private route: Router, private service: CommonService) { }
   canActive(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    
    if (this.service.IsLoggedIn()) {
      return true;
      
    } else {
      this.route.navigate(['login'])
      return false;
    }
   }
}
export const authGuard: CanActivateFn = (next: ActivatedRouteSnapshot,  state: RouterStateSnapshot
): boolean => {
   return inject(PermissionService).canActive(next, state);
};