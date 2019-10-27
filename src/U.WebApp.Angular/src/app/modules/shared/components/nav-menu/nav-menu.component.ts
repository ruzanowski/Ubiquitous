import {Component, EventEmitter, Output} from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {
  public isExpanded = false;

  @Output() navBarToggle = new EventEmitter();

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
    this.navBarToggle.emit()
  }

}
