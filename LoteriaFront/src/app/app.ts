import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  template: `
    <app-header />
    <main class="app-main">
      <div class="container-lg py-4 py-md-5">
        <router-outlet />
      </div>
    </main>
  `
})
export class AppComponent {
  title = 'LoteriaFront';
}
