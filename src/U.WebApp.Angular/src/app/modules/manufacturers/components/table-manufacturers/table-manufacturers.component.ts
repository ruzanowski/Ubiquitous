import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {Manufacturer} from "../../models/Manufacturer.model";
import {ManufacturerService} from "../../manufacturers.service";
import {Component, OnInit, ViewChild} from "@angular/core";

@Component({
  selector: 'table-manufacturers',
  templateUrl: './table-manufacturers.component.html',
  styleUrls: ['./table-manufacturers.component.css']
})

export class ManufacturersComponent implements OnInit {
  errorReceived: boolean;
  dataSource = new MatTableDataSource<Manufacturer>();
  displayedColumns: string[] = ['id', 'name', 'description', 'pictureId'];

  constructor(private service: ManufacturerService) {
  }

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  ngOnInit(): void {
    this.getManufacturers();
  }

  getManufacturers(): void {
    this.errorReceived = false;

    this.service.getManufacturers()
      .subscribe(categories => {
        this.dataSource = new MatTableDataSource<Manufacturer>(categories.data);
        this.dataSource.paginator = this.paginator;
        this.dataSource.paginator.pageIndex = 1;
        console.log('manufacturers retrieved: ' + categories.data);
      });
  }
}





