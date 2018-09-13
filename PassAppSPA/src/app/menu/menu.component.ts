import { Password } from './../_models/Password';
import { AlertifyService } from './../_services/alertify.service';
import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';
import { PasswordService } from '../_services/password.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  loggedInTime: number = 0;
  passwordsCount: number = 0;
  
  constructor(public authService: AuthService, 
      private alertify: AlertifyService,
      private router: Router,
      private passwordService: PasswordService) {}

  ngOnInit() {
    this.getLoggedInTime();
    this.getPasswords();
  }

  getPasswords(){
    this.passwordService.getPasswords(this.authService.decodedToken.nameid)
      .subscribe( passwords => {
        this.passwordsCount = passwords.length;
      }, error => {
        this.alertify.error(error);
      });
  }

  logout(){
    this.authService.userToken = null;
    this.authService.currentUser = null;

    this.alertify.message('logged out');

    localStorage.removeItem('user');
    localStorage.removeItem('token');
    localStorage.removeItem('logTime');

    this.router.navigate(['/home']);
  }
  
  getLoggedInTime(){
    let time = localStorage.getItem('logTime');
    this.loggedInTime = +time - new Date().getTime();
    this.loggedInTime = Math.round(this.loggedInTime / 60000);
  }
}
