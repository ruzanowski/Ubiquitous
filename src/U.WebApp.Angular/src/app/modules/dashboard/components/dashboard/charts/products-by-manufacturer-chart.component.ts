import { Component } from '@angular/core';

import { ChartType } from 'ng-chartist';

import * as Chartist from 'chartist';
import { Observable, of } from 'rxjs';
import {IBarChartOptions, IChartistData} from "chartist";
import {NotificationHttpService} from "../../../../notifications/services/notification-http.service";
import {IntegrationEventType} from "../../../../notifications/models/integration-event-type.model";
import ChartistTooltip from 'chartist-plugin-tooltips-updated';
import {ProductService} from "../../../../products/product.service";

@Component({
  selector: 'product-by-manufacturer-chart',
  template: `<x-chartist [data]="data$ | async" [type]="type$ " [options]="options"> </x-chartist>`
})
export class ProductsByManufacturerChartComponent {
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


  constructor(private readonly service: ProductService) {

    service.getProductStatisticsByManufacturer()
      .subscribe(value => {
        for (let o of value) {
          this.labels.push(o.manufacturerName);
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
