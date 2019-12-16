import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders, HttpErrorResponse} from "@angular/common/http";

import {Observable, throwError} from 'rxjs';
import {retry, map, catchError} from 'rxjs/operators';

// Implementing a Retry-Circuit breaker policy
// is pending to do for the SPA app
@Injectable()
export class DataService {
  constructor(private http: HttpClient) {
  }

  get(url: string, params?: any): Observable<Response> {
    let options = {};



    return this.http.get(url, options)
      .pipe(
        // retry(3), // retry a failed request up to 3 times
        map((res: Response) => {
          return res;
        }),
        catchError(DataService.handleError)
      );
  }

  postWithId(url: string, data: any, params?: any): Observable<Response> {
    return this.doPost(url, data,  params);
  }

  post(url: string, data: any, params?: any): Observable<Response> {
    return this.doPost(url, data,  params);
  }

  putWithId(url: string, data: any, params?: any): Observable<Response> {
    return this.doPut(url, data,  params);
  }

  private doPost(url: string, data: any, params?: any): Observable<Response> {
    let options = {};

    return this.http.post(url, data, options)
      .pipe(
        map((res: Response) => {
          return res;
        }),
        catchError(DataService.handleError)
      );
  }

  delete(url: string, params?: any) {
    let options = {};

    console.log('notificationNavBarToggled.service deleting');

    this.http.delete(url, options)
      .subscribe((res) => {
        console.log('deleted');
      });
  }

  public static handleError(error: any) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('Client side network error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error('Backend - ' +
        `status: ${error.status}, ` +
        `statusText: ${error.statusText}, ` +
        `message: ${error.error.message}`);
    }

    // return an observable with a user-facing error message
    return throwError(error || 'server error');
  }

  private doPut(url: string, data: any, params?: any): Observable<Response> {
    let options = {};
    return this.http.put(url, data, options)
      .pipe(
        map((res: Response) => {
          return res;
        }),
        catchError(DataService.handleError)
      );
  }
}
