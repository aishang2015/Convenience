import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler } from '@angular/common/http';
import { StorageService } from '../services/storage.service';


@Injectable()
export class AuthHeaderInterceptor implements HttpInterceptor {

    constructor(private storageService: StorageService) { }

    // 在请求中添加认证header
    intercept(req: HttpRequest<any>, next: HttpHandler) {
        const token = 'Bearer ' + this.storageService.userToken;
        const authReq = req.clone({ setHeaders: { 'Authorization': token } });
        return next.handle(authReq);
    }

}