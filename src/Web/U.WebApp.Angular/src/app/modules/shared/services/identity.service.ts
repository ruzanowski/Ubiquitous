import {Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {DataService} from "./data.service";
import {User} from "../models/user.model";

@Injectable()
export class IdentityService {

  private identityUrl = '/api/identity/users';

  constructor(private service: DataService) {
  }


  getUsersOnline(): Observable<number> {
    let url = this.identityUrl + '/online';

    return this.service.get(url).pipe(map((response: any) => response));
  }

  getAllUsers(): Observable<User[]> {
    let url = this.identityUrl + '/all';

    return this.service.get(url).pipe(map((response: any) => response));
  }

}
