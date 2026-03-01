import { createApplication } from '@angular/platform-browser';
import { createCustomElement } from '@angular/elements';
import { TopMenuComponent } from './app/top-menu/top-menu.component';
import { FooterComponent } from './app/footer/footer.component';
import { AppLayoutComponent } from './app/layout/app.layout.component';
import { initGlobalHttpInterceptor } from './globalhttphandler';

(async () => {
  const app = await createApplication({
    providers: []
  });

  const TopMenuEl = createCustomElement(TopMenuComponent, {
    injector: app.injector
  });
  if (!customElements.get('uix-top-menu')) {
    customElements.define('uix-top-menu', TopMenuEl);
  }

  const FooterEl = createCustomElement(FooterComponent, {
    injector: app.injector
  });
  if (!customElements.get('uix-footer')) {
    customElements.define('uix-footer', FooterEl);
  }

  const appLayout = createCustomElement(AppLayoutComponent, {
    injector: app.injector
  })

  if (!customElements.get('uix-app-layout')) {
    customElements.define('uix-app-layout', appLayout);
  }

  console.log("initGlobalHttpInterceptor");
  initGlobalHttpInterceptor();

  console.log('%c[uix-library]%c Web Components gotowe: <uix-app-layout>','color:#5ac8ff;font-weight:bold', 'color:inherit');
})();