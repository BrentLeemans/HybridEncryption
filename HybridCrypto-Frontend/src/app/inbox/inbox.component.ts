import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ListUsersService } from '../services/list-users.service';
import { InboxService } from '../services/inbox.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-inbox',
  templateUrl: './inbox.component.html',
  styleUrls: ['./inbox.component.less'],
})
export class InboxComponent implements OnInit {
  private changed: Observable<boolean>;

  constructor(
    private http: HttpClient,
    private listUsersService: ListUsersService,
    private inboxService: InboxService
  ) {
    this.changed = this.listUsersService.getIsChanged();
  }

  ngOnInit(): void {
    // the document loads with the first user selected.
    this.getMessages();
  }

  getMessages(): void {
    let count = 0;
    this.changed.subscribe((changed) => {
      if (count === 0 && changed === true) {
        count++;
        this.inboxService.getMessages(this.listUsersService.selectedValue);
      }
    });
  }
}
