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
    getErrorMessage: () => state.error()
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


      logout() {
        // store.stopPolling();

        http.post('/api/auth/logout', {}).subscribe({
          next: (res) => { document.location.href = "/sso"  },
          error: () => {document.location.href = "/sso"  }
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
