import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ListUsersService {
  private selected: string;
  private isLoaded = new BehaviorSubject(false);
  private isChanged = new BehaviorSubject(false);

  constructor() {}

  public get selectedValue() {
    return this.selected;
  }

  public setLoaded(value): void {
    this.isLoaded.next(value);
  }

  public setChanged(value): void {
    this.isChanged.next(value);
  }

  public setSelectedValue(value: string) {
    this.selected = value;
    this.setChanged(true);
  }

  public getIsLoaded(): Observable<boolean> {
    return this.isLoaded.asObservable();
  }

  public getIsChanged(): Observable<boolean> {
    return this.isChanged.asObservable();
  }
}
