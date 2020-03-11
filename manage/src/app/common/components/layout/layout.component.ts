import { Component, OnInit } from '@angular/core';
import { StorageService } from 'src/app/core/services/storage.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {

  constructor(private storageService: StorageService, private router: Router) { }

  ngOnInit() {
  }

  logout() {
    this.storageService.removeUserToken();
    this.router.navigate(['/account/login']);
  }
}
