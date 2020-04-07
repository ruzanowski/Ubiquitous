import {Injectable} from '@angular/core';
import {DataService} from "../shared/services/data.service";
import {Observable} from "rxjs";
import {Product, ProductStatistics} from "./models/product.model";
import {map} from "rxjs/operators";
import {PaginatedItems} from "../shared/models/paginateditems.model";
import {ProductStatisticsByCategory, ProductStatisticsByManufacturer} from "./models/product-statistics.model";

@Injectable()
export class ProductService {

  private productBaseUrl = '/api/product/products';
  private pageSizeQuery = '?PageSize=99999';

  constructor(private service: DataService) {
  }


  getProducts(categoryId?: string, manufacturerId?: string): Observable<PaginatedItems<Product>> {

    let url = this.productBaseUrl + this.pageSizeQuery;

    if(categoryId != null)
    {
      let categoryQuery = this.categoryFilterQuery(categoryId);
      url = url.concat(categoryQuery);
    }

    if(manufacturerId != null)
    {
      let manufacturerQuery = this.manufacturerFilterQuery(manufacturerId);
      url = url.concat(manufacturerQuery);
    }

    return this.service.get(url).pipe(map((response: any) => response));
  }

  getProduct(id: string): Observable<Product> {
    let url = this.productBaseUrl + '/' + id;

    return this.service.get(url).pipe(map((response: any) => response));
  }

  manufacturerFilterQuery(id: string) : string
  {
    return "&ManufacturerId=" + id;
  }

  categoryFilterQuery(id: string) : string
  {
    return "&CategoryId=" + id;
  }

  getProductCount(): Observable<number> {
    let url = this.productBaseUrl + '/count';

    return this.service.get(url).pipe(map((response: any) => response));
  }

  getProductStatistics(): Observable<Array<ProductStatistics>> {
    let url = this.productBaseUrl + '/statistics/creation';

    return this.service.get(url).pipe(map((response: any) => response));
  }
  getProductStatisticsByManufacturer(): Observable<Array<ProductStatisticsByManufacturer>> {
    let url = this.productBaseUrl + '/statistics/manufacturer';

    return this.service.get(url).pipe(map((response: any) => response));
  }
  getProductStatisticsByCategory(): Observable<Array<ProductStatisticsByCategory>> {
    let url = this.productBaseUrl + '/statistics/category';

    return this.service.get(url).pipe(map((response: any) => response));
  }
}
