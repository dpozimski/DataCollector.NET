import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TokenResponse } from '../models/token-response';
import { MessageService } from '../services/message.service';
import 'rxjs/add/operator/toPromise';

/**
 * The service which handle the current logged user
 * with login methods using api.
 */
@Injectable()
export class LoggedUserService {
  private authenticationTokenUri = 'authentication/RequestToken';
  private headers: HttpHeaders = new HttpHeaders({'Content-Type': 'application/json'});

  /**
   * The reference to the current logged user.
   * Null if the user is not logged.
   */
  public currentUser: User;

  constructor(private http: HttpClient,
              private messageService: MessageService) { }

  /**
   * Tries to verify if these credentials are correct.
   * @param user is the object which holds the login data
   */
  login(user: User): Promise<Boolean> {
    return this.http.post(this.authenticationTokenUri, user, { headers: this.headers })
      .toPromise().then(s => {
        const tokenResponse = s as TokenResponse;
        localStorage.setItem('token', tokenResponse.token);
        return true;
      }).catch(error => {
        this.messageService.log(error);
        return false;
      });
  }
}

