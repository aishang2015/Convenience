import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AccountService } from 'src/app/business/account.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { StorageService } from 'src/app/services/storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less']
})
export class LoginComponent implements OnInit {

  captcha: SafeUrl = '';

  validateForm: FormGroup;

  isLoading: boolean = false;

  private captchaKey = '';

  constructor(
    private _formBuilder: FormBuilder,
    private _accountService: AccountService,
    private _storageService: StorageService,
    private _router: Router,
    private _sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.validateForm = this._formBuilder.group({
      userName: [null, [Validators.required]],
      password: [null, [Validators.required]],
      captchaValue: [null, [Validators.required]],
      remember: [true]
    });
    this.refreshCaptcha();
  }

  refreshCaptcha() {
    this._accountService.getCaptcha().subscribe(result => {

      this.captchaKey = result.captchaKey;
      this.captcha = this._sanitizer.bypassSecurityTrustResourceUrl(`data:image/jpg;base64,${result.captchaData}`);
    });
  }

  submitForm(): void {
    for (const i in this.validateForm.controls) {
      this.validateForm.controls[i].markAsDirty();
      this.validateForm.controls[i].updateValueAndValidity();
    }
    if (this.validateForm.valid) {
      this.isLoading = true;
      this._accountService.login(
        this.validateForm.controls['userName'].value,
        this.validateForm.controls['password'].value,
        this.captchaKey,
        this.validateForm.controls['captchaValue'].value)
        .subscribe(
          result => {
            this._storageService.userToken = result.token;
            this._storageService.Name = result.name;
            this._storageService.Avatar = result.avatar;
            this._storageService.Identifycation = result.identification;
            this._storageService.Route = result.routes;
            this._router.navigate(['/dashboard']);
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
