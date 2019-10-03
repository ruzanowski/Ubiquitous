import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {SharedModule} from '../shared/shared.module';
import {ProductService} from "./product.service";
import {ProductsComponent} from "./components/products.component";

@NgModule({
  imports: [BrowserModule, SharedModule],
  declarations: [ProductsComponent],
  providers: [ProductService]
})
export class ProductsModule {
}
