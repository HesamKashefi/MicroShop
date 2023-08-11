import { Component, OnInit } from '@angular/core';
import { CatalogService } from '../services/catalog.serivce';
import { ActivatedRoute } from '@angular/router';
import { ProductDto } from '../models/product-dto';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.scss']
})
export class ProductEditComponent implements OnInit {
  product?: ProductDto | null;

  constructor(
    private route: ActivatedRoute,
    private catalogService: CatalogService) { }

  ngOnInit(): void {
    this.route.params.subscribe(p => {

      this.catalogService.getProductById(p["id"])
        .subscribe(product => {
          this.product = product;
        });
    })
  }

}
