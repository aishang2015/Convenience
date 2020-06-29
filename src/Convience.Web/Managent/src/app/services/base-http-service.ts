import { HttpParams } from "@angular/common/http";

class BaseHttpService {

    protected useCacheOption = { params: new HttpParams().set('useCache', 'use') };
}