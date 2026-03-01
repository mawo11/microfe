import { Component, CUSTOM_ELEMENTS_SCHEMA, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class App {
   appTitle = 'Dashboard';
   navItemsJson = JSON.stringify([
    { label: 'lx',  href: '#x' }
  ]);

    handleAction(event: any) {
      alert('dashboard action clicked:' + JSON.stringify(event.detail)  );
    }
}
