import {EventEmitter, Injectable, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {NotificationDto} from "../models/notification-dto.model";
import {ProductBaseEvent} from "../models/product-base-event.model";
import {Subscription} from "rxjs";
import {ProductAddedEvent} from "../models/product-added-event.model";
import {ProductPropertiesChangedEvent} from "../models/product-properties-changed-event.model";
import {ProductPublishedEvent} from "../models/product-published-event.model";
import {SignalrService} from "./signalr.service";
import {ConfirmationType} from "../models/confirmation-type.model";

@Injectable({
  providedIn: 'root'
})
export class NotificationService{

  @Input()
  numOfItemsToShow = 5;
  itemsToLoad = 5;
  isFullListDisplayed = false;

  public _notificationsData: Array<NotificationDto<ProductBaseEvent>> = [];
  private productsAddedEventsSubscription: Subscription;
  private productsPropertiesChangedEventsSubscription: Subscription;
  private productsPublishedEventsSubscription: Subscription;
  private welcomeMessagesSubscription: Subscription;

  public notificationsToShow: Array<NotificationDto<ProductBaseEvent>> = [];

  public productsAddedEvents: Array<NotificationDto<ProductAddedEvent>> = [];
  public productsPropertiesChangedEvents: Array<NotificationDto<ProductPropertiesChangedEvent>> = [];
  public productsPublishedEvents: Array<NotificationDto<ProductPublishedEvent>> = [];

  @Output() notificationsBadgeEvent = new EventEmitter();

  constructor(public signalr: SignalrService)
  {

    this.signalr.subscribeOnEvents();
  }

  set notifications(data: NotificationDto<ProductBaseEvent>) {
    this.numOfItemsToShow = this._notificationsData.push(data);
    this.notificationsToShow = this._notificationsData.slice(0, this.numOfItemsToShow);
    this.isFullListDisplayed = false;
    this.emitChangeNumberOfNotifications();
  }

  registerSubscriptions(){

    this.productsAddedEventsSubscription = this.signalr.productAdded$.asObservable().subscribe(
      (productAdded) => {
        this.productsAddedEvents.push(productAdded);
        this.notifications = productAdded;
      });

    this.productsPropertiesChangedEventsSubscription = this.signalr.productPropertiesChanged$.asObservable().subscribe(
      (productPropertiesChanged) => {
        this.productsPropertiesChangedEvents.push(productPropertiesChanged);
        this.notifications = productPropertiesChanged;
      });

    this.productsPublishedEventsSubscription = this.signalr.productPublished$.asObservable().subscribe(
      (productPublished) => {
        this.productsPublishedEvents.push(productPublished);
        this.notifications = productPublished;
      });

    this.welcomeMessagesSubscription = this.signalr.welcomeMessage$.asObservable().subscribe(
      (welcomeMessage) => {
        this.notifications = welcomeMessage;
      });
  }

  emitChangeNumberOfNotifications()
  {
    this.notificationsBadgeEvent.emit(this._notificationsData.filter(value => value.state === ConfirmationType.unread).length);
  }

  anyNotifications() {
    return this._notificationsData.length;
  }

  read(notification: NotificationDto<any>) : void
  {
    notification.state = ConfirmationType.read;
    this.emitChangeNumberOfNotifications();
    this.signalr.invokeReadNotification(notification.id);
  }

  readAll(notifications: Array<NotificationDto<any>>) : void
  {
    notifications.forEach(notification =>
    {
      notification.state = ConfirmationType.read;
      this.emitChangeNumberOfNotifications();
      this.signalr.invokeReadNotification(notification.id);
    });
  }

  hide(notification: NotificationDto<any>) : void
  {
    notification.state = ConfirmationType.hidden;
    this.signalr.invokeHideNotification(notification.id);
  }

  delete(notification: NotificationDto<any>) : void
  {
    notification.state = ConfirmationType.hidden;
    this.signalr.invokeDeleteNotification(notification.id);
  }

  changeImportancy(notification: NotificationDto<any>) : void
  {
    notification.state = ConfirmationType.hidden;
    this.signalr.invokeChangeImportancyNotification(notification.id, notification.importancy);
  }
}
