import {Injectable} from '@angular/core';
import {DataService} from "../../shared/services/data.service";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {NotificationCreationStatistics, NotificationEventTypeStatistics} from "../models/notification-statistics.model";

@Injectable({
  providedIn: 'root'
})
export class NotificationHttpService
{
  private notificationBaseUrl = '/api/notification/notifications';

  constructor(private httpService: DataService)
  {
  }

  getNotificationsProcessedCount(): Observable<number> {
    let url = this.notificationBaseUrl + '/count';

    return this.httpService.get(url).pipe(map((response: any) => response));
  }

  getNotificationStatisticsCount(): Observable<Array<NotificationCreationStatistics>> {
    let url = this.notificationBaseUrl + '/statistics/creation' + '?stepFrequency=Daily';

    return this.httpService.get(url).pipe(map((response: any) => response));
  }

  getNotificationEventTypeStatisticsCount(): Observable<Array<NotificationEventTypeStatistics>> {
    let url = this.notificationBaseUrl + '/statistics/event-types';

    return this.httpService.get(url).pipe(map((response: any) => response));
  }


}
