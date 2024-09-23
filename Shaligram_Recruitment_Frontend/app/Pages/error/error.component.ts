import { Component } from '@angular/core';

@Component({
  selector: 'app-error',
  standalone: true,
  imports: [],
  template: `
  <div class="error-page">
    <h1>Error: Page Not Found</h1>
    <p>The page you're looking for doesn't exist.</p>
    <a routerLink="/admin">Go Back to Dashboard</a>
  </div>
`,
styles: [`
  .error-page {
    text-align: center;
    padding: 50px;
  }
`]
})
export class ErrorComponent {

}
