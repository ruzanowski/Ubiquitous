import {
  IChartistAnimationOptions,
  IChartistData, ILineChartOptions
} from 'chartist';
import {ChartEvent, ChartType} from 'ng-chartist';
import {Component} from "@angular/core";
// import ChartistTooltip from 'chartist-plugin-tooltips-updated';
import {Observable, of} from "rxjs";
import * as moment from "moment";
import {ProductService} from "../../../../products/product.service";

@Component({
  selector: 'products-chart',
  template: `<x-chartist [data]="data$ | async" [type]="type " [options]="options" [events]="events"> </x-chartist>`
})
export class ProductLineChartComponent
{
  data$: Observable<IChartistData>;
  labels: Date[] = [];
  series: string[] = [];
  type: ChartType = 'Line';
  options: ILineChartOptions = {
    axisX: {
      showGrid: true,
      labelInterpolationFnc: function(value) {
        return moment(value).format('MMM D');
      },
    },
    axisY: {
      showGrid: true,
    },
    height: 250,
    // plugins: [
    //   ChartistTooltip({
    //     anchorToPoint: true,
    //     appendToBody: true
    //   })
    // ],
    showPoint: false,
    showArea: true,
  };
  events: ChartEvent = {
    draw: (data) => {
      data.element.animate({
        y2: <IChartistAnimationOptions>{
          dur: '1.5s',
          from: data.y1,
          to: data.y2,
          easing: 'easeOutQuad',

        }
      });
    }
  };

  constructor(private readonly service: ProductService) {


    service.getProductStatistics()
      .subscribe(value => {
        for (let o of value) {
          this.labels.push(o.dateTime);
          this.series.push(o.count.toString());
        }

        let schemedData: IChartistData = <IChartistData>
          {
            labels: this.labels,
            series: [this.series]
          };

        this.data$ = of(schemedData);
      });
  }


}
