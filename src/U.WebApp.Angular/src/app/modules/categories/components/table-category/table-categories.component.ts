import {Component, OnInit, ViewChild} from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {CategoryService} from "../../category.service";
import {Category} from "../../models/category.model";

@Component({
  selector: 'table-categories',
  templateUrl: './table-categories.component.html',
  styleUrls: ['./table-categories.component.css']
})

export class CategoryComponent implements OnInit {
  errorReceived: boolean;
  dataSource = new MatTableDataSource<Category>();
  displayedColumns: string[] = ['categoryId', 'name', 'description', 'categoryParentId'];

  constructor(private service: CategoryService) {
  }

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  ngOnInit(): void {
    this.getCategories();
  }

  getCategories(): void {
    this.errorReceived = false;

    this.service.getCategories()
      .subscribe(categories => {
        this.dataSource = new MatTableDataSource<Category>(categories.data);
        this.dataSource.paginator = this.paginator;
        this.dataSource.paginator.pageIndex = 1;
        console.log('categories retrieved: ' + categories.data);
      });
  }
}





