import { Component } from '@angular/core';

import { ChartType } from 'ng-chartist';

import * as Chartist from 'chartist';
import { Observable, of } from 'rxjs';
import {IBarChartOptions, IChartistData} from "chartist";
import {NotificationHttpService} from "../../../../notifications/services/notification-http.service";
import {IntegrationEventType} from "../../../../notifications/models/integration-event-type.model";
import ChartistTooltip from 'chartist-plugin-tooltips-updated';

@Component({
  selector: 'types-chart',
  template: `<x-chartist [data]="data$ | async" [type]="type$ " [options]="options"> </x-chartist>`
})
export class AsyncChartComponent {
  data$: Observable<IChartistData>;
  type$: ChartType = 'Bar';
  labels: string[] = [];
  series: number[] = [];

  options: IBarChartOptions = {

    height: 250,
    plugins: [
      ChartistTooltip({
        anchorToPoint: true,
        appendToBody: true
      })
    ],
    distributeSeries: true
  };


  constructor(private readonly service: NotificationHttpService) {

    service.getNotificationEventTypeStatisticsCount()
      .subscribe(value => {
        for (let o of value) {
          this.labels.push(IntegrationEventType[o.eventType].slice(0, IntegrationEventType[o.eventType].indexOf('IntegrationEvent')));
          this.series.push(o.count);
        }

        let schemedData: Chartist.IChartistData = <IChartistData>
          {
            labels: this.labels,
            series: this.series,
          };

        this.data$ = of(schemedData);
      });
  }
}
