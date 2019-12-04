import {Injectable} from '@angular/core';
import {DataService} from "../shared/services/data.service";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {AllowedEvents} from "./models/allowed-events.model";
import {Preferences} from "./models/preferences.model";

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

  getMyAllowedEvents(id: string): Observable<AllowedEvents> {
    let url = this.productBaseUrl + '/preferences/allowed-events';

    return this.service.get(url).pipe(map((response: any) => response));
  }
}
