export class UriConstant {

    static readonly BaseUri: string = 'https://localhost:44356/api';

    static readonly LoginUri: string = `${UriConstant.BaseUri}/login`;

    static readonly ModifySelfPasswordUri: string = `${UriConstant.BaseUri}/password`;

}