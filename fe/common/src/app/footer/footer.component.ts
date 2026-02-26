import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExtraItem } from '../models';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.scss'
})
export class FooterComponent {
  @Input() brand: string = 'My AppX';
  @Input() tagline: string = 'Built with Angular Elements';
  @Input() links: string | ExtraItem[] = '';

  parsedLinks: ExtraItem[] = [];
  
  
  get year(): number {
    return new Date().getFullYear();
  }

  ngOnChanges(): void {
    if (this.links) {
      if (typeof this.links === 'string' && this.links.trim()) {
        try {
          this.parsedLinks = JSON.parse(this.links);
        } catch {
          // zostają domyślne
        }
      } else if (Array.isArray(this.links)) {
        this.parsedLinks = this.links;
      }
    }
  }
}
