import { NgModule } from '@angular/core';
import { CatalogComponent } from './catalog.component';
import { SharedModule } from '../shared/shared.module';
import { CatalogService } from './services/catalog.serivce';
import { CatalogItemComponent } from './catalog-item/catalog-item.component';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { UpdateProductPriceComponent } from './product-edit/update-product-price/update-product-price.component';



@NgModule({
  declarations: [
    CatalogComponent,
    CatalogItemComponent,
    ProductEditComponent,
    UpdateProductPriceComponent
  ],
  imports: [
    SharedModule
  ],
  providers: [
    CatalogService
  ]
})
export class CatalogModule { }
