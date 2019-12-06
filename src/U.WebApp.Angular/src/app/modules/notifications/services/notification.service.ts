import {EventEmitter, Injectable, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {NotificationDto} from "../models/notification-dto.model";
import {Subscription} from "rxjs";
import {SignalrService} from "./signalr.service";
import {ConfirmationType} from "../models/confirmation-type.model";
import {ReactiveToasterService} from "./toastr.service";
import {ProductAddedEvent} from "../models/events/product/product-added-event.model";
import {ProductPropertiesChangedEvent} from "../models/events/product/product-properties-changed-event.model";
import {ProductPublishedEvent} from "../models/events/product/product-published-event.model";
import {BaseEvent} from "../models/events/base-event.model";
import {UserConnectedEvent} from "../models/events/identity/user-connected.model";
import {UserDisconnectedEvent} from "../models/events/identity/user-disconnected.model";
import {map} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class NotificationService{

  @Input()
  numOfItemsToShow = 5;
  itemsToLoad = 5;
  isFullListDisplayed = false;

  public _notificationsData: Array<NotificationDto<BaseEvent>> = [];
  public notificationsToShow: Array<NotificationDto<BaseEvent>> = [];
  public productsAddedEvents: Array<NotificationDto<ProductAddedEvent>> = [];
  public productsPropertiesChangedEvents: Array<NotificationDto<ProductPropertiesChangedEvent>> = [];
  public productsPublishedEvents: Array<NotificationDto<ProductPublishedEvent>> = [];
  public usersLogged: Array<string> = [];

  private productsAddedEvents$: Subscription;
  private productsPropertiesChangedEvents$: Subscription;
  private productsPublishedEvents$: Subscription;
  private welcomeMessages$: Subscription;
  private usersConnected$: Subscription;
  private usersDisconnected$: Subscription;

  @Output() notificationsBadgeEvent = new EventEmitter();

  constructor(private signalr: SignalrService, private toastr: ReactiveToasterService)
  {
    this.registerSubscriptions();
  }

  set notifications(data: NotificationDto<BaseEvent>) {
    this.numOfItemsToShow = this._notificationsData.push(data);

    this.notificationsToShow = this._notificationsData.slice(0, this.numOfItemsToShow);

    this.isFullListDisplayed = false;
    this.emitChangeNumberOfNotifications();
  }

  registerSubscriptions(){

    this.productsAddedEvents$ = this.signalr.productAdded$.asObservable().subscribe(
      (productAdded) => {
        this.productsAddedEvents.push(productAdded);
        this.notifications = productAdded;
        this.toastr.showToast('', 'New product has been added', 'success');
      });

    this.productsPropertiesChangedEvents$ = this.signalr.productPropertiesChanged$.asObservable().subscribe(
      (productPropertiesChanged) => {
        this.productsPropertiesChangedEvents.push(productPropertiesChanged);
        this.notifications = productPropertiesChanged;
        this.toastr.showToast('', 'Product has been changed', 'success');
      });

    this.productsPublishedEvents$ = this.signalr.productPublished$.asObservable().subscribe(
      (productPublished) => {
        this.productsPublishedEvents.push(productPublished);
        this.notifications = productPublished;
        this.toastr.showToast('', 'Product has been published', 'success');

      });

    this.welcomeMessages$ = this.signalr.welcomeMessage$.asObservable().subscribe(
      (welcomeMessage) => {
        this.notifications = welcomeMessage;
        this.toastr.showToast('', 'Welcome to Ubiquitous!', 'success');
      });

    this.usersConnected$ = this.signalr.usersConnected$.asObservable().subscribe(
      (notification) => {
        this.notifications = notification;
        this.toastr.showToast('', notification.event.nickname + ' has joined!', 'info');
        this.usersLogged.push(notification.event.nickname);
      });

    this.usersDisconnected$ = this.signalr.usersDisconnected$.asObservable().subscribe(
      (notification) => {
        this.notifications = notification;
        this.toastr.showToast('', notification.event.nickname + ' has left!', 'info');
        this.usersLogged = this.usersLogged.filter(x=>x != notification.event.nickname);
      });

  }

  emitChangeNumberOfNotifications()
  {
    this.notificationsBadgeEvent.emit(this._notificationsData.filter(value => value.state === ConfirmationType.unread).length);
  }
  countNotifications() {
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
