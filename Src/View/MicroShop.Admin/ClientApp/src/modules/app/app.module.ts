import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app.routing.module';
import { SharedModule } from '../shared/shared.module';
import { AppLoggedInComponent } from './app-logged-in/app-logged-in.component';
import { OAuthModule } from 'angular-oauth2-oidc';
import { CatalogModule } from '../catalog/catalog.module';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { ConfigService } from '../shared/services/config.service';


export function configLoader(configService: ConfigService) {
  return () => configService.load();
}

@NgModule({
  declarations: [
    AppComponent,
    AppLoggedInComponent,
    NavBarComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    AppRoutingModule,
    SharedModule,
    OAuthModule.forRoot(),
    CatalogModule
  ],
  providers: [
    ConfigService,
    {
      provide: APP_INITIALIZER,
      useFactory: configLoader,
      deps: [ConfigService],
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
