import { Component, EventEmitter, Output, inject, Input, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExtraItem } from '../models';
import { MenuStore } from '../store/menu.store';
import { SessionStore } from '../store/session.store';

@Component({
  selector: 'app-top-menu',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './top-menu.component.html',
  styleUrl: './top-menu.component.scss',
  providers: [MenuStore, SessionStore]
})
export class TopMenuComponent {
  @Input() title: string = 'My App';
  @Input('extra-items') extraItems: string | ExtraItem[] = '';
  @Output() actionClick = new EventEmitter<any>();

  parsedExtraItems: ExtraItem[] = [];

  menuStore = inject(MenuStore);
  sessionStore = inject(SessionStore);

  constructor(private elementRef: ElementRef) {
    this.menuStore.loadMenu();
  }

  ngOnChanges(): void {
    if (this.extraItems) {
      if (typeof this.extraItems === 'string') {
        try {
          this.parsedExtraItems = JSON.parse(this.extraItems);
        } catch {
        }
      } else if (Array.isArray(this.extraItems)) {
        this.parsedExtraItems = this.extraItems;
      }
    }
  }

  onButtonClick() {
    this.elementRef.nativeElement.dispatchEvent(
      new CustomEvent('actionClick', {
        detail: { source: 'uix-top-menu' },
        bubbles: true,
        composed: true
      })
    );
  }

  logout() {
    this.sessionStore.logout();
  }
}