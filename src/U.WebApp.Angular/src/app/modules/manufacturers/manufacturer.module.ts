import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {SharedModule} from '../shared/shared.module';
import {MatPaginatorModule, MatTableModule} from "@angular/material";
import {ManufacturerService} from "./manufacturers.service";
import {ManufacturersComponent} from "./components/table-manufacturers/table-manufacturers..component";

@NgModule({
  imports: [BrowserModule, SharedModule, MatTableModule, MatPaginatorModule],
  declarations: [ManufacturersComponent],
  exports: [
    ManufacturersComponent
  ],
  providers: [ManufacturerService, ManufacturersComponent]
})
export class ManufacturerModule {

}
