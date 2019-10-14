import {Component, OnInit, ViewChild} from '@angular/core';
import {Product} from "../models/product.model";
import {PaginatedItems} from "../../shared/models/paginateditems.model";
import {catchError, map} from "rxjs/operators";
import {ProductService} from "../product.service";
import {throwError} from "rxjs";
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';

@Component({
  selector: 'app-product',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})

export class ProductsComponent implements OnInit {
  errorReceived: boolean;
  dataSource = new MatTableDataSource<Product>();
  displayedColumns: string[] = ['name', 'price', 'description', 'height', 'width', 'length', 'weight', 'lastUpdated'];

  constructor(private service: ProductService) {
  }


  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  ngOnInit(): void {
    this.getProducts();

  }

  getProducts(): void {
    this.errorReceived = false;

    this.service.getProducts()
    .subscribe(products => {
      this.dataSource = new MatTableDataSource<Product>(products.data);
      this.dataSource.paginator = this.paginator;
      console.log('products retrieved: ' + products.data.length);
    });
  }
}





