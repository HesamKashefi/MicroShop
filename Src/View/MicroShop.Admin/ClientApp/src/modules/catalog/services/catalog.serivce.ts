import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UrlsConfig } from "src/modules/shared/urls-config";
import { ProductDto } from "../models/product-dto";
import { HttpService } from "src/modules/shared/services/http.service";
import { PagedResult } from "src/modules/shared/models/paged-result";

@Injectable()
export class CatalogService {
    constructor(private http: HttpService) { }

    getProducts(page: number): Observable<PagedResult<ProductDto[]>> {
        return this.http.get<PagedResult<ProductDto[]>>((baseUrl) => UrlsConfig.getCatalog(baseUrl))
    }
}

