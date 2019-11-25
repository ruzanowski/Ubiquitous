import { Component} from '@angular/core';
import {slideInAnimation} from "../../animations";

@Component({
  selector: 'app-login',
  template: `
    <app-loader></app-loader>
    <router-outlet></router-outlet>
  `,
  styles: [],
  animations: [slideInAnimation]
})
export class LoginLayoutComponent {}
