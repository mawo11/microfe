import { createApplication } from '@angular/platform-browser';
import { createCustomElement } from '@angular/elements';
import { TopMenuComponent } from './app/top-menu/top-menu.component';
import { FooterComponent } from './app/footer/footer.component';

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

  console.log('%c[wc-library]%c Web Components gotowe: <top-menu>, <wc-footer>','color:#5ac8ff;font-weight:bold', 'color:inherit');
})();