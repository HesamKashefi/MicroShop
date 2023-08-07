import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { switchMap, tap } from "rxjs";

export interface Config {
    ApiAddress: string;
}

@Injectable({
    providedIn: 'root'
})
export class ConfigService {
    private _config?: Config;
    Config: {
        apigateway: string;
        identity: string;
    } | any;

    constructor(private http: HttpClient) { }

    load(configName: string) {
        return this.http.get<Config>(configName).pipe(
            tap(config => {
                this._config = config;
            }),
            switchMap(config => {
                return this.http.get(config.ApiAddress + '/api/config');
            }),
            tap(config => {
                this.Config = config;
            }),
        );
    }
}