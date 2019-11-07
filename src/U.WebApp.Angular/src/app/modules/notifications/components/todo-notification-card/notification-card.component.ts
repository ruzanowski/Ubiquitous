import { Component, Input } from '@angular/core';
import {NotificationDto} from "../../models/notification.model";

@Component({
  selector: 'notification-card',
  templateUrl: './notification-card.component.html',
  styleUrls: ['./notification-card.component.css']
})
export class NotificationCardComponent {
  private notifications: NotificationDto<any>;

  @Input()
  set notification(value: NotificationDto<any>) {
    this.notifications = value;
    console.log('notification in notification card: ' + JSON.stringify(value));
  }

  get notification() {
    return this.notifications;
  }
}
