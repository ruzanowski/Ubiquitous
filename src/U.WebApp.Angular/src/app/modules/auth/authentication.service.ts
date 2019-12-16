/**
 * Based on
 * https://github.com/cornflourblue/angular-7-jwt-authentication-example
 */

import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import {catchError, map} from 'rxjs/operators';
import {Router} from "@angular/router";
import {state} from "@angular/animations";
import {DataService} from "../shared/services/data.service";

export interface ApplicationUser {
  accessToken: string;
  refreshToken: string;
  userId: string;
  userRole: string;
  expires: Date;
  claims: Map<string,string>;
}

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private currentUserSubject: BehaviorSubject<ApplicationUser>;
  public currentUser: Observable<ApplicationUser>;
  private loggedIn = new BehaviorSubject<boolean>(false);

  constructor(private readonly http: HttpClient,
              private router: Router,
              private readonly dataService: DataService) {
    this.currentUserSubject = new BehaviorSubject<ApplicationUser>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): ApplicationUser {
    return this.currentUserSubject.value;
  }

  get isLoggedIn()
  {
    return this.loggedIn.asObservable();
  }

  login(email: string, password: string) {
    return this.http.post<any>('/api/identity/auth/sign-in', { email, password })
      .pipe(map(user => {
        // login successful if there's a jwt accessToken in the response
        if (user && user.accessToken) {
          // store user details and jwt accessToken in local storage to keep user logged in between page refreshes
          localStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUserSubject.next(user);
          this.loggedIn.next(true);
        }

        return user;
      }));
  }

  logout()
  {
    this.revokeRefreshToken();

    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    this.loggedIn.next(false);
    this.router.navigate(['/login']);
  }

  private revokeRefreshToken(){
    return this.dataService.post('/api/identity/token/access/revoke', {})
      .pipe(map((response: any) =>
    {
      console.log(response);
      return response;
    }));
  }

  check()
  {
    const currentUser = this.currentUserValue;
    if(currentUser)
    {
      return;
    }

    this.logout();
  }
}
