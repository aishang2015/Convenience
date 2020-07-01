import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { StorageService } from './services/storage.service';

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
