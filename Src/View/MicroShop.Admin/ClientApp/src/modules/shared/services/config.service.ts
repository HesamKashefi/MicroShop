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

@Injectable({
    providedIn: 'root'
})
export class ConfigService {
    private _spaConfig?: Config;
    Config?: ServerUrlsConfig;
    Config$ = new BehaviorSubject<ServerUrlsConfig | undefined>(undefined);

    constructor(private http: HttpClient) { }

    load(configName: string) {
        return this.http.get<Config>(configName).pipe(
            tap(config => {
                this._spaConfig = config;
            }),
            switchMap(config => {
                return this.http.get<ServerUrlsConfig>(config.ApiAddress + '/api/config');
            }),
            tap(config => {
                this.Config = config;
                this.Config$.next(config);
            }),
        );
    }
}