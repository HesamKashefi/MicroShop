import { Component, OnInit } from '@angular/core';
import { CatalogService } from './services/catalog.serivce';
import { ProductDto } from './models/product-dto';

@Component({
  selector: 'app-catalog',
  templateUrl: './catalog.component.html',
  styleUrls: ['./catalog.component.css']
})
export class CatalogComponent implements OnInit {
  products: ProductDto[] = [];

  constructor(private catalogService: CatalogService) { }

  ngOnInit(): void {
    this.catalogService.getProducts().subscribe(products => {
      this.products = products;
    });
  }

}
