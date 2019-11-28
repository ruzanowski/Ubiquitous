import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {ProductBaseEvent} from "../../models/product-base-event.model";
import {ManufacturerService} from "../../../manufacturers/manufacturers.service";
import {NotificationDto} from "../../models/notification-dto.model";
import {IntegrationEventType} from "../../models/integration-event-type.model";
import {ConfirmationType} from "../../models/confirmation-type.model";
import {NotificationService} from "../../services/notification.service";
import {Importancy} from "../../models/importancy.model";
import {AuthGuard} from "../../../auth";

@Component({
  selector: 'notification-card',
  templateUrl: './notification-card.component.html',
  styleUrls: ['./notification-card.component.css']
})
export class NotificationCardComponent implements OnInit, OnDestroy
{

  public primary_color_primary: string = '#00695c';
  public primary_color_light: string  ='#439889';
  public primary_color_read: string = '#707070';
  public primary_color_dark: string  ='#003d33';

  public manufacturerName: string = "none";
  public importancyIcon: string = "star_border";
  @Input()
  public notification: NotificationDto<ProductBaseEvent>;

  constructor(private manufacturerService: ManufacturerService, private notificationService: NotificationService)
  {
  }

  ngOnInit(): void {

    this.manufacturerService.getManufacturer(this.notification.event.manufacturer).subscribe((data)=>
    {
      console.log(data);
      this.manufacturerName = data.name;
    });

    console.log('notification card init');


    this.bindColor();
    this.bindImportancy();
  }
  ngOnDestroy(): void {
    console.log('notification card destroy');
  }

  public deleteNotification(){
    this.notification.state = ConfirmationType.hidden;
   this.notificationService.delete(this.notification);
  }


  public hideNotification(){
    this.notification.state = ConfirmationType.hidden;
   this.notificationService.hide(this.notification);
  }

  public iterateImportancy()
  {
    if(this.notification.importancy == Importancy.trivial)
    {
      this.notification.importancy = Importancy.normal;
    }
    else if (this.notification.importancy == Importancy.normal){
      this.notification.importancy = Importancy.important;

    }
    else {
      this.notification.importancy = Importancy.trivial;

    }
    this.bindImportancy();
    this.notificationService.changeImportancy(this.notification);
  }

  public bindImportancy(){
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

  public bindColor() : string
  {
    switch (this.notification.state) {
      case ConfirmationType.unread:
        return this.primary_color_light;
      case ConfirmationType.read:
        return this.primary_color_read;
      case ConfirmationType.hidden:
        return this.primary_color_light;
    }
  }

  public bindEventType()
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

  public bindStateAsRead(){
    this.notification.state = ConfirmationType.read;
  }

}
