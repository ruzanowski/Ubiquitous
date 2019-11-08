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
import {
  MatButtonModule,
  MatGridListModule,
  MatIconModule, MatListModule,
  MatNativeDateModule,
  MatSidenavModule,
  MatTabsModule
} from "@angular/material";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {FlexLayoutModule} from "@angular/flex-layout";
import {ManufacturersComponent} from "./modules/manufacturers/components/table-manufacturers/table-manufacturers.component";
import {NotificationsModule} from "./modules/notifications/notifications.module";
import {DashboardModule} from "./modules/dashboard/dashboard.module";
import {DashboardComponent} from "./modules/dashboard/components/dashboard/dashboard.component";
import {OverlayModule} from "@angular/cdk/overlay";

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    BrowserAnimationsModule,
    MatNativeDateModule,
    HttpClientModule,
    RouterModule.forRoot([
      {path: '', redirectTo: '/dashboard', pathMatch: 'full'},
      {path: 'dashboard', component: DashboardComponent},
      {path: 'products', component: ProductsComponent},
      {path: 'products/:id', component: ProductsDetailsComponent},
      {path: 'categories', component: CategoryComponent},
      {path: 'manufacturers', component: ManufacturersComponent}
    ]),
    FlexLayoutModule,
    MatIconModule,
    // Only module that app module loads
    SharedModule.forRoot(),
    ProductsModule,
    CategoryModule,
    MatGridListModule,
    MatTabsModule,
    ManufacturerModule,
    MatSidenavModule,
    MatListModule,
    MatButtonModule,
    DashboardModule,
    NotificationsModule,
    OverlayModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
