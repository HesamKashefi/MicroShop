import { Component, Input } from '@angular/core';
import { ProductDto } from '../models/product-dto';

@Component({
  selector: 'app-catalog-item',
  templateUrl: './catalog-item.component.html',
  styleUrls: ['./catalog-item.component.css']
})
export class CatalogItemComponent {
  @Input()
  product!: ProductDto;
}
