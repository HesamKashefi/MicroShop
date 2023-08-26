import { NgModule } from "@angular/core";
import { OrdersComponent } from "./orders.component";
import { OrderDetailsComponent } from "./order-details/order-details.component";
import { OrderItemComponent } from "./order-details/order-item/order-item.component";
import { SharedModule } from "../shared/shared.module";
import { RouterModule, Routes } from "@angular/router";
import { OrdersService } from "./services/orders.service";

const routes: Routes = [
    { path: '', component: OrdersComponent },
    { path: ':id', component: OrderDetailsComponent }
];

@NgModule({
    declarations: [
        OrdersComponent,
        OrderDetailsComponent,
        OrderItemComponent
    ],
    imports: [
        SharedModule,
        RouterModule.forChild(routes)
    ],
    providers: [
        OrdersService
    ]
})
export class OrdersModule {

}