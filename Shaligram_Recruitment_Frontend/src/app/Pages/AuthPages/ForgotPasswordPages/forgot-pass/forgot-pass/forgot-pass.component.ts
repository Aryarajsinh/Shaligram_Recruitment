import { Component, OnInit } from '@angular/core';
import { CommonService } from '../../../../../Service/Common/common.service';
import { ApiUrlHelper } from '../../../../../Common/apiUrlHelper';
import { ToastrService } from 'ngx-toastr';
import { Router, RouterLink } from '@angular/router';
import { StorageService } from '../../../../../Service/Storage/storage.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApiResponse } from '../../../../../Models/CommonModel';
import { WentWrong } from '../../../../../Common/Messages';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-forgot-pass',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule,RouterLink],
  templateUrl: './forgot-pass.component.html',
  styleUrl: './forgot-pass.component.css'
})
export class ForgotPassComponent implements OnInit {
  ngOnInit(): void { }
  constructor(private CommonServ: CommonService,
    private apiURL: ApiUrlHelper,
    private toast: ToastrService,
    private route: Router,
    private storageService: StorageService
  ) { }

  // Common Properties
  Email: string='';
  submitted:boolean = false;

  // Reactive Forgot Pass Email  Form 
  ForgotPassEmail = new FormGroup({
    UserEmail: new FormControl('', [Validators.required, Validators.email, Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/)]),
  })

  // Get Method For Apply Validations
  get ForgotPassValidations() {
    return this.ForgotPassEmail.controls;
  }

  // Checking If User Added Correct Values
  UserValidCheck() {
    this.submitted = true;
    if (this.ForgotPassEmail.valid) {
      this.SendEmail();
    }
  }

  //For Sending Email perpose
  SendEmail() {
    if (this.ForgotPassEmail.valid) {
      var EmailModel = {
        Email: this.ForgotPassEmail.value.UserEmail
      }
      this.CommonServ.doPost(this.apiURL.apiUrl.login.ForgotPassword, EmailModel).pipe().subscribe({
        next: async (Response: ApiResponse) => {
          if (Response.success == true) {
            this.toast.success(Response.message);
            this.Email = EmailModel.Email!
            this.Email = this.CommonServ.Encrypt(this.Email);
            this.storageService.setValue('EncryptedEmail', this.Email);
            this.route.navigate(['otpverify']);
          }
          else {
            this.toast.error(Response.message);
          }
        },
        error: (er) => {
          this.toast.error(WentWrong)
        }
      });

    }
  }

}

