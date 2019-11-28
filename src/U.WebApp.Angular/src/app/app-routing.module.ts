import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {AuthGuard} from "./modules/auth";
import {LoginComponent} from "./modules/login/components/login/login.component";
import {DashboardComponent} from "./modules/dashboard/components/dashboard/dashboard.component";
import {ProductsComponent} from "./modules/products/components/products.component";
import {ProductsDetailsComponent} from "./modules/products/components/products-details/products-details.component";
import {CategoryComponent} from "./modules/categories/components/table-category/table-categories.component";
import {ManufacturersComponent} from "./modules/manufacturers/components/table-manufacturers/table-manufacturers.component";
import {HomeLayoutComponent} from "./layouts/home-layout/home-layout.component";
import {LoginLayoutComponent} from "./layouts/login-layout/login-layout.component";

const routes: Routes = [
  {
    path: '',
    component: HomeLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        component: DashboardComponent
      },
      {
        path: 'products',
        component: ProductsComponent
      },
      {
        path: 'products/:id',
        component: ProductsDetailsComponent
      },
      {
        path: 'categories',
        component: CategoryComponent
      },
      {
        path: 'manufacturers',
        component: ManufacturersComponent
      }
    ]
  },
  {
    path: '',
    component: LoginLayoutComponent,
    children: [
      {
        path: 'login',
        component: LoginComponent
      }
    ]
  },
  {path: '**', redirectTo: ''}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
