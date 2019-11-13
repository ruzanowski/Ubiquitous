import {Component, EventEmitter, OnDestroy, OnInit, Output} from '@angular/core';
import {NotificationService} from "../../services/notification.service";
import {NotificationDto} from "../../models/notification-dto.model";
import {ConfirmationType} from "../../models/confirmation-type.model";
import {CdkDragDrop, moveItemInArray} from "@angular/cdk/drag-drop";
import {ProductBaseEvent} from "../../models/product-base-event.model";

@Component({
  selector: 'notifications-list',
  templateUrl: './notifications-list.component.html',
  styleUrls: ['./notifications-list.component.css']
})
export class NotificationsComponent implements OnInit, OnDestroy
{
  @Output() notificationsBadgeEvent;

  constructor(private notificationService: NotificationService)
  {
    this.notificationsBadgeEvent = this.notificationService.notificationsBadgeEvent;
  }

  ngOnDestroy(): void {
    console.log('destroy');
    this.notificationService.signalr.disconnect();
  }

  ngOnInit(): void {
    console.log('init');
    this.notificationService.signalr.connect();
    this.notificationService.registerSubscriptions();
  }

  onScroll() {

    if (this.notificationService.numOfItemsToShow <= this.notificationService._notificationsData.length) {
      this.notificationService.numOfItemsToShow += this.notificationService.itemsToLoad;
      this.notificationService.notificationsToShow = this.notificationService._notificationsData.slice(0, this.notificationService.numOfItemsToShow);
    } else {
      this.notificationService.isFullListDisplayed = true;
    }
  }
  anyNotifications() {
    return this.notificationService._notificationsData.length;
  }

  setStateToRead(notification: NotificationDto<any>) : void
  {
    this.notificationService.read(notification);
  }

  setAllStateToRead(notifications: Array<NotificationDto<any>>) : void
  {
    this.notificationService.readAll(notifications);
  }

  drop(event: CdkDragDrop<Array<NotificationDto<ProductBaseEvent>>>) {
    moveItemInArray(this.notificationService.notificationsToShow, event.previousIndex, event.currentIndex);
  }

}
