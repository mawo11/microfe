import { Component, computed, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuStore } from '../store/menuStore';
import { SessionStore } from '../store/SessionStore';

@Component({
  selector: 'app-top-menu',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './top-menu.component.html',
  styleUrl: './top-menu.component.scss'
})
export class TopMenuComponent {
  @Input() title: string = 'Top Menux';

  menuStore = inject(MenuStore);
  sessionStore = inject(SessionStore);

  constructor() {
    this.menuStore.loadMenu();
  }

  get menuItems() {
    return this.menuStore.getMenuItems();
  }  
}