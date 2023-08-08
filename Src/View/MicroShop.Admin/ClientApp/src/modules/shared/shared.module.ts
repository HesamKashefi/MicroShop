import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { APP_INITIALIZER, NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ConfigService } from "./services/config.service";
import { environment } from "src/environments/environment";
import { HttpService } from "./services/http.service";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";

export function configLoader(configService: ConfigService) {
    return () => configService.load(environment.config);
}

const modules: any[] = [
    CommonModule,
    FormsModule,
    BrowserModule,
    HttpClientModule,
    RouterModule
];

@NgModule({
    imports: [...modules],
    exports: [...modules],
    providers: [
        HttpService,
        ConfigService,
        {
            provide: APP_INITIALIZER,
            useFactory: configLoader,
            deps: [ConfigService],
            multi: true
        }
    ]
})
export class SharedModule {

}