import {NgModule, ModuleWithProviders} from '@angular/core';
import {CommonModule, ViewportScroller} from '@angular/common';
import {FormsModule, ReactiveFormsModule, FormBuilder} from '@angular/forms';
import {RouterModule} from '@angular/router';
import {HTTP_INTERCEPTORS, HttpClientJsonpModule, HttpClientModule} from '@angular/common/http';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

// Services
import {DataService} from './services/data.service';
import {NavMenuComponent} from "./components/nav-menu/nav-menu.component";
import {ProgressSpinnerComponent} from "./components/spinner-overlay/progress-spinner.component";
import {OverlayService} from "./services/overlay.service";
import {
  MatButtonModule, MatGridListModule,
  MatIconModule, MatMenuModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatToolbarModule
} from "@angular/material";
import {LoaderService} from "./services/loader.service";
import {LoaderComponent} from "./components/loader/loader.component";
import {LoaderInterceptor} from "../../loader.interceptor";
import {SignalrService} from "./services/signalr.service";
import {FlexLayoutModule, FlexModule} from "@angular/flex-layout";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    NgbModule,
    // No need to export as these modules don't expose any components/directive etc'
    HttpClientModule,
    HttpClientJsonpModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    FlexModule,
    FlexLayoutModule,
    MatGridListModule,
  ],
  declarations: [NavMenuComponent, ProgressSpinnerComponent, LoaderComponent],
  exports: [
    // Modules
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    NgbModule,
    //providers, components
    NavMenuComponent,
    ProgressSpinnerComponent,
    LoaderComponent,
  ]
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
      providers: [
        DataService,
        OverlayService,
        LoaderService,
        // ToastrService,
        { provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true },
        SignalrService,
        NavMenuComponent
      ]
    };
  }
}
