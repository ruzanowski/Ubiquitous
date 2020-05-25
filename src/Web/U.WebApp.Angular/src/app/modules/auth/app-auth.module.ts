import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {JwtInterceptor} from "./jwt.interceptor";
import {ErrorInterceptor} from "./error.interceptor";
import {HTTP_INTERCEPTORS} from "@angular/common/http";


@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ]
})
export class AppAuthModule { }
