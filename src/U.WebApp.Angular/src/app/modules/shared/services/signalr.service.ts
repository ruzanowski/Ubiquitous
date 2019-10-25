import {Injectable} from '@angular/core';
import * as signalR from "@aspnet/signalr";
import {LogLevel} from "@aspnet/signalr";
import {ProductAddedEvent} from "../models/product-added-event.model";

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  connection: signalR.HubConnection;
  public productsAdded: Array<ProductAddedEvent> = [];
  public productsPublished: Array<ProductAddedEvent> = [];
  public productsPropertiesChanged: Array<ProductAddedEvent> = [];
  public usersConnected: Array<string> = [];


  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5500/signalr')
      .configureLogging(LogLevel.Trace)
      .build();
    this.connect();
  }

  public connect() {
    if (this.connection.state === signalR.HubConnectionState.Disconnected) {
      this.connection
        .start()
        .catch(err => console.log(err));
    }
  }

  public subscribeOnEvents() {

    this.connection.on('connected', (user : string) => {
      console.log(JSON.stringify(user));
      this.usersConnected.push(user);
    });

    this.connection.on('disconnected', (user : string) => {
      console.log(JSON.stringify(user));
      this.usersConnected = this.usersConnected.filter(item => item !== user);
    });

    this.connection.on('ProductPublishedIntegrationEvent', (product) => {
      console.log(JSON.stringify(product));
      this.productsPublished.push(product);
    });

    this.connection.on('ProductPropertiesChangedIntegrationEvent', (product) => {
      console.log(JSON.stringify(product));
      this.productsPropertiesChanged.push(product);
    });

    this.connection.on('ProductAddedIntegrationEvent', (product) => {
      console.log(JSON.stringify(product));
      this.productsAdded.push(product);
    });
  }

  public disconnect() {
    this.connection.stop();
  }
}
