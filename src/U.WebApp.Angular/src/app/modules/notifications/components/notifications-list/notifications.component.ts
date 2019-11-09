import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {SignalrService} from "../../services/signalr.service";
import {ProductAddedEvent} from "../../models/product-added-event.model";
import {ProductPublishedEvent} from "../../models/product-published-event.model";
import {ProductPropertiesChangedEvent} from "../../models/product-properties-changed-event.model";
import {Subscription} from "rxjs";
import {ProductBaseEvent} from "../../models/product-base-event.model";
import {NotificationDto} from "../../models/notificationdto.model";

@Component({
  selector: 'notifications-drawer',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit, OnDestroy {
  @Input()
  private numOfItemsToShow = 12;
  private itemsToLoad = 15;
  private isFullListDisplayed = false;

  private _notificationsData: Array<NotificationDto<ProductBaseEvent>> = [];
  private productsAddedEventsSubscription: Subscription;
  private productsPropertiesChangedEventsSubscription: Subscription;
  private productsPublishedEventsSubscription: Subscription;
  private welcomeMessagesSubscription: Subscription;

  public notificationsToShow: Array<NotificationDto<ProductBaseEvent>> = [];

  public productsAddedEvents: Array<NotificationDto<ProductAddedEvent>> = [];
  public productsPropertiesChangedEvents: Array<NotificationDto<ProductPropertiesChangedEvent>> = [];
  public productsPublishedEvents: Array<NotificationDto<ProductPublishedEvent>> = [];

  constructor(private signalr: SignalrService) {
    this.signalr.subscribeOnEvents();
  }

  ngOnInit(): void {
    this.signalr.connect();
    this.registerSubscriptions();
  }

  set notifications(data: NotificationDto<ProductBaseEvent>) {
    this._notificationsData.push(data);
    this.notificationsToShow = this._notificationsData.slice(0, this.numOfItemsToShow);
    this.isFullListDisplayed = false;
  }

  private registerSubscriptions(){

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

  onScroll() {
    if (this.numOfItemsToShow <= this._notificationsData.length) {
      this.numOfItemsToShow += this.itemsToLoad;
      this.notificationsToShow = this._notificationsData.slice(0, this.numOfItemsToShow);
    } else {
      this.isFullListDisplayed = true;
    }
  }

  anyNotifications() {
    return this._notificationsData.length;
  }

  ngOnDestroy(): void {
    this.signalr.disconnect();
  }
}
