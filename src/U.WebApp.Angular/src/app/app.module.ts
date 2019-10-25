import {AppComponent} from './app.component';
import {ProductsModule} from "./modules/products/product.module";
import {SharedModule} from "./modules/shared/shared.module";
import {ProductsComponent} from "./modules/products/components/products.component";
import {ProductsDetailsComponent} from "./modules/products/components/products-details/products-details.component";
import {CategoryModule} from "./modules/categories/category.module";
import {CategoryComponent} from "./modules/categories/components/table-category/table-categories.component";
import {ManufacturerModule} from "./modules/manufacturers/manufacturer.module";
import {BrowserModule} from "@angular/platform-browser";
import {HttpClientModule} from "@angular/common/http";
import {RouterModule} from "@angular/router";
import {NgModule} from "@angular/core";
import {MatGridListModule, MatIconModule, MatNativeDateModule, MatTabsModule} from "@angular/material";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {FlexLayoutModule} from "@angular/flex-layout";

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
BrowserAnimationsModule,
    MatNativeDateModule,
    HttpClientModule,
    RouterModule.forRoot([
      {path: '', component: AppComponent, pathMatch: 'full'},
      {path: 'products', component: ProductsComponent},
      {path: 'products/:id', component: ProductsDetailsComponent},
      {path: 'categories', component: CategoryComponent}
    ]),
    FlexLayoutModule,
    MatIconModule,
    // Only module that app module loads
    SharedModule.forRoot(),
    ProductsModule,
    CategoryModule,
    MatGridListModule,
    MatTabsModule,
    ManufacturerModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
