import { Component, OnInit } from '@angular/core';
import { Message } from '../models/message';
import { InboxService } from '../services/inbox.service';
import * as fileSaver from 'file-saver';

@Component({
  selector: 'app-list-messages',
  templateUrl: './list-messages.component.html',
  styleUrls: ['./list-messages.component.less'],
})
export class ListMessagesComponent implements OnInit {
  messages: Message[];
  specialLink: string;

  constructor(private inboxService: InboxService) {}

  ngOnInit(): void {
    this.inboxService.fetchedMessages.subscribe((d) => {
      this.messages = d;
    });
  }

  createLink(fileBytes) {
    fileBytes = atob(fileBytes);
    const file = new Blob([fileBytes], { type: 'text/plain' });
    fileSaver.saveAs(file, 'file.txt');
    this.specialLink = encodeURIComponent(atob(fileBytes));
  }

  changeDate(date) {
    const dateObject = new Date(date);
    return dateObject.setHours(dateObject.getHours() + 2);
  }
}
