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
                    if (error.status == 0) {
                        this.messageService.error('无法连接服务器');
                    } else {
                        this.messageService.error(error['error'][''][0]);
                    }
                }
            )
        );
    }

}