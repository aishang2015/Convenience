import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpResponse } from '@angular/common/http';
import { NzMessageService } from 'ng-zorro-antd';
import { tap } from 'rxjs/operators';


@Injectable()
export class ErrorHandlerInterceptor implements HttpInterceptor {

    constructor(private messageService: NzMessageService) { }

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
                    } else if (error.status == 401) {
                        msg = '还没有进行授权！';
                    } else if (error.status == 403) {
                        msg = '没有操作的权限！';
                    } else if (error.status == 500) {
                        msg = '发生系统错误！';
                    } else {
                        for (var key in error['error']) {
                            msg += error.error[key];
                        }
                    }
                    this.messageService.error(msg);
                }
            )
        );
    }

}