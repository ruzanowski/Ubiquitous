import {NgModule, ModuleWithProviders} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {RouterModule} from '@angular/router';
import {HTTP_INTERCEPTORS, HttpClientJsonpModule, HttpClientModule} from '@angular/common/http';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

// Services
import {DataService} from './services/data.service';
import {ProgressSpinnerComponent} from "./components/spinner-overlay/progress-spinner.component";
import {OverlayService} from "./services/overlay.service";
import {
  MatBadgeModule,
  MatButtonModule, MatCardModule, MatFormFieldModule, MatGridListModule,
  MatIconModule, MatInputModule, MatMenuModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatToolbarModule
} from "@angular/material";
import {LoaderService} from "./services/loader.service";
import {LoaderComponent} from "./components/loader/loader.component";
import {LoaderInterceptor} from "../../loader.interceptor";
import {FlexLayoutModule, FlexModule} from "@angular/flex-layout";
import {BrowserModule} from "@angular/platform-browser";
import {NavbarComponent} from "./components/navbar/navbar.component";
import {SidebarComponent} from "./components/sidebar/sidebar.component";
import {FooterComponent} from "./components/footer/footer.component";
import {LoginComponent} from "../login/components/login/login.component";
import {AppRoutingModule} from "../../app-routing.module";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    AppRoutingModule,
    BrowserModule,
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
    MatBadgeModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule
  ],
  declarations:
    [
      NavbarComponent,
      SidebarComponent,
      ProgressSpinnerComponent,
      LoaderComponent,
      LoginComponent,
      FooterComponent
    ],
  exports:
    [
      CommonModule,
      FormsModule,
      ReactiveFormsModule,
      RouterModule,
      NgbModule,
      //providers, components
      NavbarComponent,
      SidebarComponent,
      ProgressSpinnerComponent,
      LoaderComponent,
      LoginComponent,
      FooterComponent
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
        {provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true}
      ]
    };
  }
}
