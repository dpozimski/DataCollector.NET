import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

/**
 * Implementation of the CanActivate to check if local storage has jwt tokne
 * required to execute api methods.
 */
@Injectable()
export class EnsureAuthenticatedService implements CanActivate {
  constructor(private router: Router) {}

  /**
   * Checks if the request can be proceeded by
   * checking for jwt token in the local storage.
   */
  canActivate(): boolean {
    if (localStorage.getItem('token')) {
      return true;
    } else {
      this.router.navigateByUrl('/login');
      return false;
    }
  }
}

