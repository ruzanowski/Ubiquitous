import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent} from '@angular/common/http';
import { AuthenticationService } from './authentication.service';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root'
})
export class JwtInterceptor implements HttpInterceptor {
	constructor(private authenticationService: AuthenticationService) { }

	intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		// add authorization header with jwt accessToken if available
		let currentUser = this.authenticationService.currentUserValue;
		if (currentUser && currentUser.accessToken) {
			request = request.clone({
				setHeaders: {
					Authorization: `Bearer ${currentUser.accessToken}`
				}
			});
		}

		return next.handle(request);
	}
}
