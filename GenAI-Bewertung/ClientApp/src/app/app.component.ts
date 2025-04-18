import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  constructor(private translate: TranslateService) {
    const savedLang = localStorage.getItem('lang');
    translate.addLangs(['en', 'de']);
    translate.setDefaultLang('de');
    translate.use(savedLang || 'de');
  }
}
