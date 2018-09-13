import { ViewPassUser } from './../_models/ViewPassUser';
import { Router } from '@angular/router';
import { AuthService } from './../_services/auth.service';
import { PasswordService } from './../_services/password.service';
import { Password } from './../_models/Password';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { NgForm, FormBuilder, FormGroup, Validators } from '@angular/forms';

const Status = {
  initial: 'Password',
  pending: 'Pending...'
}


@Component({
  selector: 'app-list-passwords',
  templateUrl: './list-passwords.component.html',
  styleUrls: ['./list-passwords.component.css']
})
export class ListPasswordsComponent implements OnInit {
  passwords: Password[] = [];
  passwordsReady: boolean = false;
  modelPass: Password;
  editForm: NgForm;
  // editForm: FormGroup;
  elementNr: number = 0;
  showPass: boolean = false;
  status: string = Status.initial;
  searchField: string = "";
  
  constructor(private passwordService: PasswordService, 
    private authService: AuthService,
    private alertify: AlertifyService,
    private router: Router,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.getPasswords(); 
    // this.createForm();
  }

  // Reactive Forms
  // createForm(){
  //   this.editForm = this.fb.group({
  //     created: [''],
  //     updated: [''],
  //     comment: [''],
  //     hint: [''],
  //     secretQuestion: [''],
  //     answer: [''],
  //     other: [''],
  //     password: ['', [Validators.required]],
  //   });
  // }


  editPassword(pass: Password){
    this.passwordService.updatePassword(pass)
      .subscribe( next => {
        this.elementNr = 0;
        this.alertify.success('Your data has been updated');
        this.getPasswords();
        this.showPass = false;
      }, error => {
        this.alertify.error(error);
      });
  }

  getPasswords(){
    this.passwordService.getPasswords(this.authService.decodedToken.nameid)
      .subscribe( passwords => {
        this.passwords = passwords;
        this.passwordsReady = true;
      }, error => {
        this.alertify.error(error);
      });
  }

  deletePassword(id:number){
    this.alertify.confirm('The password will be deleted and the data will be lost', () => {
      this.passwordService.deletePassword(id)
        .subscribe( next => {
          this.getPasswords();
          this.alertify.success('Password has been deleted');
        },error => {
          this.alertify.error(error);
      });
    });
  }

  cancel() {
    this.getPasswords();
    this.elementNr = 0;
    this.showPass = false;
  }

  show(id:number){
    if (this.elementNr == id) this.elementNr = 0;
    else this.elementNr = id;
    this.showPass = false;
  }

  // Using Promise with async await. Is faster
  async visibleTogglePromise(password: Password){
    this.status = Status.pending;
    if (!this.showPass){
      await this.passwordService.viewUserPassword2(this.elementNr)
        .then( (data: ViewPassUser) => {
          password.passwordEncrypt = data.password;
          this.showPass = !this.showPass;
          this.status = Status.initial;
        })
        .catch(error => {
          this.alertify.error(error);
        });
    } else {
      this.showPass = false;
      this.status = Status.initial;
    }
  }

  // Using Observable
  visibleToggleObs(password: Password){
    this.status = Status.pending;
    if (!this.showPass){
      this.passwordService.viewUserPassword(this.elementNr)
        .subscribe( PassToUser => {
          password.passwordEncrypt = PassToUser.password;
          this.showPass = !this.showPass;
        }, error => {
          this.alertify.error(error);
        });
    } else {
      this.showPass = false;
      this.status = Status.initial;
    }
  }

  clearSearchField(){
    this.searchField = '';
  }
}