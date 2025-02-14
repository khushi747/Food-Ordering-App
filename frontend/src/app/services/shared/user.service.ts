import { Injectable } from '@angular/core';
import { promises } from 'dns';
import { BehaviorSubject } from 'rxjs';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private userIdSource = new BehaviorSubject<number | null>(null);
  private roleSource = new BehaviorSubject<string | null>(null);
  role$ = this.roleSource.asObservable();
  userId$ = this.userIdSource.asObservable();

  setUserId(userId: number) {
    this.userIdSource.next(userId);
  }

  async getuserIdAsync(): Promise<number | null> {
    return await firstValueFrom(this.userId$);
  }

  setRole(role: string) {
    this.roleSource.next(role);
  }
  //role
  // async getUserRole(): Promise<string | null>{
  //   return await firstValueFrom();
  // }
}