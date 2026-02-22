import { interval, Subscription } from 'rxjs';
import { inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  signalStore,
  withState,
  withComputed,
  withMethods,
  patchState
} from '@ngrx/signals';
import { UserLoginResponse } from './models';

interface AuthState {
  loading: boolean;
  error: string | null;
  loggedIn: boolean;
  startPageUrl: string | null;
  checkingState: boolean;
}

const initialState: AuthState = {
  loading: false,
  error: null,
  loggedIn: false,
  startPageUrl: null,
  checkingState: false
};


export const SessionStore = signalStore(
  { providedIn: 'root' },

  withState(initialState),

  withComputed((state) => ({
    isLoggedIn: () => state.loggedIn(),
    isLoading: () => state.loading(),
    getStartPageUrl: () => state.startPageUrl(),
    isCheckingState: () => state.checkingState(),
  })),


  withMethods((store: any) => {
    const http = inject(HttpClient);

    let pollingSub: Subscription | null = null;

    return {

      // startPolling() {
      //   if (pollingSub) return;

      //   pollingSub = interval(30_000).subscribe(() => {
      //     // if (store.token()) {
      //     //   store.loadMe();
      //     // }
      //   });
      // },

      // stopPolling() {
      //   pollingSub?.unsubscribe();
      //   pollingSub = null;
      // },


     
      checkSession() {
        patchState(store, { checkingState: true });
        http.get<UserLoginResponse>('/api/auth/check')
          .subscribe(
            {
              next: (res) => {
                patchState(store, {
                  checkingState: false,
                  error: res.success ? null : 'Login failed',
                  loggedIn: res.success,
                  startPageUrl: res.urlToRedirect
                });
              },
              error: () => patchState(store, {
                checkingState: false,
                error: 'Failed to check session'
              })
            }
          );
      },
      logout() {
        // store.stopPolling();

        http.post('/api/auth/logout', {}).subscribe(() => {
        });

        patchState(store, {
          user: null
        });
      },

      loadMe() {
        http.get<any>('/api/me')
          .subscribe(user => patchState(store, { user }));
      }
    };
  })
);
