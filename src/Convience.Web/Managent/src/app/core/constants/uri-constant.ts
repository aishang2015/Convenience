import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class UriConstant {

    ApiUri: string = '';
    BaseUri: string = '';

    constructor(private httpclient: HttpClient) {
        //this.init();
    }

    init() {
        return new Promise<void>((resolve, reject) => {
            const jsonFile = `assets/config/config.json`;
            this.httpclient.get(jsonFile).toPromise().then(data => {
                this.ApiUri = data["apiUrl"];
                this.BaseUri = data["baseUrl"];
                resolve();
            });
        });
    }

    get LoginUri(): string { return `${this.ApiUri}/login` };

    get CaptchaUri(): string { return `${this.ApiUri}/captcha` };

    get ModifySelfPasswordUri(): string { return `${this.ApiUri}/password` };

    get RoleUri(): string { return `${this.ApiUri}/role` };

    get UserUri(): string { return `${this.ApiUri}/user` };

    get MenuUri(): string { return `${this.ApiUri}/menu` };

    get TenantUri(): string { return `${this.ApiUri}/tenant` };

    get DashboardUri(): string { return `${this.ApiUri}/dashboard` };

    get PositiondUri(): string { return `${this.ApiUri}/position` };

    get DepartmentUri(): string { return `${this.ApiUri}/department` };

    get EmployeeUri(): string { return `${this.ApiUri}/employee` };

    get FileUri(): string { return `${this.ApiUri}/file` };

    get FolderUri(): string { return `${this.ApiUri}/folder` };

    get ColumnUri(): string { return `${this.ApiUri}/column` };

    get ArticleUri(): string { return `${this.ApiUri}/article` };

    get DicDataUri(): string { return `${this.ApiUri}/dicData` };

    get DicTypeUri(): string { return `${this.ApiUri}/dicType` };

    get WorkFlowGroupUri(): string { return `${this.ApiUri}/workflowGroup` };

    get WorkFlowUri(): string { return `${this.ApiUri}/workflow` };
}