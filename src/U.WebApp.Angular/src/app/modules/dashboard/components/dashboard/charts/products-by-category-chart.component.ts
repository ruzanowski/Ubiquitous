import { Component } from '@angular/core';

import { ChartType } from 'ng-chartist';

import * as Chartist from 'chartist';
import { Observable, of } from 'rxjs';
import {IBarChartOptions, IChartistData} from "chartist";
// import ChartistTooltip from 'chartist-plugin-tooltips-updated';
import {ProductService} from "../../../../products/product.service";

@Component({
  selector: 'product-by-category-chart',
  template: `<x-chartist [data]="data$ | async" [type]="type$ " [options]="options"> </x-chartist>`
})
export class ProductsByCategoryChartComponent {
  data$: Observable<IChartistData>;
  type$: ChartType = 'Bar';
  labels: string[] = [];
  series: number[] = [];


  options: IBarChartOptions = {

    height: 250,
    // plugins: [
    //   ChartistTooltip({
    //     anchorToPoint: true,
    //     appendToBody: true
    //   })
    // ],
    distributeSeries: true
  };


  constructor(private readonly service: ProductService) {

    service.getProductStatisticsByCategory()
      .subscribe(value => {
        for (let o of value) {
          this.labels.push(o.categoryName);
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
