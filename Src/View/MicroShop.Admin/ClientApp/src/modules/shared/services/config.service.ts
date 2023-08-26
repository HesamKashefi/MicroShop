import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, switchMap, tap } from "rxjs";

export interface Config {
    ApiAddress: string;
}
export interface ServerUrlsConfig {
    apigateway: string;
    identity: string;
    adminLocalSpa: string;
}

@Injectable()
export class ConfigService {
    private _spaConfig?: Config;
    Config?: ServerUrlsConfig;
    Config$ = new BehaviorSubject<ServerUrlsConfig | undefined>(undefined);

    constructor(private http: HttpClient) { }

    load() {
        return this.http.get<ServerUrlsConfig>('/api/config').pipe(
            tap(config => {
                this.Config = config;
                this.Config$.next(config);
            }),
        );
    }
}