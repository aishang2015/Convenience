import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { StorageService } from '../services/storage.service';

@Injectable({
  providedIn: 'root'
})
export class LoginGuard implements CanActivate {

  constructor(
    private router: Router,
    private storageService: StorageService) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    if (next.routeConfig.path == 'login' && this.storageService.hasUserToken()) {
      this.router.navigate(['']);
      return false;
    }

    if (next.routeConfig.path != 'login' && !this.storageService.hasUserToken() && this.storageService.IsTokenExpire) {
      this.router.navigate(['/account/login']);
      return false;
    }

    if (next.routeConfig.path != 'login' && this.storageService.IsTokenExpire) {
      this.storageService.removeUserToken();
      this.router.navigate(['/account/login']);
      return false;
    }
    return true;
  }

}
