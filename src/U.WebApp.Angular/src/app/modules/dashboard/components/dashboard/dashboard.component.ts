import {Component, OnInit} from '@angular/core';
import {catchError} from "rxjs/operators";
import {ProductService} from "../../../products/product.service";
import {throwError} from "rxjs";
import {ManufacturerService} from "../../../manufacturers/manufacturers.service";
import {CategoryService} from "../../../categories/category.service";
import {NotificationService} from "../../../notifications/services/notification.service";
import {IdentityService} from "../../../shared/services/identity.service";
import {NotificationHttpService} from "../../../notifications/services/notification-http.service";

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  public productCardCount: number = 0;
  public manufacturerCardCount: number = 0;
  public categoryCardCount: number = 0;
  public notificationsCardCount: number = 0;
  public usersOnlineCardCount: number = 0;
  public usersOverallCardCount: number = 0;
  created = new Date();

  errorReceived: boolean;

  constructor(private readonly productService: ProductService,
              private readonly manufacturerService: ManufacturerService,
              private readonly categoryService: CategoryService,
              private readonly notificationService: NotificationService,
              private readonly identityService: IdentityService,
              private readonly notificationHttpService: NotificationHttpService) {

  }

  ngOnInit() {
    this.getProductCount();
    this.getManufacturerCount();
    this.getCategoryCount();
    this.getNotificationsCount();
    this.getOnlineUsersCount();
    this.getAllUsersCount();
  }

  getProductCount(): void {
    this.productService.getProductCount()
      .pipe(catchError((err) => this.handleError(err)))
      .subscribe(count => {
        this.productCardCount = count;
      });
  }

  getManufacturerCount(): void {
    this.manufacturerService.getManufacturerCount()
      .pipe(catchError((err) => this.handleError(err)))
      .subscribe(count => {
        this.manufacturerCardCount = count;
      });
  }

  getCategoryCount(): void {
    this.categoryService.getCategoryCount()
      .pipe(catchError((err) => this.handleError(err)))
      .subscribe(count => {
        this.categoryCardCount = count;
      });
  }

  getNotificationsCount(): void {
    this.notificationHttpService.getNotificationsProcessedCount()
      .pipe(catchError((err) => this.handleError(err)))
      .subscribe(count => {
        this.notificationsCardCount = count;
      });
  }

  getOnlineUsersCount(): void {
    this.identityService.getUsersOnline()
      .pipe(catchError((err) => this.handleError(err)))
      .subscribe(count => {
        this.usersOnlineCardCount = count;
      });
  }

  getAllUsersCount(): void {
    this.identityService.getAllUsers()
      .pipe(catchError((err) => this.handleError(err)))
      .subscribe(count => {
        this.usersOverallCardCount = count.length;
      });
  }

  private handleError(error: any) {
    this.errorReceived = true;
    return throwError(error);
  }

}
