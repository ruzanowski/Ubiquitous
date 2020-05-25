import {Component, OnDestroy, OnInit, Output} from '@angular/core';
import {NotificationService} from "../../services/notification.service";
import {NotificationDto} from "../../models/notification-dto.model";
import {ConfirmationType} from "../../models/confirmation-type.model";

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
  }

  ngOnInit(): void {
    console.log('notification list init');
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

  unreadNotifications(){
    return this.notificationService._notificationsData.filter(selector => selector.state == ConfirmationType.unread).length;
  }
  readNotifications(){
    return this.notificationService._notificationsData.filter(selector => selector.state == ConfirmationType.read).length;
  }

  setStateToRead(notification: NotificationDto<any>) : void
  {
    this.notificationService.read(notification);
  }

  setAllStateToRead(notifications: Array<NotificationDto<any>>) : void
  {
    this.notificationService.readAll(notifications);
  }
}
