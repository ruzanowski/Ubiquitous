import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {ManufacturerService} from "../../../manufacturers/manufacturers.service";
import {NotificationDto} from "../../models/notification-dto.model";
import {IntegrationEventType} from "../../models/integration-event-type.model";
import {ConfirmationType} from "../../models/confirmation-type.model";
import {NotificationService} from "../../services/notification.service";
import {Importancy} from "../../models/importancy.model";
import {ProductBaseEvent} from "../../models/events/product/product-base-event.model";
import {BaseEvent} from "../../models/events/base-event.model";
import {UserEventBase} from "../../models/events/identity/user-event-base.model";

@Component({
  selector: 'notification-card',
  templateUrl: './notification-card.component.html',
  styleUrls: ['./notification-card.component.css']
})
export class NotificationCardComponent
  implements OnInit
{
  public primary_color_light: string = '#66bb6a';
  public primary_color_read: string = '#c4c4c4';
  public primary_color_dark: string = '#003d33';

  public manufacturerName: string = "none";
  public importancyIcon: string = "star_border";
  @Input()
  public notification: NotificationDto<BaseEvent>;

  constructor(private manufacturerService: ManufacturerService, private notificationService: NotificationService) {
  }

  ngOnInit(): void {


    if (this.notification.eventType > 0 && this.notification.eventType < 4) {
      this.manufacturerService.getManufacturer(((this.notification.event as ProductBaseEvent).manufacturer)).subscribe((data) => {
        console.log(data);
        this.manufacturerName = data.name;
      });
    }
    else if (this.notification.eventType == 5 || this.notification.eventType == 6)
    {
      this.manufacturerName = (this.notification.event as UserEventBase).nickname;
    }

    this.bindColor();
    this.bindImportancy();
  }

  public deleteNotification() {
    this.notification.state = ConfirmationType.hidden;
    this.notificationService.delete(this.notification);
  }


  public hideNotification() {
    this.notification.state = ConfirmationType.hidden;
    this.notificationService.hide(this.notification);
  }

  public iterateImportancy() {
    if (this.notification.importancy == Importancy.trivial) {
      this.notification.importancy = Importancy.normal;
    } else if (this.notification.importancy == Importancy.normal) {
      this.notification.importancy = Importancy.important;

    } else {
      this.notification.importancy = Importancy.trivial;

    }
    this.bindImportancy();
    this.notificationService.changeImportancy(this.notification);
  }

  public bindImportancy() {
    switch (this.notification.importancy) {
      case Importancy.trivial:
        this.importancyIcon = "star_border";
        break;
      case Importancy.normal:
        this.importancyIcon = "star_half";
        break;
      case Importancy.important:
        this.importancyIcon = "star";
        break;

    }
  }

  public bindColor(): string {
    switch (this.notification.state) {
      case ConfirmationType.unread:
        return this.primary_color_light;
      case ConfirmationType.read:
        return this.primary_color_read;
      case ConfirmationType.hidden:
        return this.primary_color_light;
    }
  }

  public bindEventType() {
    switch (this.notification.eventType) {
      case IntegrationEventType.NewProductFetched:
        return 'NewProductFetched';
      case IntegrationEventType.UserConnected:
        return 'User Joined';
      case IntegrationEventType.UserDisconnected:
        return 'User Left';
      case IntegrationEventType.AccessTokenRefreshedIntegrationEvent:
        return 'Access Token Refreshed';
      case IntegrationEventType.SignedUp:
        return 'SignedUp';
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

  public bindStateAsRead() {
    this.notification.state = ConfirmationType.read;
  }

}
