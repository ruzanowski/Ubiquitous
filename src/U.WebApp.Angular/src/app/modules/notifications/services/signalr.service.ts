import {Injectable} from '@angular/core';
import * as signalR from "@aspnet/signalr";
import {LogLevel} from "@aspnet/signalr";
import {Subject} from "rxjs";
import {NotificationDto} from "../models/notification-dto.model";
import {IntegrationEventType} from "../models/integration-event-type.model";
import {Importancy} from "../models/importancy.model";
import {AuthenticationService} from "../../auth";
import {ReactiveToasterService} from "./toastr.service";
import {ProductBaseEvent} from "../models/events/product/product-base-event.model";
import {ProductPropertiesChangedEvent} from "../models/events/product/product-properties-changed-event.model";
import {ProductAddedEvent} from "../models/events/product/product-added-event.model";
import {ProductPublishedEvent} from "../models/events/product/product-published-event.model";
import {UserConnectedEvent} from "../models/events/identity/user-connected.model";
import {UserDisconnectedEvent} from "../models/events/identity/user-disconnected.model";

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  connection: signalR.HubConnection;
  public productAdded$ = new Subject<NotificationDto<ProductAddedEvent>>();
  public productPublished$: Subject<NotificationDto<ProductPublishedEvent>> = new Subject();
  public productPropertiesChanged$: Subject<NotificationDto<ProductPropertiesChangedEvent>> = new Subject();
  public welcomeMessage$: Subject<NotificationDto<ProductBaseEvent>> = new Subject();
  public usersConnected$: Subject<NotificationDto<UserConnectedEvent>> = new Subject();
  public usersDisconnected$: Subject<NotificationDto<UserDisconnectedEvent>> = new Subject();

  constructor(private authenticationService: AuthenticationService, private toastr: ReactiveToasterService) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:4500/signalr',  { accessTokenFactory: () => this.authenticationService.currentUserValue.accessToken })
      .configureLogging(LogLevel.Trace)
      .build();
    this.subscribeOnEvents();
    this.connect();
  }

  private connect() {
    if (this.connection.state === signalR.HubConnectionState.Disconnected) {
      this.connection
        .start()
        .catch(err =>
        {
          this.authenticationService.logout();
          this.toastr.showToast('Fatal Error','Could not connect to Notification Service', 'error');
          console.log(err)
        });
    }
  }

  private subscribeOnEvents() {

    this.connection.on('UserConnectedIntegrationEvent', (user: NotificationDto<UserConnectedEvent>) => {
      console.log(JSON.stringify(user));
      this.usersConnected$.next(user);
    });

    this.connection.on('UserDisconnectedIntegrationEvent', (user: NotificationDto<UserConnectedEvent>) => {
      console.log(JSON.stringify(user));
      this.usersDisconnected$.next(user);
    });

    this.connection.on('WelcomeNotifications', (welcomeNotification: NotificationDto<ProductBaseEvent>) =>
    {
      this.MatchTypeOfWelcomeMessage(welcomeNotification);
    });

    this.connection.on('ProductPublishedSignalRIntegrationEvent', (product: NotificationDto<ProductPublishedEvent>) => {
      this.productPublished$.next(product);
    });

    this.connection.on('ProductPropertiesChangedSignalRIntegrationEvent', (product: NotificationDto<ProductPropertiesChangedEvent>) => {
      this.productPropertiesChanged$.next(product);
    });

    this.connection.on('ProductAddedSignalRIntegrationEvent', (product: NotificationDto<ProductAddedEvent>) => {
      this.productAdded$.next(product);
    });
  }

  private MatchTypeOfWelcomeMessage(welcomeNotification: NotificationDto<ProductBaseEvent>)
  {
    switch (welcomeNotification.eventType) {
      case IntegrationEventType.ProductPublishedIntegrationEvent:
        this.productPublished$.next(welcomeNotification as NotificationDto<ProductPublishedEvent>);
        break;
      case IntegrationEventType.ProductPropertiesChangedIntegrationEvent:
        this.productPropertiesChanged$.next(welcomeNotification as NotificationDto<ProductPropertiesChangedEvent>);
        break;
      case IntegrationEventType.ProductAddedIntegrationEvent:
        this.productAdded$.next(welcomeNotification as NotificationDto<ProductAddedEvent>);
        break;
      default:
      case IntegrationEventType.Unknown:
        console.log(JSON.stringify('Could not determine type of Notification.' +
          ' Please resolve this issue! Problem occured with parsing json: ' + JSON.stringify(welcomeNotification)
        ));
        break;
    }
  }

  invokeReadNotification(notificationId: string)
  {
    this.connection.invoke('confirmReadNotification', notificationId)
      .catch(err => console.error(err));
  }

  invokeHideNotification(notificationId: string)
  {
    this.connection.invoke('hideNotification', notificationId)
      .catch(err => console.error(err));
  }


  invokeDeleteNotification(notificationId: string)
  {
    this.connection.invoke('deleteNotification', notificationId)
      .catch(err => console.error(err));
  }

  invokeChangeImportancyNotification(notificationId: string, importancy: Importancy)
  {
    this.connection.invoke('changeNotificationImportancy', notificationId, importancy.toString())
      .catch(err => console.error(err));
  }

  public disconnect() {
    this.connection.stop();
  }
}
