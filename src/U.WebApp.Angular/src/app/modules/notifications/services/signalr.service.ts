import {Injectable} from '@angular/core';
import * as signalR from "@aspnet/signalr";
import {LogLevel} from "@aspnet/signalr";
import {ProductAddedEvent} from "../models/product-added-event.model";
import {IntegrationEventType, NotificationDto} from "../models/notification.model";
import {ProductPublishedEvent} from "../models/product-published-event.model";
import {ProductPropertiesChangedEvent} from "../models/product-properties-changed-event.model";
import {Observable, Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  connection: signalR.HubConnection;
  public productAdded$: Subject<NotificationDto<ProductAddedEvent>> = new Subject();
  public productPublished$: Subject<NotificationDto<ProductAddedEvent>> = new Subject();
  public productPropertiesChanged$: Subject<NotificationDto<ProductPropertiesChangedEvent>> = new Subject();
  public welcomeMessage$: Subject<NotificationDto<object>> = new Subject();
  public usersConnected: Array<string> = [];

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5500/signalr')
      .configureLogging(LogLevel.Trace)
      .build();
  }

  public connect() {
    if (this.connection.state === signalR.HubConnectionState.Disconnected) {
      this.connection
        .start()
        .catch(err => console.log(err));
    }
  }

  public subscribeOnEvents() {

    this.connection.on('connected', (user: string) => {
      console.log(JSON.stringify(user));
      this.usersConnected.push(user);
    });

    this.connection.on('disconnected', (user: string) => {
      console.log(JSON.stringify(user));
      this.usersConnected = this.usersConnected.filter(item => item !== user);
    });

    this.connection.on('WelcomeNotifications', (welcomeNotification: NotificationDto<object>) => {

      console.log('WelcomeNotifications in signalr' + JSON.stringify(welcomeNotification));
      this.welcomeMessage$.next(welcomeNotification as NotificationDto<any>);

      switch (welcomeNotification.EventType) {
        case IntegrationEventType.Unknown:
          console.log(JSON.stringify('Could not determine type of Notification.' +
            ' Please resolve this issue! Problem occured with parsing json: ' + welcomeNotification
          ));
          break;
        case IntegrationEventType.ProductPublishedIntegrationEvent:
          this.productPublished$.next(welcomeNotification as NotificationDto<ProductPublishedEvent>);
          break;
        case IntegrationEventType.ProductPropertiesChangedIntegrationEvent:
          this.productPropertiesChanged$.next(welcomeNotification as NotificationDto<ProductPropertiesChangedEvent>);
          break;
        case IntegrationEventType.ProductAddedIntegrationEvent:
          this.productAdded$.next(welcomeNotification as NotificationDto<ProductAddedEvent>);
          break;
      }
    });

    this.connection.on('ProductPublishedIntegrationEvent', (product: NotificationDto<ProductPublishedEvent>) => {
      this.productPublished$.next(product);
    });

    this.connection.on('ProductPropertiesChangedIntegrationEvent', (product: NotificationDto<ProductPropertiesChangedEvent>) => {
      this.productPropertiesChanged$.next(product);
    });

    this.connection.on('ProductAddedIntegrationEvent', (product: NotificationDto<ProductAddedEvent>) => {
      this.productAdded$.next(product);
    });
  }

  public getProductPropertiesChanged() :Observable<NotificationDto<ProductPropertiesChangedEvent>>
  {
    return this.productPropertiesChanged$.asObservable();
  }

  public getProductAdded() :Observable<NotificationDto<ProductAddedEvent>>
  {
    return this.productAdded$.asObservable();
  }

  public getProductPublished() :Observable<NotificationDto<ProductPublishedEvent>>
  {
    return this.productPublished$.asObservable();
  }
  public getWelcomeMessages() :Observable<NotificationDto<object>>
  {
    return this.welcomeMessage$.asObservable();
  }

  public disconnect() {
    this.connection.stop();
  }
}
