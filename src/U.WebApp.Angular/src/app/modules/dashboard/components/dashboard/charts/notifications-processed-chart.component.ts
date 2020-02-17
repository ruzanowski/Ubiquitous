import {
  IChartistAnimationOptions,
  IChartistData, ILineChartOptions
} from 'chartist';
import {ChartEvent, ChartType} from 'ng-chartist';
import {Component} from "@angular/core";
// import ChartistTooltip from 'chartist-plugin-tooltips-updated';
import {Observable, of} from "rxjs";
import {NotificationHttpService} from "../../../../notifications/services/notification-http.service";
import * as moment from "moment";

@Component({
  selector: 'processed-chart',
  template: `<x-chartist [data]="data$ | async" [type]="type " [options]="options" [events]="events"> </x-chartist>`,
  styles: [`.ct-series-a .ct-line {
    /* Set the colour of this series line */
    stroke: #0d47a1;
    /* Control the thikness of your lines */
    stroke-width: 5px;
    fill: #0d47a1;
  }`]
})
export class NotificationsBarChartComponent {

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

  constructor(private readonly service: NotificationHttpService) {


    service.getNotificationStatisticsCount()
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
