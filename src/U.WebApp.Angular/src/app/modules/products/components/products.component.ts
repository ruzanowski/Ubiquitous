import {Component, OnInit, ViewChild} from '@angular/core';
import {Product} from "../models/product.model";
import {ProductService} from "../product.service";
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {Category} from "../../categories/models/category.model";
import {CategoryService} from "../../categories/category.service";
import {Manufacturer} from "../../manufacturers/models/Manufacturer.model";
import {ManufacturerService} from "../../manufacturers/manufacturers.service";
import {PaginatedItems} from "../../shared/components/models/paginateditems.model";

@Component({
  selector: 'table-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})

export class ProductsComponent implements OnInit {
  dataSource = new MatTableDataSource<Product>();
  displayedColumns: string[] = ['name', 'price', 'description', 'height', 'width', 'length', 'weight', 'lastUpdated'];
  categories: PaginatedItems<Category>;
  manufacturers: PaginatedItems<Manufacturer>;
  public manufacturerIdFilter?: string;
  public categoryIdFilter?: string;

  constructor(private service: ProductService, private categoryService: CategoryService, private manufacturerService: ManufacturerService) {
this.categoryIdFilter = null;
this.manufacturerIdFilter = null;
  }

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  ngOnInit(): void {
    this.getProducts();
    this.getCategories();
    this.getManufacturers();
  }

  getProducts(): void {
    this.service.getProducts(this.categoryIdFilter, this.manufacturerIdFilter)
      .subscribe(products => {
        this.dataSource = new MatTableDataSource<Product>(products.data);
        this.dataSource.paginator = this.paginator;
        this.dataSource.paginator.pageIndex = 1;
        console.log('products retrieved: ' + products.data.length);
      });
  }

  getCategories(): void {
    this.categoryService.getCategories().subscribe(categories => {
      this.categories = categories;
      console.log('categories retrieved: ' + categories.data.length);
    });
  }

  getManufacturers(): void {
    this.manufacturerService.getManufacturers().subscribe(manufacturers => {
      this.manufacturers = manufacturers;
      console.log('manufacturers retrieved: ' + manufacturers.data.length);
    })
  }

  setManufacturerFilter(id: string){
   this.manufacturerIdFilter = id;

    console.log('selected manufacturer filter: ' + id);
  }

  setCategoryFilter(id: string){
    this.categoryIdFilter = id;
  }
}





