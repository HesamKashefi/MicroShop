import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, filter, switchMap } from "rxjs";
import { ConfigService } from "./config.service";
import { OAuthService } from "angular-oauth2-oidc";

@Injectable()
export class HttpService {
    constructor(
        private http: HttpClient,
        private oauthService: OAuthService,
        private configService: ConfigService) { }

    get<T>(urlBuilder: (basePath: string) => string): Observable<T> {
        if (!this.configService.Config) {
            return this.configService.Config$.pipe(filter(c => !!c), switchMap(x => this.get<T>(urlBuilder)));
        }

        const headers = new HttpHeaders();
        const token = this.oauthService.hasValidAccessToken() ? this.oauthService.getAccessToken() : undefined;
        if (token) {
            headers.set('Authorization', 'Bearer ' + token)
        }
        return this.http.get<T>(urlBuilder(this.configService.Config.apigateway), { headers })
    }
}