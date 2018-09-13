import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from './_models/User';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  year:number;

  constructor(private authService: AuthService, private jwtHelperService: JwtHelperService) {}

  ngOnInit() {
    const token = localStorage.getItem('token');
    this.year = new Date().getUTCFullYear();

    if (token) {
      this.authService.decodedToken = this.jwtHelperService.decodeToken(token);
      this.authService.username = this.authService.decodedToken.unique_name;
    }
    
  }

}
