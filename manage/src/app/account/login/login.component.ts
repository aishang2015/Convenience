import { Component, OnInit } from '@angular/core';
import { StorageService } from 'src/app/core/services/storage.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private storage: StorageService, private router: Router) { }

  ngOnInit() {
  }

  login() {
    this.storage.userToken = "token";
    this.router.navigate(['']);
  }

}
