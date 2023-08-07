import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app.routing.module';
import { SharedModule } from '../shared/shared.module';
import { AppLoggedInComponent } from './app-logged-in/app-logged-in.component';
import { OAuthModule } from 'angular-oauth2-oidc';
import { CatalogModule } from '../catalog/catalog.module';

@NgModule({
  declarations: [
    AppComponent,
    AppLoggedInComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    AppRoutingModule,
    SharedModule,
    OAuthModule.forRoot(),
    CatalogModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
