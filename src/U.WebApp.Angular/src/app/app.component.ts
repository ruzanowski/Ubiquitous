import {AfterViewInit, Component, OnInit} from '@angular/core';
import {LoaderService} from "./modules/shared/services/loader.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],

})
export class AppComponent implements OnInit {
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
}

