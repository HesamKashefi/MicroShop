import { NgModule } from "@angular/core";
import { LoginComponent } from "./login/login.component";
import { SharedModule } from "../shared/shared.module";
import { OAuthModule } from "angular-oauth2-oidc";

@NgModule({
    declarations: [
        LoginComponent
    ],
    imports: [
        SharedModule,
        OAuthModule
    ]
})
export class AuthModule {

}