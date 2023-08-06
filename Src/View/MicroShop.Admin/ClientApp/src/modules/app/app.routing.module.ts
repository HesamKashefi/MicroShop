import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { AppLoggedInComponent } from "./app-logged-in/app-logged-in.component";
import { LoginComponent } from "../auth/login/login.component";

@NgModule({
    imports: [RouterModule.forRoot([
        { path: '', pathMatch: 'full', component: AppLoggedInComponent },
        { path: 'login', component: LoginComponent }
    ])],
    exports: [RouterModule]
})
export class AppRoutingModule { }