import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {SharedModule} from '../shared/shared.module';
import {
  MatButtonModule,
  MatCardModule,
  MatDatepickerModule, MatIconModule, MatInputModule,
  MatListModule,
  MatOptionModule,
  MatPaginatorModule,
  MatSelectModule,
  MatTableModule, MatTooltipModule
} from "@angular/material";
import {FlexModule} from "@angular/flex-layout";
import {SubscriptionComponent} from "./components/subscription.component";
import {SubscriptionService} from "./subscription.service";

@NgModule({
  imports: [
    BrowserModule,
    SharedModule,
    MatTableModule,
    MatPaginatorModule,
    MatOptionModule,
    MatSelectModule,
    MatListModule,
    MatDatepickerModule,
    MatInputModule,
    MatIconModule,
    MatCardModule,
    FlexModule,
    MatTooltipModule,
    MatButtonModule
  ],
  declarations: [SubscriptionComponent],
  exports: [],
  providers: [SubscriptionService]
})
export class SubscriptionModule {
}
