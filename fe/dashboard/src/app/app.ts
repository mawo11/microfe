import { Component, ComponentRef, ViewChild, ViewContainerRef, effect, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { loadRemoteModule } from '@angular-architects/native-federation';
import { SessionStore } from './store/session';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  sessionStorage = inject(SessionStore);
  protected readonly title = signal('dashboard');

  @ViewChild('menu', { read: ViewContainerRef })
  menu!: ViewContainerRef;

  menuComponent!: ComponentRef<any>;
  constructor() {
    this.sessionStorage.checkSession();

    effect(async () => {
      if (!this.sessionStorage.isCheckingState()) {

        if (!this.sessionStorage.isLoggedIn()) {
          const url = this.sessionStorage.getStartPageUrl();
          console.log('Redirecting to:', url);
          document.location.href = "/sso"
          return;
        }

        const menu = await loadRemoteModule({
          remoteName: 'common',
          exposedModule: './TopMenu'
        });

        this.menuComponent = this.menu.createComponent(menu.TopMenuComponent);

        this.menuComponent.setInput("title", "Dashboardx");
      }
    });
  }


  async sayHelLo() {
    const result = await fetch('/api/hello')
      .then(res => res.text())
    console.log('result', result);
    this.menuComponent.setInput("title", result);
  }
}
