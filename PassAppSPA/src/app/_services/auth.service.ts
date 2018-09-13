import { userChangePass } from './../_models/userChangePass';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthUser } from '../_models/authUser';
import { map } from 'rxjs/operators';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl: string = 'https://localhost:5000/api/auth/';
  decodedToken: any;
  currentUser: User;
  userToken: any;
  username: string; 
  logTime: number;

  constructor(private http: HttpClient, private jwtHelperService: JwtHelperService) { }

  login(model: any){
    return this.http.post<AuthUser>(this.baseUrl + 'login', model, { headers: new HttpHeaders()
      .set('Content-Type', 'application/json')})
      .pipe(
        map(user => {
          if (user) {
            localStorage.setItem('token', user.tokenString);
            this.decodedToken = this.jwtHelperService.decodeToken(user.tokenString);
            this.currentUser = user.user;
            this.userToken = user.tokenString;
            this.username = this.decodedToken.unique_name;      
            this.logTime = new Date().getTime();
            localStorage.setItem('logTime', (this.logTime +1740000).toString());
          }
        })
      )
  }

  register(user: User){
    return this.http.post(this.baseUrl + 'register', user, {headers: new HttpHeaders()
      .set('Content-Type', 'application/json')});
  }

  loggedIn(){
    const token = this.jwtHelperService.tokenGetter();

    if(!token) return false;

    return !this.jwtHelperService.isTokenExpired(token);
  }

  changePass(user: userChangePass){
    return this.http.put(this.baseUrl + 'changepassword', user);
  }

}
