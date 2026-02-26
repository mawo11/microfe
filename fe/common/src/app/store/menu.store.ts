import { inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  signalStore,
  withState,
  withComputed,
  withMethods,
  patchState
} from '@ngrx/signals';
import { MenuItem } from './models';
import { MenuResponse } from './models';

interface MenuState {
  loading: boolean;
  error: string | null;
  menuItems: MenuItem[];
}

const initialState: MenuState = {
  loading: false,
  error: null,
  menuItems: []
};

export const MenuStore = signalStore(
  { providedIn: 'root' },

  withState(initialState),

  withComputed((state) => ({
    getMenuItems: () => state.menuItems(),
  })),


  withMethods((store: any) => {
    const http = inject(HttpClient);
    return {

      loadMenu() {
        http.get<MenuResponse>('/config/apps')
          .subscribe(
            {
              next: (res) => {
                patchState(store, {
                  menuItems: res.items
                })
              },
              error: () => { }
            }
          );
      },

    };
  })
);