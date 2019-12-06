import {
  IBarChartOptions,
  IChartistAnimationOptions,
  IChartistData
} from 'chartist';
import { ChartEvent, ChartType } from 'ng-chartist';
import {Component} from "@angular/core";
import * as Chartist from "chartist";

@Component({
  selector: 'app-bar-chart',
  template: `
    <x-chartist
      [type]="type"
      [data]="data"
      [options]="options"
      [events]="events"
    ></x-chartist>
  `
})
export class NotificationsBarChartComponent {
  type: ChartType = 'Line';
  data: IChartistData = {
    labels: [
      'Mon',
      'Tue',
      'Wed',
      'Thu',
      'Fri',
      'Sat',
      'Sun',
    ],
    series: [
      [35780, 50310, 45702, 77043, 51012, 31430, 75301]
    ]
  };

  options: IBarChartOptions = {
    axisX: {
      showGrid: false
    },
    height: 250,
    plugins:
      [
      ]
  };

  events: ChartEvent = {
    draw: (data) => {
        data.element.animate({
          y2: <IChartistAnimationOptions>{
            dur: '0.8s',
            from: data.y1,
            to: data.y2,
            easing: 'easeOutQuad',
          }
        });
    }
  };
}
