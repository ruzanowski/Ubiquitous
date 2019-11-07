import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {MatDividerModule, MatIconModule, MatPaginatorModule, MatSidenavModule, MatTableModule} from "@angular/material";
import {InfiniteScrollModule} from "ngx-infinite-scroll";
import {SharedModule} from "../shared/shared.module";
import {NotificationCardComponent} from "./components/todo-notification-card/notification-card.component";
import {SignalrService} from "./services/signalr.service";
import {NotificationsComponent} from "./components/notifications-list/notifications.component";

@NgModule({
  imports: [BrowserModule, SharedModule, MatTableModule, MatPaginatorModule, MatDividerModule, InfiniteScrollModule, MatIconModule, MatSidenavModule],
  declarations: [NotificationsComponent, NotificationCardComponent],
  exports: [
    NotificationsComponent, NotificationCardComponent, InfiniteScrollModule
  ],
  providers: [SignalrService]
})
export class NotificationsModule {

}
