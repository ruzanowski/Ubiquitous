import {Component, ElementRef, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Router} from "@angular/router";
import {ROUTES} from "../sidebar/sidebar.component";
import {Location} from '@angular/common';
import {Observable} from "rxjs";
import {AuthenticationService} from "../../../auth";

@Component({
  selector: 'nav-bar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent
implements OnInit
{
  public isNotificationNavBarToggled: boolean = false;

  public primary_color_primary: string = '#00695c';
  public primary_color_light: string = '#439889';
  public primary_color_read: string = '#ffffff';

  @Input()
  public notificationNavBadgeCounter: number = 2;
  public notificationNavColor: string = this.primary_color_light;
  public notificationNavColor_text: string = this.primary_color_read;

  public sideNavColor: string = this.primary_color_light;
  public sideNavcolor_text: string = this.primary_color_read;

  @Output() notificationBarToggle = new EventEmitter<boolean>();

  private listTitles: any[];
  location: Location;
  mobile_menu_visible: any = 0;
  private toggleButton: any;
  private sidebarVisible: boolean;
  isLoggedIn$: Observable<boolean>;
  currentUser: string;


  toggleNotificationMenu() {
    this.isNotificationNavBarToggled = !this.isNotificationNavBarToggled;
    this.notificationBarToggle.emit(this.isNotificationNavBarToggled);

    if (this.isNotificationNavBarToggled) {
      this.notificationNavColor = this.primary_color_light;
    } else {
      this.notificationNavColor = this.primary_color_primary;
    }
  }

  constructor(location: Location, private element: ElementRef, private router: Router, private authService: AuthenticationService) {
    this.location = location;
    this.sidebarVisible = false;
    this.isLoggedIn$ = this.authService.isLoggedIn;
    this.authService.currentUser.subscribe(x=>
      {
       this.currentUser = x.claims["nickname"]
      });
  }

  ngOnInit() {
    this.listTitles = ROUTES.filter(listTitle => listTitle);
    const navbar: HTMLElement = this.element.nativeElement;
    this.toggleButton = navbar.getElementsByClassName('navbar-toggler')[0];
    this.router.events.subscribe((event) => {
      this.sidebarClose();
      var $layer: any = document.getElementsByClassName('close-layer')[0];
      if ($layer) {
        $layer.remove();
        this.mobile_menu_visible = 0;
      }
    });
  }

  sidebarOpen() {
    const toggleButton = this.toggleButton;
    const body = document.getElementsByTagName('body')[0];
    setTimeout(function () {
      toggleButton.classList.add('toggled');
    }, 500);

    body.classList.add('nav-open');

    this.sidebarVisible = true;
  };

  sidebarClose() {
    const body = document.getElementsByTagName('body')[0];
    this.toggleButton.classList.remove('toggled');
    this.sidebarVisible = false;
    body.classList.remove('nav-open');
  };

  sidebarToggle() {
    // const toggleButton = this.toggleButton;
    // const body = document.getElementsByTagName('body')[0];
    var $toggle = document.getElementsByClassName('navbar-toggler')[0];

    if (this.sidebarVisible === false) {
      this.sidebarOpen();
    } else {
      this.sidebarClose();
    }
    const body = document.getElementsByTagName('body')[0];

    if (this.mobile_menu_visible == 1) {
      // $('html').removeClass('nav-open');
      body.classList.remove('nav-open');
      if ($layer) {
        $layer.remove();
      }
      setTimeout(function () {
        $toggle.classList.remove('toggled');
      }, 400);

      this.mobile_menu_visible = 0;
    } else {
      setTimeout(function () {
        $toggle.classList.add('toggled');
      }, 430);

      var $layer = document.createElement('div');
      $layer.setAttribute('class', 'close-layer');


      if (body.querySelectorAll('.main-panel')) {
        document.getElementsByClassName('main-panel')[0].appendChild($layer);
      } else if (body.classList.contains('off-canvas-sidebar')) {
        document.getElementsByClassName('wrapper-full-page')[0].appendChild($layer);
      }

      setTimeout(function () {
        $layer.classList.add('visible');
      }, 100);

      $layer.onclick = function () { //asign a function
        body.classList.remove('nav-open');
        this.mobile_menu_visible = 0;
        $layer.classList.remove('visible');
        setTimeout(function () {
          $layer.remove();
          $toggle.classList.remove('toggled');
        }, 400);
      }.bind(this);

      body.classList.add('nav-open');
      this.mobile_menu_visible = 1;

    }
  };

  getTitle() {
    var titlee = this.location.prepareExternalUrl(this.location.path());
    if (titlee.charAt(0) === '#') {
      titlee = titlee.slice(1);
    }

    for (var item = 0; item < this.listTitles.length; item++) {
      if (this.listTitles[item].path === titlee) {
        return this.listTitles[item].title;
      }
    }
    return 'Dashboard';
  }

  logout(){
    this.authService.logout();
  }
}
