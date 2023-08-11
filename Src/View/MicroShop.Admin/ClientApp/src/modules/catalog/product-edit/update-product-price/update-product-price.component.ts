import { Component, Input, OnInit } from '@angular/core';
import { ProductDto } from '../../models/product-dto';
import { CatalogService } from '../../services/catalog.serivce';

@Component({
  selector: 'app-update-product-price',
  templateUrl: './update-product-price.component.html',
  styleUrls: ['./update-product-price.component.scss']
})
export class UpdateProductPriceComponent implements OnInit {
  @Input()
  product!: ProductDto;

  newPrice = 0;

  constructor(
    private catalogService: CatalogService) { }

  ngOnInit(): void {
    this.newPrice = this.product.price;
  }

  onSubmit() {
    this.catalogService.updatePrice(this.product.id, this.newPrice)
      .subscribe(() => {
        this.product.price = this.newPrice;
      })
  }
}
