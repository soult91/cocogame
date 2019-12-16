import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

import { User } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  domain: string = 'http://localhost:3000'

  constructor(private http: HttpClient) {}



  // getSockUser(id: string){
  //   this.socket.emit('getUser', id);
  // }

  getUsers() {
    return this.http.get<User[]>(`${this.domain}/api/users`)
      .pipe(map(res => res));
  }

  getUser(id) {
    return this.http.get<User>(`${this.domain}/api/users/${id}`)
      .pipe(map(res => res));
  }


  addUser(newUser: User) {
    console.log(newUser);
    return this.http.post<User>(`${this.domain}/api/users`, newUser)
      .pipe(map(res => res));
  }

  deleteUser(id: number) {
    return this.http.delete<User>(`${this.domain}/api/users/${id}`)
      .pipe(map(res => res));
  }
  updateUser(newUser) {
    return this.http.put(`${this.domain}/api/users/${newUser._id}`, newUser)
      .pipe(map(res => res));
  }


}
