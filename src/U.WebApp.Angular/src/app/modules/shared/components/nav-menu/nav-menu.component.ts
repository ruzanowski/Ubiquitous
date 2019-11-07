import {Component, ElementRef, EventEmitter, Output, ViewChild} from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  public isSideNavMenuExpanded = false;
  public isNotificationNavBarToggled: boolean = false;
  private colorDefault: string = 'primary';
  private colorClicked: string = 'warn';

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
    this.notificationsSideBarActive()
  }

  @ViewChild('NotificationsSideNavButton', {static: false}) notificationsSideNavButton: ElementRef;

  notificationsSideBarActive() {
    if ((<any>this.notificationsSideNavButton).color === this.colorDefault) {
      (<any>this.notificationsSideNavButton).color = this.colorClicked
    } else {
      (<any>this.notificationsSideNavButton).color = this.colorDefault;
    }
  }

  @ViewChild('sideNavButton', {static: false}) sideNavButton: ElementRef;

  sideBarActive() {
    if ((<any>this.sideNavButton).color === this.colorDefault) {
      (<any>this.sideNavButton).color = this.colorClicked
    } else {
      (<any>this.sideNavButton).color = this.colorDefault;
    }
  }
}
