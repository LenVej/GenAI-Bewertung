import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

export interface CanExitComponent {
  canExit: () => boolean | Observable<boolean>;
}

@Injectable({
  providedIn: 'root'
})
export class ConfirmExitGuard implements CanDeactivate<CanExitComponent> {
  canDeactivate(
    component: CanExitComponent,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState?: RouterStateSnapshot
  ): boolean | Observable<boolean> {
    return component.canExit()
      ? true
      : confirm('❗ Willst du die Prüfung wirklich verlassen?');
  }
}

