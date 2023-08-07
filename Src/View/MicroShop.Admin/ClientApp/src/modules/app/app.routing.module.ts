import { NgModule, inject } from "@angular/core";
import { Router, RouterModule } from "@angular/router";
import { AppLoggedInComponent } from "./app-logged-in/app-logged-in.component";
import { LoginComponent } from "../auth/login/login.component";
import { OAuthService } from "angular-oauth2-oidc";
import { CatalogComponent } from "../catalog/catalog.component";

export const avoidUnAuthorizedAccess = () => {
    const oauthService = inject(OAuthService);
    const unAuthorized = !oauthService.hasValidAccessToken();
    if (unAuthorized) {
        const router = inject(Router);
        router.navigateByUrl("/login");
    }
    return !unAuthorized;
}
export const avoidAuthorizedAccess = () => {
    const oauthService = inject(OAuthService);
    const authorized = oauthService.hasValidAccessToken();
    if (authorized) {
        const router = inject(Router);
        router.navigateByUrl("/");
    }
    return !authorized;
}

@NgModule({
    imports: [RouterModule.forRoot([
        {
            path: '',
            component: AppLoggedInComponent,
            canActivate: [avoidUnAuthorizedAccess],
            canActivateChild: [avoidUnAuthorizedAccess],
            children: [
                { path: 'catalog', component: CatalogComponent },
                { path: '**', redirectTo: 'catalog' },
            ]
        },
        { path: 'login', component: LoginComponent, canActivate: [avoidAuthorizedAccess] },
        { path: 'signin-oidc', component: LoginComponent },
        { path: '**', redirectTo: '' }

    ])],
    exports: [RouterModule]
})
export class AppRoutingModule { }