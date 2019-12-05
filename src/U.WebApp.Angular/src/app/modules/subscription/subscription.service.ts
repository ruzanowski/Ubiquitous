import {Injectable} from '@angular/core';
import {DataService} from "../shared/services/data.service";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {AllowedEvents} from "./models/allowed-events.model";
import {Preferences} from "./models/preferences.model";
import {IntegrationEventType} from "../notifications/models/integration-event-type.model";

@Injectable()
export class SubscriptionService {

  private productBaseUrl = '/api/subscription';

  constructor(private service: DataService)
  {
  }

  getMyPreferences(): Observable<Preferences> {

    let url = this.productBaseUrl + '/preferences/me';

    return this.service.get(url).pipe(map((response: any) => response));
  }

  setMyPreferences(preferences: Preferences)
  {

    let url = this.productBaseUrl + '/preferences/me';

    return this.service.post(url, preferences).pipe(map((response: any) =>
    {
      console.log(response);
      return response;
    }));
  }

  getMyAllowedEvents(): Observable<IntegrationEventType[]> {
    let url = this.productBaseUrl + '/preferences/me/allowed-events';

    return this.service.get(url).pipe(map((response: any) => response));
  }

  setMyAllowedEvents(events: IntegrationEventType[])
  {

    let url = this.productBaseUrl + '/preferences/me/allowed-events';

    return this.service.post(url, events).pipe(map((response: any) =>
    {
      console.log(response);
      return response;
    }));
  }
}
