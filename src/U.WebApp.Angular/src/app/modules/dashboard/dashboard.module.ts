import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {
  MatButtonModule,
  MatDividerModule, MatGridListModule,
  MatIconModule,
  MatPaginatorModule,
  MatTableModule,
  MatTabsModule, MatTooltipModule
} from "@angular/material";
import {ManufacturerModule} from "../manufacturers/manufacturer.module";
import {CategoryModule} from "../categories/category.module";
import {ProductsModule} from "../products/product.module";
import {DashboardMainTabsComponent} from "./components/dashboard-main-tabs/dashboard-main-tabs.component";
import {DashboardComponent} from "./components/dashboard/dashboard.component";
import {ChartistModule} from "ng-chartist";
import {NgbModule} from "@ng-bootstrap/ng-bootstrap";
import { ChartsModule } from 'ng2-charts';
import {NotificationsBarChartComponent} from "./components/dashboard/dashboard-chart-notifications/notifications-bar-chart.component";
import {
  AsyncChartComponent,
} from "./components/dashboard/dashboard-chart-notifications/notifications-pie-chart.component";
import {LiveChartComponent} from "./components/dashboard/dashboard-chart-notifications/notifications-live-chart.component";

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
      MatTabsModule,
      MatGridListModule,
      MatTooltipModule,
      ChartistModule,
      MatButtonModule,
      NgbModule,
      ChartsModule
    ],
  declarations: [DashboardMainTabsComponent, DashboardComponent, NotificationsBarChartComponent, LiveChartComponent, AsyncChartComponent ],
  exports: [
    DashboardMainTabsComponent, DashboardComponent, NotificationsBarChartComponent, LiveChartComponent, AsyncChartComponent
  ],
  providers: [DashboardMainTabsComponent, DashboardComponent]
})
export class DashboardModule {
}
