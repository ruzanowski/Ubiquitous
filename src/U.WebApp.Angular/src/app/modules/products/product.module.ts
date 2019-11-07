import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {SharedModule} from '../shared/shared.module';
import {ProductService} from "./product.service";
import {ProductsComponent} from "./components/products.component";
import {ProductsDetailsComponent} from "./components/products-details/products-details.component";
import {
  MatCardModule,
  MatDatepickerModule, MatIconModule, MatInputModule,
  MatListModule,
  MatOptionModule,
  MatPaginatorModule,
  MatSelectModule,
  MatTableModule
} from "@angular/material";
import {CategoryModule} from "../categories/category.module";
import {CategoryService} from "../categories/category.service";
import {FlexModule} from "@angular/flex-layout";

@NgModule({
  imports: [BrowserModule, SharedModule, MatTableModule, MatPaginatorModule, MatOptionModule, MatSelectModule, CategoryModule, MatListModule, MatDatepickerModule, MatInputModule, MatIconModule, MatCardModule, FlexModule],
  declarations: [ProductsComponent, ProductsDetailsComponent],
  exports: [
    ProductsComponent
  ],
  providers: [ProductService, ProductsDetailsComponent, CategoryService]
})
export class ProductsModule {
}
