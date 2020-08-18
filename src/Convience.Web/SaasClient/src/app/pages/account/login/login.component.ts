import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { LoginModel } from '../../../model/login';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup = new FormGroup({});

  registForm: FormGroup = new FormGroup({});

  uri = 'https://localhost:5001/api/tenant';

  constructor(private _formBuilder: FormBuilder,
    private _httpClient: HttpClient,
    private _router: Router,
    private _toast: ToastrService) { }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.loginForm = this._formBuilder.group({
      account: [null, [Validators.required]],
      password: [null, [Validators.required]],
    });
    this.registForm = this._formBuilder.group({
      account: [null, [Validators.required]],
      password: [null, [Validators.required]],
    });
  }

  login() {
    for (const i in this.loginForm.controls) {
      this.loginForm.controls[i].markAsDirty();
      this.loginForm.controls[i].updateValueAndValidity();
    }
    if (this.loginForm.valid) {
      let model = new LoginModel();
      model.account = this.loginForm.value['account'];
      model.password = this.loginForm.value['password'];
      this._httpClient.post(this.uri + '/login', model).subscribe(
        result => {
          this._router.navigate(['home']);
          localStorage.setItem('token', result['token']);
          this._toast.success('登录成功！');
        }
      );

    }
  }

  regist() {
    for (const i in this.registForm.controls) {
      this.registForm.controls[i].markAsDirty();
      this.registForm.controls[i].updateValueAndValidity();
    }
    if (this.registForm.valid) {
      let model = new LoginModel();
      model.account = this.registForm.value['account'];
      model.password = this.registForm.value['password'];
      this._httpClient.post(this.uri + '/regist', model).subscribe(
        result => {
          this.registForm.reset();
          Object.keys(this.registForm.controls).forEach(key => {
            this.registForm.get(key).setErrors(null);
          });
          this._toast.success('注册成功！请登录');
        }
      );
    }
  }

}
