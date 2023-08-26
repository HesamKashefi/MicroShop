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
        console.log(UrlsConfig.catalog_getCatalog('http://localhost/', page));
        return this.http.get<PagedResult<ProductDto[]>>((baseUrl) => UrlsConfig.catalog_getCatalog(baseUrl, page));
    }

    getProductById(id: string): Observable<ProductDto | null> {
        return this.http.get<ProductDto | null>((baseUrl) => UrlsConfig.catalog_getProductById(baseUrl, id));
    }

    updatePrice(productId: string, newPrice: number) {
        return this.http.put<void>((baseUrl) => UrlsConfig.catalog_updateProductPrice(baseUrl), { productId, newPrice });
    }
}

