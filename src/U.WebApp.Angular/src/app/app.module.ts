import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';
import {AppComponent} from './app.component';
import {ProductsModule} from "./modules/products/product.module";
import {SharedModule} from "./modules/shared/shared.module";
import {ProductsComponent} from "./modules/products/components/products.component";
import {ProductsDetailsComponent} from "./modules/products/components/products-details/products-details.component";
import {CategoryModule} from "./modules/categories/category.module";
import {CategoryComponent} from "./modules/categories/components/table-category/table-categories.component";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {MatGridListModule, MatIconModule, MatNativeDateModule, MatTabsModule} from "@angular/material";
import {ManufacturerModule} from "./modules/manufacturers/manufacturer.module";

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    BrowserAnimationsModule,
    MatNativeDateModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      {path: '', component: AppComponent, pathMatch: 'full'},
      {path: 'products', component: ProductsComponent},
      {path: 'products/:id', component: ProductsDetailsComponent},
      {path: 'categories', component: CategoryComponent}
    ]),
    // Only module that app module loads
    SharedModule.forRoot(),
    ProductsModule,
    CategoryModule,
    MatGridListModule,
    MatTabsModule,
    ManufacturerModule,
    MatIconModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
