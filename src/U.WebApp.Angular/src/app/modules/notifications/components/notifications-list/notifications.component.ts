import {Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild} from '@angular/core';
import {MatDrawer} from "@angular/material";
import {SignalrService} from "../../services/signalr.service";
import {NotificationDto} from "../../models/notification.model";
import {ProductAddedEvent} from "../../models/product-added-event.model";
import {ProductPublishedEvent} from "../../models/product-published-event.model";
import {ProductPropertiesChangedEvent} from "../../models/product-properties-changed-event.model";
import {Subscription} from "rxjs";

@Component({
  selector: 'notifications-drawer',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit, OnDestroy {
  @Input()
  public isNotificationNavBarToggled: boolean = false;

  private notificationsData: Array<NotificationDto<object>> = [];
  public notificationsToShow: Array<NotificationDto<object>> = [];

  private productsAddedEventsSubscription: Subscription;
  private productsPropertiesChangedEventsSubscription: Subscription;
  private productsPublishedEventsSubscription: Subscription;
  private welcomeMessagesSubscription: Subscription;

  public productsAddedEvents: Array<NotificationDto<ProductAddedEvent>> = [];
  public productsPropertiesChangedEvents: Array<NotificationDto<ProductPropertiesChangedEvent>> = [];
  public productsPublishedEvents: Array<NotificationDto<ProductPublishedEvent>> = [];
  public welcomeMessagesEvents: Array<NotificationDto<any>> = [];

  private numOfItemsToShow = 12;
  private itemsToLoad = 15;

  isFullListDisplayed = false;

  constructor(private signalr: SignalrService) {
    this.signalr.subscribeOnEvents();
  }

  ngOnInit(): void {
    this.signalr.connect();
    this.registerSubscriptions();
  }

  @Input()
  set notifications(data: Array<NotificationDto<object>>) {
    this.notificationsToShow = data.slice(0, this.numOfItemsToShow);
    this.notificationsData = data;
    this.isFullListDisplayed = false;

    console.log('notificationsToShow updated: '+ JSON.stringify(this.notificationsToShow));
  }

  private registerSubscriptions(){

    this.productsAddedEventsSubscription = this.signalr.getProductAdded().subscribe(
      (productAdded) => {
        console.log('productsAddedEventsSubscription');
        this.productsAddedEvents.push(productAdded);
        this.notificationsData.push(productAdded);
        this.notifications = this.notificationsData;
      });

    this.productsPropertiesChangedEventsSubscription = this.signalr.getProductPropertiesChanged().subscribe(
      (productPropertiesChanged) => {
        console.log('productsPropertiesChangedEventsSubscription');

        this.productsPropertiesChangedEvents.push(productPropertiesChanged);
        this.notificationsData.push(productPropertiesChanged);
        this.notifications = this.notificationsData;
      });

    this.productsPublishedEventsSubscription = this.signalr.getProductPublished().subscribe(
      (productPublished) => {
        console.log('productsPublishedEventsSubscription');

        this.productsPublishedEvents.push(productPublished);
        this.notificationsData.push(productPublished);
        this.notifications = this.notificationsData;
      });

    this.welcomeMessagesSubscription = this.signalr.getWelcomeMessages().subscribe(
      (productPublished) => {
        console.log('welcomeMessagesSubscription');

        this.welcomeMessagesEvents.push(productPublished);
        this.notificationsData.push(productPublished);
        this.notifications = this.notificationsData;
      });
  }

  onScroll() {
    if (this.numOfItemsToShow <= this.notificationsData.length) {
      this.numOfItemsToShow += this.itemsToLoad;
      this.notificationsToShow = this.notificationsData.slice(0, this.numOfItemsToShow);
    } else {
      this.isFullListDisplayed = true;
    }
  }

  anyNotifications() {
    return this.notificationsData.length;
  }

  ngOnDestroy(): void {
    this.signalr.disconnect();
  }
}
