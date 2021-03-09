import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SendService {
  private postMessageURL = environment.apiURL + 'Messages/';

  constructor(private http: HttpClient) {}

  send(data) {
    return this.http.post(this.postMessageURL, data);
  }
}
