import {Injectable} from '@angular/core';
import {DataService} from "../shared/services/data.service";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {Manufacturer} from "./models/Manufacturer.model";
import {PaginatedItems} from "../shared/models/paginateditems.model";

@Injectable()
export class ManufacturerService {

  private manufacturerBaseUrl = '/api/product/manufacturers';
  private pageSizeQuery = '?PageSize=99999';

  constructor(private service: DataService) {
  }

  getManufacturers(): Observable<PaginatedItems<Manufacturer>> {
    let url = this.manufacturerBaseUrl + "/query" + this.pageSizeQuery;

    return this.service.get(url).pipe(map((response: any) => response));
  }

  getManufacturer(id: string): Observable<Manufacturer> {
    let url = this.manufacturerBaseUrl + "/query/" + id;

    return this.service.get(url).pipe(map((response: any) => response));
  }

  getManufacturerCount(): Observable<number> {
    let url = this.manufacturerBaseUrl + '/count';

    return this.service.get(url).pipe(map((response: any) => response));
  }
}
