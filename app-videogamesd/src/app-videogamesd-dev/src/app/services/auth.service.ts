import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/User';
import { JwtResponseI } from '../models/jwt-response';
import { tap } from 'rxjs/operators';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  AUTH_SERVER: string = 'http://localhost:3000';
  authSubject = new BehaviorSubject(false);
  private token:  string;

  constructor(private httpClient: HttpClient) {    
  }

  register(user: User): Observable<JwtResponseI>{
    return this.httpClient.post<JwtResponseI>(`${this.AUTH_SERVER}/register`,
    user).pipe(tap(
      (res: JwtResponseI) => {
        if (res) {
          this.saveToken(res.dataUser.accessToken, res.dataUser.expiresInt);        }
      }));
  }

  login(user: User): Observable<JwtResponseI>{
    return this.httpClient.post<JwtResponseI>(`${this.AUTH_SERVER}/api/login`,
    user).pipe(tap(
      (res: JwtResponseI) => {
        if (res) {
          this.saveToken(res.dataUser.accessToken, res.dataUser.expiresInt);
        }
      }));
  }


  logout(){
    this.token = '';
    localStorage.removeItem("ACCESS_TOKEN");
    localStorage.removeItem("EXPIRES_IN");
  }

  private saveToken(token: string, expiresIn: string): void {
    localStorage.setItem("ACCESS_TOKEN", token);
    localStorage.setItem("EXPIRES_IN", expiresIn);
    this.token = token;
  }

  private getToken():string {
    if(!this.token){
      this.token= localStorage.getItem("ACCESS_TOKEN");
    }
    return this.token;
  }
}
