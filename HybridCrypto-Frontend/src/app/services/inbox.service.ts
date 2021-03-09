import { Injectable } from '@angular/core';
import { Message } from '../models/message';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { ListUsersService } from './list-users.service';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class InboxService {
  private getMessagesURL = environment.apiURL + 'Messages/';

  // this will hold the current value of the message.
  private messages = new BehaviorSubject<Message[]>(undefined);
  // Then we define a current message variable set to an observable that will be used by the components
  fetchedMessages = this.messages.asObservable();
  // Next we create a function that calls next on the behaviorsubject to change its current value
  setMessage(message: Message[]) {
    this.messages.next(message);
  }

  constructor(
    private http: HttpClient,
    private listUsersService: ListUsersService
  ) {}

  getMessages(senderId) {
    this.http
      .get<Message[]>(this.getMessagesURL + senderId)
      .subscribe((data) => {
        this.listUsersService.setChanged(false);
        this.setMessage(data);
      });
  }
}
