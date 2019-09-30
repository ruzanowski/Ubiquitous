import {Component, OnInit} from '@angular/core';
import {Product} from "../models/product.model";
import {PaginatedItems} from "../../shared/models/paginateditems.model";
import {catchError} from "rxjs/operators";
import {ProductService} from "../product.service";
import {throwError} from "rxjs";

@Component({
  selector: 'app-product',
  templateUrl: './products.component.html'
})
export class ProductsComponent implements OnInit {
  private products: PaginatedItems<Product>;
  errorReceived: boolean;

  constructor(private service: ProductService) {
  }

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts(): void {
    this.errorReceived = false;
    this.service.getProducts()
      .pipe(catchError((err) => this.handleError(err)))
      .subscribe(products => {
        this.products = products;
        console.log('products retrieved: ' + products.data.length);
      });
  }

  private handleError(error: any) {
    this.errorReceived = true;
    return throwError(error);
  }
}





