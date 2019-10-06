import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {SharedModule} from '../shared/shared.module';
import {ProductService} from "./product.service";
import {ProductsComponent} from "./components/products.component";
import {ProductsDetailsComponent} from "./components/products-details/products-details.component";

@NgModule({
  imports: [BrowserModule, SharedModule],
  declarations: [ProductsComponent, ProductsDetailsComponent],
  providers: [ProductService, ProductsDetailsComponent]
})
export class ProductsModule {
}
