import { inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  signalStore,
  withState,
  withComputed,
  withMethods,
  patchState
} from '@ngrx/signals';
import { DiagItem } from './models';

interface AuthState {
  items: DiagItem[];
  loading?: boolean;
}

const initialState: AuthState = {
  items: [],
  loading: false
};


export const DashboardStore = signalStore(
  { providedIn: 'root' },

  withState(initialState),

  withComputed((state) => ({
    getDiagItems: () => state.items()
  })),


  withMethods((store: any) => {
    const http = inject(HttpClient);

    return {
      loadDiagItems() {
        console.log('fetching diag items');
        patchState(store, { loading: true });

        http.get<DiagItem[]>('/api/dashboard/diagnostic')
          .subscribe(
            {
              next: (res) => {
                console.log('fetched diag items', res);
                patchState(store, {
                  loading: false,
                  items: res
                })
              },
              error: () => patchState(store, {
                loading: false,
              })
            }
          )
      },
    };
  })
);
