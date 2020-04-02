import { Component, OnChanges } from '@angular/core';
import { StorageService } from './core/services/storage.service';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  isLogin = this.storageService.hasUserToken();

  constructor(private storageService: StorageService, private router: Router) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.isLogin = this.storageService.hasUserToken();
      }
    });
  }

}
