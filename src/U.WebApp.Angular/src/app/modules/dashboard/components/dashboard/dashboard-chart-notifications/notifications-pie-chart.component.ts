import { Component } from '@angular/core';

import { ChartType } from 'ng-chartist';

import * as Chartist from 'chartist';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';
import {getRandomInt} from "./notifications-live-chart.component";
import {IBarChartOptions, IPieChartOptions} from "chartist";

declare var require: any;

const data: any = require('./data.json');

@Component({
  selector: 'async-chart',
  template: `
    <x-chartist [data]="data$ | async" [type]="type$ | async" [options]="options"> </x-chartist>
  `
})
export class AsyncChartComponent {
  data$: Observable<Chartist.IChartistData>;
  type$: Observable<ChartType>;

  options: IPieChartOptions = {
    height: 250,
    width: 320
  };

  constructor() {
    // simulate slow API call
    this.data$ = of(data.Pie).pipe(delay(getRandomInt(500, 1000)));
    this.type$ = of(<ChartType>'Pie').pipe(delay(getRandomInt(500, 1000)));
  }
}
