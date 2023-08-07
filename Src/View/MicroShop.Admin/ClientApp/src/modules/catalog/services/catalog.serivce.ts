import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UrlsConfig } from "src/modules/shared/urls-config";
import { ProductDto } from "../models/product-dto";
import { HttpService } from "src/modules/shared/services/http.service";

@Injectable()
export class CatalogService {
    constructor(private http: HttpService) { }

    getProducts(): Observable<ProductDto[]> {
        return this.http.get<ProductDto[]>((baseUrl) => UrlsConfig.getCatalog(baseUrl))
    }
}

