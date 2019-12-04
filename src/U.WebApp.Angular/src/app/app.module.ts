import {AppComponent} from './app.component';
import {ProductsModule} from "./modules/products/product.module";
import {SharedModule} from "./modules/shared/shared.module";
import {CategoryModule} from "./modules/categories/category.module";
import {ManufacturerModule} from "./modules/manufacturers/manufacturer.module";
import {BrowserModule} from "@angular/platform-browser";
import {HttpClientModule} from "@angular/common/http";
import {NgModule} from "@angular/core";
import {
  MatButtonModule,
  MatGridListModule,
  MatIconModule, MatListModule,
  MatNativeDateModule,
  MatSidenavModule,
  MatTabsModule, MatToolbarModule
} from "@angular/material";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {FlexLayoutModule} from "@angular/flex-layout";
import {NotificationsModule} from "./modules/notifications/notifications.module";
import {DashboardModule} from "./modules/dashboard/dashboard.module";
import {OverlayModule} from "@angular/cdk/overlay";
import {AppAuthModule} from "./modules/auth";
import {HomeLayoutComponent} from "./layouts/home-layout/home-layout.component";
import {LoginLayoutComponent} from "./layouts/login-layout/login-layout.component";
import {AppRoutingModule} from "./app-routing.module";
import {SubscriptionModule} from "./modules/subscription/subscription.module";

@NgModule({
  declarations: [AppComponent,
    HomeLayoutComponent,
    LoginLayoutComponent],
  imports: [
    AppRoutingModule,
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    BrowserAnimationsModule,
    MatNativeDateModule,
    HttpClientModule,
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
    OverlayModule,
    MatToolbarModule,
    AppAuthModule,
    SubscriptionModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
