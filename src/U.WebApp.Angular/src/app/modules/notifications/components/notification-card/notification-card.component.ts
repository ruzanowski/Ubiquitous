import { Component, Input } from '@angular/core';
import {ProductBaseEvent} from "../../models/product-base-event.model";
import {ConfirmationType, IntegrationEventType, NotificationDto} from "../../models/notificationdto.model";

@Component({
  selector: 'notification-card',
  templateUrl: './notification-card.component.html',
  styleUrls: ['./notification-card.component.css']
})
export class NotificationCardComponent {

  public primary_color_primary: string = '#00695c';
  public primary_color_light: string  ='#439889';
  public primary_color_read: string = '#ffffff';
  public primary_color_dark: string  ='#003d33';

  public secondary_color_primary: string  = '#ad1457';
  public secondary_color_light: string  = '#e35183';
  public secondary_color_dark: string  = '#78002e';

  @Input()
  public notification: NotificationDto<ProductBaseEvent>;

  public getEventType()
  {
    switch (this.notification.eventType)
    {
      case IntegrationEventType.Unknown:
        return 'Unknown';
      case IntegrationEventType.ProductPublishedIntegrationEvent:
        return 'Product published';
      case IntegrationEventType.ProductPropertiesChangedIntegrationEvent:
        return 'Product changed';
      case IntegrationEventType.ProductAddedIntegrationEvent:
        return 'New product';
    }
  }

  public getColor() : string
  {
    console.log('state of notification is:'+ this.notification.state);

    switch (this.notification.state) {
      case ConfirmationType.unread:
        return this.primary_color_light;
      case ConfirmationType.read:
        return this.primary_color_read;
      case ConfirmationType.removed:
        return this.primary_color_light;
      case ConfirmationType.hidden:
        return this.primary_color_light;
    }
  }
}
