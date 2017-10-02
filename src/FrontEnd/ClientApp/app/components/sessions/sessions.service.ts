import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { Session } from './session';


@Injectable()
export class SessionsService {

  private headers = new Headers({ 'Content-Type': 'application/json' });
  private serviceUrl = '/api/sessions';

  /**
   * init with Http
   */
  constructor(private http: Http) { }

  getSessions(): Promise<Session[]> {

    return this.http.get(this.serviceUrl)
      .toPromise()
      .then(response => response.json().data as Session[])
      .catch(this.handleError);
  }

  private handleError(error: any): Promise<any> {
    console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }
}

