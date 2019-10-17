import {AfterViewInit, Component, OnInit} from '@angular/core';
import {LoaderService} from "./modules/shared/services/loader.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],

})
export class AppComponent implements OnInit {
  tiles: Tile[] = [
    {text: 'One', cols: 4, rows: 1, color: 'lightblue'},
    {text: 'Two', cols: 1, rows: 2, color: 'lightgreen'},
    {text: 'Three', cols: 1, rows: 1, color: 'lightpink'},
    {text: 'Four', cols: 2, rows: 1, color: '#DDBDF1'},
  ];

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

export interface Tile {
  color: string;
  cols: number;
  rows: number;
  text: string;
}

