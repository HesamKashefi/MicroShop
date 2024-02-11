import { Component, Input } from '@angular/core';
import { ProductDto } from '../../models/product-dto';
import { CatalogService } from '../../services/catalog.serivce';

@Component({
  selector: 'app-update-product-info',
  templateUrl: './update-product-info.component.html',
  styleUrl: './update-product-info.component.scss',
})
export class UpdateProductInfoComponent {
  @Input()
  product!: ProductDto;

  newName = '';

  constructor(
    private catalogService: CatalogService) { }

  ngOnInit(): void {
    this.newName = this.product.name;
  }

  onSubmit() {
    this.catalogService.updateInfo(this.product.id, this.newName)
      .subscribe(() => {
        this.product.name = this.newName;
      })
  }
}
