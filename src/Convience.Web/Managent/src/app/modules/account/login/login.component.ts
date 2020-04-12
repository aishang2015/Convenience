import { Component, OnInit } from '@angular/core';
import { StorageService } from 'src/app/core/services/storage.service';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AccountService } from 'src/app/services/account.service';
import { error } from 'protractor';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  captcha: SafeUrl = '';

  validateForm: FormGroup;

  isLoading: boolean = false;

  private captchaKey = '';

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private storage: StorageService,
    private router: Router,
    private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.validateForm = this.fb.group({
      userName: [null, [Validators.required]],
      password: [null, [Validators.required]],
      captchaValue: [null, [Validators.required]],
      remember: [true]
    });
    this.refreshCaptcha();
  }

  refreshCaptcha() {
    this.accountService.getCaptcha().subscribe(result => {

      this.captchaKey = result['captchaKey'];
      this.captcha = this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,'
        + result['captchaData']);
    });
  }

  submitForm(): void {
    for (const i in this.validateForm.controls) {
      this.validateForm.controls[i].markAsDirty();
      this.validateForm.controls[i].updateValueAndValidity();
    }
    if (this.validateForm.valid) {
      this.isLoading = true;
      this.accountService.login(
        this.validateForm.controls['userName'].value,
        this.validateForm.controls['password'].value,
        this.captchaKey,
        this.validateForm.controls['captchaValue'].value)
        .subscribe(
          result => {
            this.storage.userToken = result["token"];
            this.storage.Name = result["name"];
            this.storage.Avatar = result["avatar"];
            this.storage.Identifycation = result["identification"];
            this.storage.Route = result["routes"];
            this.router.navigate(['/dashboard']);
          },
          error => {
            this.refreshCaptcha();
            this.validateForm.controls['password'].reset();
            this.validateForm.controls['captchaValue'].reset();
            this.isLoading = false;
          }
        );
    }
  }
}
