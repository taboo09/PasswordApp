import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PasswordService } from './../_services/password.service';
import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-new-password',
  templateUrl: './new-password.component.html',
  styleUrls: ['./new-password.component.css']
})
export class NewPasswordComponent implements OnInit {
  password: any = {};
  passwordForm: FormGroup;

  constructor(private passwordService: PasswordService,
    private alertify: AlertifyService,
    private router: Router,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.createPasswordForm();
  }
 
  createPasswordForm(){
    this.passwordForm = this.fb.group({
      name: ['', Validators.required],
      username: [],
      comment: [],
      hint: [],
      secretquestion: [],
      answer: [],
      other: [],
      passwordencrypt: ['', Validators.required ]
    })
  }

  addPassword(){
    if (this.passwordForm.valid){
      this.password = Object.assign({}, this.passwordForm.value);
      this.passwordService.addPassword(this.password).subscribe( next => {
        this.alertify.success('Your Password has been saved.');
        this.router.navigate(['/passwords']);
      }, error => {
        this.alertify.error(error);
      })
    }
  }

  cancel(){
    this.router.navigate(['/menu']);
  }
}
