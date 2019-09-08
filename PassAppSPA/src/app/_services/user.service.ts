import { HttpClient } from '@angular/common/http';
import { User } from './../_models/User';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  // baseUrl: string = 'https://localhost:5000/api/user/';
  baseUrl = environment.apiUrl + 'user/';
  user: User;

  constructor(private http: HttpClient) { }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(this.baseUrl + id);
  }

  deleteUser(id: number, user: any){
    return this.http.put(this.baseUrl + id, user);
  }
}
