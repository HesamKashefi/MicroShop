import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { DataResult, PagedResult } from "src/modules/shared/models/paged-result";
import { HttpService } from "src/modules/shared/services/http.service";
import { OrderDto } from "../models/order-dto";
import { OrderDetailsDto } from "../models/order-details-dto";
import { UrlsConfig } from "src/modules/shared/urls-config";

@Injectable()
export class OrdersService {
    constructor(private http: HttpService) { }

    getOrders(page: number): Observable<PagedResult<OrderDto[]>> {
        return this.http.get<PagedResult<OrderDto[]>>((baseUrl) => UrlsConfig.orders_getAll(baseUrl, page));
    }

    getOrderById(orderId: number): Observable<DataResult<OrderDetailsDto>> {
        return this.http.get<DataResult<OrderDetailsDto>>((baseUrl) => UrlsConfig.orders_getById(baseUrl, orderId));
    }
}
