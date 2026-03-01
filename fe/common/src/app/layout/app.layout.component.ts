import { Component, inject, Input, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SessionStore } from '../store/session.store';
import { FooterComponent } from '../footer/footer.component';
import { TopMenuComponent } from '../top-menu/top-menu.component';

@Component({
    selector: 'app-layout',
    standalone: true,
    imports: [CommonModule, TopMenuComponent, FooterComponent],
    templateUrl: './app.layout.component.html',
    styleUrl: './app.layout.component.scss',
    encapsulation: ViewEncapsulation.ShadowDom,
    providers: [SessionStore]
})
export class AppLayoutComponent {
    @Input() menuTitle: string = '';
    @Input() menuItems: string = '';


    sessionStore = inject(SessionStore);

    constructor() { }
}