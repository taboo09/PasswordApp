import { ViewPassUser } from './../_models/ViewPassUser';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { PasswordReg } from './../_models/PasswordReg';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Password } from '../_models/Password';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PasswordService {
  // baseUrl: string = 'https://localhost:5000/api/password/';
  baseUrl: string = environment.apiUrl + 'password/';
  password: PasswordReg;

  constructor(private http: HttpClient, private authService: AuthService) { }

  addPassword(password: PasswordReg){
    password.userId = +this.authService.decodedToken.nameid;
    return this.http.post(this.baseUrl + 'new', password);
  }

  getPasswords(userId: number): Observable<Password[]>{
    return this.http.get<Password[]>(this.baseUrl + userId);
  }

  updatePassword(password: Password){
    password.userId = +this.authService.decodedToken.nameid;
    return this.http.put(this.baseUrl + 'update', password);
  }

  viewUserPassword(id: number): Observable<ViewPassUser>{
    let userId = +this.authService.decodedToken.nameid;
    return this.http.get<ViewPassUser>(this.baseUrl + userId + '/' + id);
    
  }

  // using promise
  viewUserPassword2(id: number){
    let userId = +this.authService.decodedToken.nameid;
    return this.http.get(this.baseUrl + userId + '/' + id).toPromise();
  }

  deletePassword(id:number){
    return this.http.delete(this.baseUrl + id);
  }
}
