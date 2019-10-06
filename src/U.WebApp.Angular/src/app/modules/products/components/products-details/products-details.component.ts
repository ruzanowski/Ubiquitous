import {Component, OnInit} from '@angular/core';
import {Product} from "../../models/product.model";
import {PaginatedItems} from "../../../shared/models/paginateditems.model";
import {catchError} from "rxjs/operators";
import {ProductService} from "../../product.service";
import {throwError} from "rxjs";
import {stringify} from "@angular/compiler/src/util";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-product',
  templateUrl: './products-details.component.html'
})
export class ProductsDetailsComponent implements OnInit {
  private product: Product;
  errorReceived: boolean;

  constructor(private service: ProductService, private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
       let id = params['id'];
      this.getProduct(id);
    });
  }

  getProduct(id: string): void {
    this.errorReceived = false;
    this.service.getProduct(id)
      .pipe(catchError((err) => this.handleError(err)))
      .subscribe(product => {
        this.product = product;
        console.log('products retrieved: ' + stringify(product));
      });
  }

  private handleError(error: any) {
    this.errorReceived = true;
    return throwError(error);
  }
}





