import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {SharedModule} from '../shared/shared.module';
import {ProductService} from "./product.service";
import {ProductsComponent} from "./components/products.component";
import {ProductsDetailsComponent} from "./components/products-details/products-details.component";
import {MatPaginatorModule, MatTableModule} from "@angular/material";

@NgModule({
  imports: [BrowserModule, SharedModule, MatTableModule, MatPaginatorModule],
  declarations: [ProductsComponent, ProductsDetailsComponent],
  providers: [ProductService, ProductsDetailsComponent]
})
export class ProductsModule {
}
