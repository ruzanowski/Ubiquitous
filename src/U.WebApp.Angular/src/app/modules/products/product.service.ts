import {Injectable} from '@angular/core';
import {DataService} from "../shared/services/data.service";
import {Observable} from "rxjs";
import {Product} from "./models/product.model";
import {map} from "rxjs/operators";
import {PaginatedItems} from "../shared/components/models/paginateditems.model";

@Injectable()
export class ProductService {

  private productBaseUrl = '/api/product/products';
  private pageSizeQuery = '?PageSize=99999';

  constructor(private service: DataService) {
  }


  getProducts(categoryId?: string, manufacturerId?: string): Observable<PaginatedItems<Product>> {

    let url = this.productBaseUrl + '/query' + this.pageSizeQuery;

    if(categoryId != null)
    {
      let categoryQuery = ProductService.categoryFilterQuery(categoryId);
      url = url.concat(categoryQuery);
    }

    if(manufacturerId != null)
    {
      let manufacturerQuery = ProductService.manufacturerFilterQuery(manufacturerId);
      url = url.concat(manufacturerQuery);
    }

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

  private static manufacturerFilterQuery(id: string) : string
  {
    return "&ManufacturerId=" + id;
  }

  private static categoryFilterQuery(id: string) : string
  {
    return "&CategoryId=" + id;
  }
}
