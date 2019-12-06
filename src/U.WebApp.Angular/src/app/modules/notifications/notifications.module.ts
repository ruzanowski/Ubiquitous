import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {
  MatButtonModule,
  MatButtonToggleModule, MatCardModule,
  MatDividerModule, MatExpansionModule,
  MatIconModule, MatListModule, MatMenuModule,
  MatPaginatorModule, MatRadioModule,
  MatSidenavModule,
  MatTableModule
} from "@angular/material";
import {InfiniteScrollModule} from "ngx-infinite-scroll";
import {SharedModule} from "../shared/shared.module";
import {SignalrService} from "./services/signalr.service";
import {NotificationCardComponent} from "./components/notification-card/notification-card.component";
import {NotificationsComponent} from "./components/notifications-list/notifications-list.component";
import {NotificationService} from "./services/notification.service";
import {DragDropModule} from "@angular/cdk/drag-drop";
import {ReactiveToasterService} from "./services/toastr.service";
import {ToastrModule, ToastrService} from "ngx-toastr";

@NgModule({
  imports: [
    BrowserModule,
    SharedModule,
    MatTableModule,
    MatPaginatorModule,
    MatDividerModule,
    InfiniteScrollModule,
    MatIconModule,
    MatSidenavModule,
    MatButtonToggleModule,
    MatRadioModule,
    MatButtonModule,
    MatListModule,
    MatExpansionModule,
    MatMenuModule,
    DragDropModule,
    MatCardModule,
    ToastrModule.forRoot()
  ],
  declarations:
    [
    NotificationsComponent,
    NotificationCardComponent,
    NotificationCardComponent,
    ],
  exports:
    [
    NotificationsComponent,
    NotificationCardComponent,
    InfiniteScrollModule,
  ],
  providers:
    [
    SignalrService,
    NotificationService,
    ToastrService,
    ReactiveToasterService
    ]
})
export class NotificationsModule {

}
