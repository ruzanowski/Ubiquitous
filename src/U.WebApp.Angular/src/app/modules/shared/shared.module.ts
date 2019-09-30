import {NgModule, ModuleWithProviders} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule, ReactiveFormsModule, FormBuilder} from '@angular/forms';
import {RouterModule} from '@angular/router';
import {HttpClientJsonpModule, HttpClientModule} from '@angular/common/http';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

// Services
import {DataService} from './services/data.service';
import {NavMenuComponent} from "./components/nav-menu/nav-menu.component";


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    NgbModule,
    // No need to export as these modules don't expose any components/directive etc'
    HttpClientModule,
    HttpClientJsonpModule
  ],
  declarations: [NavMenuComponent],
  exports: [
    // Modules
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    NgbModule,
    //providers, components
    NavMenuComponent
  ]
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
      providers: [
        DataService
      ]
    };
  }
}
