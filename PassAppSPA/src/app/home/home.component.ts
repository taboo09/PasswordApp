import { AlertifyService } from './../_services/alertify.service';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode: boolean= false;
  loggedIn: boolean = false;

  constructor(private authService: AuthService,
    private alertify: AlertifyService,
    private router: Router) { }

  ngOnInit() {
    if (this.authService.loggedIn()) {
      this.loggedIn = true;
    }
  }

  registerToggle(){
    this.registerMode = true;
  }

  cancelRegisterMode(registerModeType: boolean){
    this.registerMode = registerModeType;
  }

  logout(){
    this.authService.userToken = null;
    this.authService.currentUser = null;

    this.alertify.message('logged out');

    localStorage.removeItem('user');
    localStorage.removeItem('token');

    this.router.navigate(['**']);
    this.loggedIn = false;
  }
  
}
