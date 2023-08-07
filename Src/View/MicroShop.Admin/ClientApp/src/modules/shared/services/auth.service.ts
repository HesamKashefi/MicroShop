import { Injectable } from "@angular/core";
import { ConfigService } from "./config.service";
import { AuthConfig, OAuthService } from "angular-oauth2-oidc";
import { Subject } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    user$ = new Subject<any>();

    constructor(private configService: ConfigService, private oauthService: OAuthService) {
        const authCodeFlowConfig: AuthConfig = {
            // Url of the Identity Provider
            issuer: this.configService.Config.identity,
            tokenEndpoint: this.configService.Config.identity + 'connect/token',

            // URL of the SPA to redirect the user to after login
            redirectUri: this.configService.Config.adminLocalSpa + 'signin-oidc',
            clientId: 'MicroShop',
            dummyClientSecret: '38567b43-tebe-18ce-8ba8-ab57356d4dga',

            responseType: 'code',
            scope: 'openid profile email',

            showDebugInformation: true,
            requireHttps: false
        };

        this.oauthService.configure(authCodeFlowConfig);

        this.oauthService.events.subscribe(e => {
            console.log(e);
            if (e.type === "token_received") {
                const claims = this.oauthService.getIdentityClaims();
                this.user$.next(claims);
            }
        });
    }

}