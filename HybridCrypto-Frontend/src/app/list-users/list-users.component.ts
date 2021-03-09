import { Component, OnInit } from '@angular/core';
import { User } from '../models/user';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { ListUsersService } from '../services/list-users.service';

@Component({
  selector: 'app-list-users',
  templateUrl: './list-users.component.html',
  styleUrls: ['./list-users.component.less'],
})
export class ListUsersComponent implements OnInit {
  users: User[];

  private getUsersURL = environment.apiURL + 'Users/';

  constructor(
    private http: HttpClient,
    private listUsersService: ListUsersService
  ) {}

  ngOnInit(): void {
    this.http.get<User[]>(this.getUsersURL).subscribe((data) => {
      this.users = data;
      if (this.users[0] !== undefined) {
        this.listUsersService.setSelectedValue(this.users[0].id);
      }
      this.listUsersService.setLoaded(true);
    });
  }

  public selectUser(userId): void {
    this.listUsersService.setSelectedValue(userId);
  }
}
