import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { HttpService } from "./services/http.service";
import { RouterModule } from "@angular/router";

const modules: any[] = [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule
];

@NgModule({
    imports: [...modules],
    exports: [...modules],
    providers: [
        HttpService,
    ]
})
export class SharedModule {

}