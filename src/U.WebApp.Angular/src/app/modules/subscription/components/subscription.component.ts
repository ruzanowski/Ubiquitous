import {Component} from '@angular/core';
import {SubscriptionService} from "../subscription.service";
import {Preferences} from "../models/preferences.model";
import {AllowedEvents} from "../models/allowed-events.model";
import {ReactiveToasterService} from "../../notifications/services/toastr.service";
import {IntegrationEventType} from "../../notifications/models/integration-event-type.model";
import {Observable} from "rxjs";

@Component({
  selector: 'dashboard-subscription',
  templateUrl: './subscription.component.html',
  styleUrls: ['./subscription.component.css']
})

export class SubscriptionComponent
{
  preferences: Preferences;
  allowedEvents: IntegrationEventType[];

  ProductPublishedIntegrationEvent: boolean = false; // 1
  ProductPropertiesChangedIntegrationEvent: boolean = false; // 2
  ProductAddedIntegrationEvent: boolean = false; // 3
  NewProductFetched: boolean = false; // 4
  UserConnected: boolean = false; // 5
  UserDisconnected: boolean = false; // 6
  AccessTokenRefreshedIntegrationEvent: boolean = false; // 7
  SignedUp: boolean = false; // 8

  constructor(private subscription: SubscriptionService, private toastr: ReactiveToasterService)
  {
    this.subscription.getMyPreferences().subscribe(preferences =>
    {
      this.preferences = preferences;
      toastr.showToast('Preferences', 'Notifications preferences loaded', 'success');

    });

    this.subscription.getMyAllowedEvents().subscribe((allowedEvents) =>
    {
        allowedEvents.forEach((x) =>
        {


          if(x == 1)
          {
            this.allowedEvents.push(IntegrationEventType.ProductPublishedIntegrationEvent);
            this.ProductPublishedIntegrationEvent = true;
          }

          if(x ==  IntegrationEventType.ProductPropertiesChangedIntegrationEvent)
          {
            this.allowedEvents.push(IntegrationEventType.ProductPropertiesChangedIntegrationEvent);
            this.ProductPropertiesChangedIntegrationEvent = true;
          }

          if(x == IntegrationEventType.ProductPropertiesChangedIntegrationEvent)
          {
            this.allowedEvents.push(IntegrationEventType.ProductAddedIntegrationEvent);
            this.ProductAddedIntegrationEvent = true;
          }

          if(x == IntegrationEventType.ProductPropertiesChangedIntegrationEvent)
          {
            this.allowedEvents.push(IntegrationEventType.NewProductFetched);
          }

          if(x == IntegrationEventType.ProductPropertiesChangedIntegrationEvent)
          {
            this.allowedEvents.push(IntegrationEventType.UserConnected);
          }

          if(x == IntegrationEventType.ProductPropertiesChangedIntegrationEvent)
          {
            this.allowedEvents.push(IntegrationEventType.UserDisconnected);
          }

          if(x == IntegrationEventType.ProductPropertiesChangedIntegrationEvent)
          {
            this.allowedEvents.push(IntegrationEventType.AccessTokenRefreshedIntegrationEvent);
          }

          if(x == IntegrationEventType.ProductPropertiesChangedIntegrationEvent)
          {
            this.allowedEvents.push(IntegrationEventType.SignedUp);
          }
        });

      this.allowedEvents = allowedEvents;
        toastr.showToast('Preferences', 'Allowed events loaded', 'success');
    });
  }

  savePreferences()
  {

    this.subscription.setMyPreferences(this.preferences);
  }

  saveAllowedEvents()
  {
    if(this.ProductPublishedIntegrationEvent)
    {
      this.allowedEvents.push(IntegrationEventType.ProductPublishedIntegrationEvent);
    }

    if(this.ProductPropertiesChangedIntegrationEvent)
    {
      this.allowedEvents.push(IntegrationEventType.ProductPropertiesChangedIntegrationEvent);
    }

    if(this.ProductAddedIntegrationEvent)
    {
      this.allowedEvents.push(IntegrationEventType.ProductAddedIntegrationEvent);
    }

    if(this.NewProductFetched)
    {
      this.allowedEvents.push(IntegrationEventType.NewProductFetched);
    }

    if(this.UserConnected)
    {
      this.allowedEvents.push(IntegrationEventType.UserConnected);
    }

    if(this.UserDisconnected)
    {
      this.allowedEvents.push(IntegrationEventType.UserDisconnected);
    }

    if(this.AccessTokenRefreshedIntegrationEvent)
    {
      this.allowedEvents.push(IntegrationEventType.AccessTokenRefreshedIntegrationEvent);
    }

    if(this.SignedUp)
    {
      this.allowedEvents.push(IntegrationEventType.SignedUp);
    }

    this.subscription.setMyAllowedEvents(this.allowedEvents);
  }

}





