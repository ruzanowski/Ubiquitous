import {Injectable} from '@angular/core';
import {DataService} from "../shared/services/data.service";
import {Observable} from "rxjs";
import {PaginatedItems} from "../shared/models/paginateditems.model";
import {Product} from "./models/product.model";
import {map} from "rxjs/operators";

@Injectable()
export class ProductService {

  private productBaseUrl = '/api/product/products';

  constructor(private service: DataService) {
  }

  getProducts(): Observable<PaginatedItems<Product>> {
    let url = this.productBaseUrl + '/query';

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
