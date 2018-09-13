import { AlertifyService } from './../_services/alertify.service';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  model: any = {}; 

  constructor(private authService: AuthService, 
      private alertify: AlertifyService,
      private router: Router) { }

  ngOnInit() {
  }

  login(){
    this.authService.login(this.model).subscribe(data => {
      this.alertify.success('login successful');
    }, error => {
      this.alertify.error('Username or password don\'t match');
    }, () => {
      this.router.navigate(['/menu']);
    });
  }

  loggedIn(){
    return this.authService.loggedIn();
  }
}
