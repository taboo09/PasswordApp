import { AlertifyService } from './../_services/alertify.service';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators, FormBuilder, AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  
  @Output() cancelRegister = new EventEmitter();
  model:any = {};
  registerForm: FormGroup;


  constructor(private authService: AuthService, 
    private alerttify: AlertifyService,
    private router: Router,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.createRegisterForm();
  }

  createRegisterForm(){
    this.registerForm = this.fb.group({
      username: ['', 
        [Validators.required, Validators.maxLength(12)]
      ],
      email: ['', 
        [Validators.required, Validators.email]
      ],
      password: ['',
        [Validators.required, Validators.minLength(4), Validators.maxLength(6)]
      ],
      confirmPassword: ['', Validators.required]
    },
      {validator: this.passwordMatchValidator}
    );
  }

  getErrorMessagePassword(){
    return this.registerForm.get('password').touched &&
      this.registerForm.get('password').hasError('required') ? 'Password is required' :
      this.registerForm.get('password').hasError('minlength') ? 'Password must be at least 4 characters long' : 'Password';
  }
  getErrorMessageConfirmPassword(){
    return this.registerForm.get('confirmPassword').touched &&
      this.registerForm.get('confirmPassword').hasError('required') ? 'Password is required' :
      this.registerForm.hasError('mismatch') ? 'Confirm password' : 'Password';
  }
  passwordMatchValidator(g: FormGroup){
    return g.get('password').value === g.get('confirmPassword').value ? null : {'mismatch': true};
  }

  getErrorMessageEmail() {
    return this.registerForm.get('email').touched &&
      this.registerForm.get('email').hasError('required') ? 'Email is required' :
      this.registerForm.get('email').hasError('email') ? 'Not a valid email' : 'Email';
  }

  register() {
    if(this.registerForm.valid){
      this.model = Object.assign({}, this.registerForm.value);
      this.authService.register(this.model).subscribe( () => {
        this.alerttify.success("user registrated");
      }, error => {
        this.alerttify.error(error);
      }, () => {
        this.authService.login(this.model).subscribe( () => {
          this.router.navigate(['/menu']);
        });
      });
    }
  }

  cancel(){
    this.cancelRegister.emit(false);
  }
}
