import {Component, OnInit} from '@angular/core';

declare const $: any;

declare interface RouteInfo {
  path: string;
  title: string;
  icon: string;
  class: string;
}

export const ROUTES: RouteInfo[] = [
  {path: '/dashboard', title: 'Dashboard', icon: 'dashboard', class: ''},
  {path: '/products', title: 'Products', icon: 'redeem', class: ''},
  {path: '/manufacturers', title: 'Manufacturers', icon: 'supervisor_account', class: ''},
  {path: '/categories', title: 'Categories', icon: 'category', class: ''},
  {path: '/notifications', title: 'Notifications', icon: 'notifications', class: ''},
  {path: '/subscription', title: 'Subscription', icon: 'person', class: ''},
];

@Component({
  selector: 'side-bar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  menuItems: any[];

  constructor() {
  }

  ngOnInit() {
    this.menuItems = ROUTES.filter(menuItem => menuItem);
  }

  isMobileMenu() {
    return $(window).width() <= 991;
  };
}
