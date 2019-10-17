import {Injectable} from '@angular/core';
import {DataService} from "../shared/services/data.service";
import {Observable} from "rxjs";
import {PaginatedItems} from "../shared/models/paginateditems.model";
import {Product} from "./models/product.model";
import {map} from "rxjs/operators";
import {$} from "protractor";

@Injectable()
export class ProductService {

  private productBaseUrl = '/api/product/products';
  private pageSizeQuery = '?PageSize=99999';

  constructor(private service: DataService) {
  }

  getProducts(): Observable<PaginatedItems<Product>> {
    let url = this.productBaseUrl + '/query' + this.pageSizeQuery;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  getProduct(id: string): Observable<Product> {
    let url = this.productBaseUrl + '/query/' + id;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }
}
