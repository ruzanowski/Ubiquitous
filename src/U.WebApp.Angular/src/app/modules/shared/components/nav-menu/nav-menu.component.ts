import {Component, ElementRef, EventEmitter, Input, Output, ViewChild} from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  public isSideNavMenuExpanded = false;
  public isNotificationNavBarToggled: boolean = false;

  public primary_color_primary: string = '#00695c';
  public primary_color_light: string  ='#439889';
  public primary_color_read: string = '#ffffff';

  @Input()
  public notificationNavBadgeCounter: number = 2;
  public notificationNavColor: string = this.primary_color_light;
  public notificationNavColor_text: string = this.primary_color_read;

  public sideNavColor: string = this.primary_color_light;
  public sideNavcolor_text: string = this.primary_color_read;

  @Output() navBarToggle = new EventEmitter();
  @Output() notificationBarToggle = new EventEmitter<boolean>();

  toggleSideNavMenu() {
    this.isSideNavMenuExpanded = !this.isSideNavMenuExpanded;
    this.navBarToggle.emit();
    this.sideBarActive();
  }

  toggleNotificationMenu() {
    this.isNotificationNavBarToggled = !this.isNotificationNavBarToggled;
    this.notificationBarToggle.emit(this.isNotificationNavBarToggled);

    if(this.isNotificationNavBarToggled){
      this.notificationNavColor = this.primary_color_light;
    }
    else {
      this.notificationNavColor = this.primary_color_primary;
    }
  }
  sideBarActive() {
    if(this.isSideNavMenuExpanded){
      this.sideNavColor = this.primary_color_light;
    }
    else {
      this.sideNavColor = this.primary_color_primary;
    }
  }
}
