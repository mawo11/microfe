import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // Automatycznie wysyłaj cookies
    request = request.clone({
      withCredentials: true
    });

    return next.handle(request).pipe(
      tap((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          // Obsługuj cookies z response'u
          const setCookie = event.headers.get('Set-Cookie');
          if (setCookie) {
            console.log('Cookie ustawione:', setCookie);
          }
        }
      })
    );
  }
}
