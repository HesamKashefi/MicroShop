import { Component, OnInit } from '@angular/core';
import { CatalogService } from './services/catalog.serivce';
import { ProductDto } from './models/product-dto';
import { PagedResult } from '../shared/models/paged-result';

@Component({
  selector: 'app-catalog',
  templateUrl: './catalog.component.html',
  styleUrls: ['./catalog.component.scss']
})
export class CatalogComponent implements OnInit {
  products?: PagedResult<ProductDto[]>;
  currentPage = 1;

  constructor(private catalogService: CatalogService) {
  }

  ngOnInit(): void {
    this.fetch(1);
  }

  fetch(page: number) {
    this.catalogService.getProducts(page)
      .subscribe(products => {
        this.currentPage = page;
        this.products = products;
      });
  }

}
