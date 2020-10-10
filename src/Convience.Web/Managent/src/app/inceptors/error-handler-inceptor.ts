import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpResponse } from '@angular/common/http';
import { NzMessageService } from 'ng-zorro-antd/message';
import { tap } from 'rxjs/operators';
import { StorageService } from '../services/storage.service';
import { Router } from '@angular/router';
import { ShowErrorService } from '../services/show-error.service';


@Injectable()
export class ErrorHandlerInterceptor implements HttpInterceptor {

    constructor(private messageService: NzMessageService,
        private storageService: StorageService,
        private showErrorService: ShowErrorService,
        private router: Router) { }

    // 在请求中添加认证header
    intercept(req: HttpRequest<any>, next: HttpHandler) {
        return next.handle(req).pipe(
            tap(
                event => {
                    if (event instanceof HttpResponse) {
                    }
                }, error => {
                    let msg = '';
                    if (error.status == 0) {
                        msg = '无法连接服务器,请联系管理员！';
                        this.storageService.removeUserToken();
                        this.router.navigate(['/account/login']);
                    } else if (error.status == 401) {
                        msg = '还没有进行登录或登录信息已过期！';
                        this.storageService.removeUserToken();
                        this.router.navigate(['/account/login']);
                    } else if (error.status == 403) {
                        msg = '没有操作的权限！';
                    } else if (error.status == 404) {
                        msg = '服务器地址错误！';
                    } else if (error.status == 500) {
                        msg = '发生系统错误！';
                    } else {
                        for (var key in error['error']) {
                            msg += error.error[key];
                        }
                    }
                    this.showErrorService.publishError(msg);
                }
            )
        );
    }

}