export class Tenant {
    id?: string;
    name?: string;
    urlPrefix?: string;
    dataBaseType?: number = 0;
    connectionString?: string;
    isActive?: boolean = false;
    createdTime?: Date;
}