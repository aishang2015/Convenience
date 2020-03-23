import { Component, OnInit } from '@angular/core';
import { StorageService } from 'src/app/core/services/storage.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {

  breadcrumbInfo: string[] = ['仪表盘'];

  name;
  avatar;

  constructor(private storageService: StorageService, private router: Router) { }

  ngOnInit() {
    this.name = this.storageService.Name;
    this.avatar = this.storageService.Avatar;
  }

  logout() {
    this.storageService.removeUserToken();
    this.router.navigate(['/account/login']);
  }

  setBreadcrumb(first: string, ...rest: string[]) {
    this.breadcrumbInfo = [];
    this.breadcrumbInfo.push(first);
    rest.forEach(element => {
      this.breadcrumbInfo.push(element);
    });
  }

  getImgUrl(name) {
    return `/assets/avatars/${name}.png`;
  }
}
