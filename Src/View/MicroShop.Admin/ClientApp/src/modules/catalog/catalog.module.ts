import { NgModule } from '@angular/core';
import { CatalogComponent } from './catalog.component';
import { SharedModule } from '../shared/shared.module';
import { CatalogService } from './services/catalog.serivce';
import { CatalogItemComponent } from './catalog-item/catalog-item.component';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { UpdateProductPriceComponent } from './product-edit/update-product-price/update-product-price.component';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', component: CatalogComponent },
  { path: ':id', component: ProductEditComponent },
];

@NgModule({
  declarations: [
    CatalogComponent,
    CatalogItemComponent,
    ProductEditComponent,
    UpdateProductPriceComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(routes)
  ],
  providers: [
    CatalogService
  ]
})
export class CatalogModule { }
