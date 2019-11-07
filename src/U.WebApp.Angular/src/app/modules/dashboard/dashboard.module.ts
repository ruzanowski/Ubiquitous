import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {
  MatDividerModule,
  MatIconModule,
  MatPaginatorModule,
  MatTableModule,
  MatTabsModule
} from "@angular/material";
import {ManufacturerModule} from "../manufacturers/manufacturer.module";
import {CategoryModule} from "../categories/category.module";
import {ProductsModule} from "../products/product.module";
import {DashboardMainTabsComponent} from "./components/dashboard-main-tabs/dashboard-main-tabs.component";
import {DashboardComponent} from "./components/dashboard/dashboard.component";

@NgModule({
  imports:
    [BrowserModule,
      MatTableModule,
      MatPaginatorModule,
      MatDividerModule,
      MatIconModule,
      ManufacturerModule,
      CategoryModule,
      ProductsModule,
      MatTabsModule],
  declarations: [DashboardMainTabsComponent, DashboardComponent],
  exports: [
    DashboardMainTabsComponent, DashboardComponent
  ],
  providers: [DashboardMainTabsComponent, DashboardComponent],
  entryComponents: [DashboardMainTabsComponent, DashboardComponent]
})
export class DashboardModule {
}
