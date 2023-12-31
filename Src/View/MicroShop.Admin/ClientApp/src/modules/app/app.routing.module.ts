import { NgModule, inject } from "@angular/core";
import { Router, RouterModule } from "@angular/router";
import { AppLoggedInComponent } from "./app-logged-in/app-logged-in.component";
import { LoginComponent } from "../auth/login/login.component";
import { OAuthService } from "angular-oauth2-oidc";

export const avoidUnAuthorizedAccess = () => {
    const oauthService = inject(OAuthService);
    const authorized = oauthService.hasValidAccessToken();
    if (!authorized) {
        const router = inject(Router);
        router.navigateByUrl("/login");
    }
    return authorized;
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
    imports: [
        RouterModule.forRoot(
            [
                { path: 'login', component: LoginComponent, canActivate: [avoidAuthorizedAccess] },
                { path: 'signin-oidc', component: LoginComponent, canActivate: [avoidAuthorizedAccess] },
                {
                    path: '',
                    component: AppLoggedInComponent,
                    canActivate: [avoidUnAuthorizedAccess],
                    canActivateChild: [avoidUnAuthorizedAccess],
                    children: [
                        { path: 'catalog', loadChildren: () => import('./../catalog/catalog.module').then(x => x.CatalogModule) },
                        { path: 'orders', loadChildren: () => import('./../orders/orders.module').then(x => x.OrdersModule) },
                        { path: '**', redirectTo: 'catalog' },
                    ]
                },
                { path: '**', redirectTo: '' }

            ])
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
