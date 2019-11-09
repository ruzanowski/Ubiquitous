import {
  Component, OnDestroy,
  OnInit} from '@angular/core';
import {LoaderService} from "./modules/shared/services/loader.service";

@Component({
  selector: 'ubiquitous-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit, OnDestroy {
  public isNotificationNavBarToggled: any = true;

  constructor(private loaderService: LoaderService) {
  }

  ngOnInit() {
    /** spinner starts on init */
    this.loaderService.show();
    setTimeout(() => {
      /** spinner ends after 5 seconds */
      this.loaderService.hide();
    }, 5000);
  }

  ngOnDestroy(): void {
  }

  receiveNotificationBarToggle(event) {
    this.isNotificationNavBarToggled = event;
    console.log('navbar notificationBarToggle event received: ' + this.isNotificationNavBarToggled);
  }


}

