import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler } from '@angular/common/http';


@Injectable()
export class AuthHeaderInterceptor implements HttpInterceptor {

    constructor() { }

    intercept(req: HttpRequest<any>, next: HttpHandler) {
        const token = 'Bearer ' + localStorage.getItem('token');
        const authReq = req.clone({ setHeaders: { 'Authorization': token } });
        return next.handle(authReq);
    }

}