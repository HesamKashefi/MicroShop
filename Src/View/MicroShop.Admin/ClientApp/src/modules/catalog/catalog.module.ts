import { NgModule } from '@angular/core';
import { CatalogComponent } from './catalog.component';
import { SharedModule } from '../shared/shared.module';
import { CatalogService } from './services/catalog.serivce';
import { CatalogItemComponent } from './catalog-item/catalog-item.component';



@NgModule({
  declarations: [
    CatalogComponent,
    CatalogItemComponent
  ],
  imports: [
    SharedModule
  ],
  providers: [
    CatalogService
  ]
})
export class CatalogModule { }
