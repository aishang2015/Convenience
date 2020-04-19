import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class UriConstant {

    BaseUri: string = '';

    constructor(private httpclient: HttpClient) {
        //this.init();
    }

    init() {
        return new Promise<void>((resolve, reject) => {
            const jsonFile = `assets/config/config.json`;
            this.httpclient.get(jsonFile).toPromise().then(data => {
                this.BaseUri = data["apiUrl"];
                resolve();
            });
        });
    }

    get LoginUri(): string { return `${this.BaseUri}/login` };

    get CaptchaUri(): string { return `${this.BaseUri}/captcha` };

    get ModifySelfPasswordUri(): string { return `${this.BaseUri}/password` };

    get RoleUri(): string { return `${this.BaseUri}/role` };

    get UserUri(): string { return `${this.BaseUri}/user` };

    get MenuUri(): string { return `${this.BaseUri}/menu` };

    get TenantUri(): string { return `${this.BaseUri}/tenant` };

    get DashboardUri(): string { return `${this.BaseUri}/dashboard` };

    get PositiondUri(): string { return `${this.BaseUri}/position` };

    get DepartmentUri(): string { return `${this.BaseUri}/department` };

}