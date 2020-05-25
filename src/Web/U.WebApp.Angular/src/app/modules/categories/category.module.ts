import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {SharedModule} from '../shared/shared.module';
import {MatPaginatorModule, MatTableModule} from "@angular/material";
import {CategoryService} from "./category.service";
import {CategoryComponent} from "./components/table-category/table-categories.component";

@NgModule({
  imports: [BrowserModule, SharedModule, MatTableModule, MatPaginatorModule],
  declarations: [CategoryComponent],
  exports: [
    CategoryComponent
  ],
  providers: [CategoryService, CategoryComponent]
})
export class CategoryModule {

}
