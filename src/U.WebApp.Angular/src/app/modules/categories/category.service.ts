import {Injectable} from '@angular/core';
import {DataService} from "../shared/services/data.service";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {Category} from "./models/category.model";
import {PaginatedItems} from "../shared/components/models/paginateditems.model";

@Injectable()
export class CategoryService {

  private categoryBaseUrl = '/api/product/categories';
  private pageSizeQuery = '?PageSize=99999';

  constructor(private service: DataService) {
  }

  getCategories(): Observable<PaginatedItems<Category>> {
    let url = this.categoryBaseUrl + '/query' + this.pageSizeQuery;

    return this.service.get(url).pipe(map((response: any) => response));
  }

  getCategory(id: string): Observable<Category> {
    let url = this.categoryBaseUrl + '/query/' + id;

    return this.service.get(url).pipe(map((response: any) => response));
  }
}
