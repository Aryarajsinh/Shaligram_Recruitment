import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { CommonService } from '../../../../Service/Common/common.service';
import { ApiUrlHelper } from '../../../../Common/apiUrlHelper';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { StorageService } from '../../../../Service/Storage/storage.service';
import { ApiResponse } from '../../../../Models/CommonModel';
import { WentWrong } from '../../../../Common/Messages';
import { HttpClient, HttpClientModule } from '@angular/common/http';



@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,FormsModule,RouterLink,ReactiveFormsModule,HttpClientModule,ToastrModule],
  providers :[HttpClient],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  

  constructor(private commonService: CommonService,
    private apiURL: ApiUrlHelper,
    private toast: ToastrService,
    private route: Router,
    
    private storageService: StorageService    
  ) {}
  admin: any;
  loggedIn: any;

  passwordFieldType : string= 'password';
  password : string= '';

  togglePasswordVisibility() {
    this.passwordFieldType = this.passwordFieldType === 'password' ? 'text' : 'password';
  }
  
  ngOnInit(): void {
    if (this.storageService.getValue('Token')) {
      this.route.navigate(['/dashboard']);
    }
    this.storageService.clearStorage();  
}

// Reactive Login Form 
LoginForm = new FormGroup({
  emailId: new FormControl('', [Validators.required, Validators.email, Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/)]),
  password: new FormControl('', [Validators.required])
})

// Get Method For Apply Validations
get LoginValidations() {
  return this.LoginForm.controls;
}

// Checking If User Added Correct Values
submitted = false;
UserValidCheck() {
  this.submitted = true;
  if (this.LoginForm.valid) {
    this.LoginUser();
  }
}



// Submit Login Form
LoginUser() { 
  if (this.LoginForm.valid) {
    var LoginModel = {
      Email: this.LoginForm.value.emailId,
      Password: this.LoginForm.value.password
    } 
    
    this.commonService.doPost(this.apiURL.apiUrl.login.LoginUser, LoginModel).pipe().subscribe({


      next: async (res: ApiResponse) => {
        if (res.success == true) {
          this.toast.success(res.message)
          this.storageService.setValue('Token', res.data.token)
          this.storageService.setValue('EncryptedEmail', this.commonService.Encrypt(this.LoginForm.value.emailId!))
                
          this.route.navigate(["/admin"])
        }
        else {
          this.toast.error(res.message);        

        }
      },
      error: () => {
        this.storageService.clearStorage()
        this.toast.error(WentWrong)
             
      }
    });
  }
}
}

