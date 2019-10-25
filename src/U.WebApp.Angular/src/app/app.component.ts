import {Component, OnDestroy, OnInit} from '@angular/core';
import {LoaderService} from "./modules/shared/services/loader.service";
import {SignalrService} from "./modules/shared/services/signalr.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit, OnDestroy {

  public content: any;


  constructor(private loaderService: LoaderService, private signalrService: SignalrService) {

    this.signalrService.subscribeOnEvents();
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
    this.signalrService.disconnect();
  }
}

