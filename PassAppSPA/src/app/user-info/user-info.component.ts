import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from './../_services/user.service';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.css']
})
export class UserInfoComponent implements OnInit {
  passwordDelete: string = '';
  user: any = {};
  userPass: any = {};
  changePassForm: FormGroup;
  
  constructor(private userService: UserService, 
      private authService: AuthService,
      private alertify: AlertifyService,
      private router: Router,
      private fb: FormBuilder) { }

  ngOnInit() {
    this.getUser();
    this.createChangePassForm();
  } 

  createChangePassForm(){
    this.changePassForm = this.fb.group({
      password: ['',
        [Validators.required, Validators.minLength(4), Validators.maxLength(6)]
      ],
      newPassword: ['',
        [Validators.required, Validators.minLength(4), Validators.maxLength(6)]
      ],
      confirmPassword: ['', Validators.required]
    }, 
    {validator: this.passwordMatchValidator})
  }

  passwordMatchValidator(g: FormGroup){
    return g.get('newPassword').value === g.get('confirmPassword').value ? null : {'mismatch': true};
  }
  formError(){
    return this.changePassForm.get('newPassword').touched && this.changePassForm.get('newPassword').hasError('required') ? ' New password is required' :
    this.changePassForm.get('newPassword').hasError('minlength') ? 'New password must be at least 4 digits long' : null
  }

  getUser(){
    this.userService.getUser(this.authService.decodedToken.nameid)
      .subscribe(user => {
        this.user = user;
      }, error => {
        this.alertify.error(error);
      });
  }

  deleteUser(){
    this.user.password = this.passwordDelete;
    this.alertify.confirm('Are you sure you want to delete your account?', () => {
      this.userService.deleteUser(+this.authService.decodedToken.nameid, this.user)
      .subscribe( next => {
        this.logout();
        this.alertify.success('Your account has been deleted');
      }, error => {
        this.passwordDelete = '';
        this.alertify.error(error);
      });
    });
  }

  changePass(){
    if (this.changePassForm.valid){
      this.userPass = Object.assign({}, this.changePassForm.value);
      this.userPass.username = this.user.username;
      this.authService.changePass(this.userPass)
        .subscribe( next => {
          this.userPass = {};
          this.changePassForm.reset();
          this.alertify.success('Your password has been changed');
          this.router.navigate(['/menu']);
        }, error => {
          this.changePassForm.reset();
          this.alertify.error(error);
        })
    }
  }



  logout(){
    this.authService.userToken = null;
    this.authService.currentUser = null;

    localStorage.removeItem('user');
    localStorage.removeItem('token');

    this.router.navigate(['/home']);
  }

}
