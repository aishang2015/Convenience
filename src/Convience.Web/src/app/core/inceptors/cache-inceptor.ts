import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpResponse } from '@angular/common/http';
import { CacheService } from '../services/cache.service';
import { of } from 'rxjs';
import { tap } from 'rxjs/operators';


@Injectable()
export class CacheInterceptor implements HttpInterceptor {

    constructor(private cacheService: CacheService) { }

    // 在请求中添加认证header
    intercept(req: HttpRequest<any>, next: HttpHandler) {
        var isUseCahe = req.params.get('useCache');
        if (isUseCahe == 'use') {
            var cachedResponse = this.cacheService.getObject(req.url);
            if (cachedResponse != null) {
                return of(cachedResponse);
            } else {
                return next.handle(req).pipe(
                    tap(event => {
                        if (event instanceof HttpResponse) {
                            this.cacheService.setObject(req.url, event);
                        }
                    })
                );
            }
        }
        return next.handle(req);
    }

}