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

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      {path: '', component: ProductsComponent, pathMatch: 'full'},
      {path: 'products', component: ProductsComponent},
      {path: 'products/:id', component: ProductsDetailsComponent}
    ]),
    // Only module that app module loads
    SharedModule.forRoot(),
    ProductsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
