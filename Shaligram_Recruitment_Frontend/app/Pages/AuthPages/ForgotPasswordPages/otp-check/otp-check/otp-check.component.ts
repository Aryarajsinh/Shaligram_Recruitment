import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonService } from '../../../../../Service/Common/common.service';
import { ToastrService } from 'ngx-toastr';
import { ApiUrlHelper } from '../../../../../Common/apiUrlHelper';
import { Router } from '@angular/router';
import { StorageService } from '../../../../../Service/Storage/storage.service';
import { Timeout, WentWrong } from '../../../../../Common/Messages';
import { NgOtpInputModule } from 'ng-otp-input';
import { ApiResponse } from '../../../../../Models/CommonModel';

@Component({
  selector: 'app-otp-check',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NgOtpInputModule],
  templateUrl: './otp-check.component.html',
  styleUrl: './otp-check.component.css'
})
export class OtpCheckComponent implements OnInit {
  // Common Properties
  error: boolean = false;
  email: string = '';
  otpValue: string = '';
  timer: any;
  remainingTime: number = 0;

  constructor(private commonService: CommonService,
    private apiURL: ApiUrlHelper,
    private toast: ToastrService,
    private route: Router,
    private storageService: StorageService
  ) { }

  ngOnInit() {
    this.startTimer();
  }

  ngOnDestroy() {
    this.stopTimer();

  }

  ValueChange(otp: any) {
    this.otpValue = otp
  }

  // Checking Of Otp id Valid
  OTPValidCheck() {
    if (this.otpValue.length == 6) {
      this.error = false;
      this.OTPCheck();
    }
    else {
      this.error = true;
    }
  }

  // Otp Check And If Valid Redirect
  OTPCheck() {
    if (!this.error) {
      this.email = this.commonService.Decrypt(this.storageService.getValue('EncryptedEmail'));


      var OTPModel = {
        Email: this.email,
        otp: Number.parseInt(this.otpValue)
      }
      console.log(OTPModel);
      this.commonService.doPost(this.apiURL.apiUrl.login.VerifyVerficationCode, OTPModel).pipe().subscribe({
        next: async (Response: ApiResponse) => {
         
          if (Response.success) {
            this.toast.success(Response.message);

            this.route.navigate(['/resetpassword']);

          }
          else {
            this.toast.error(Response.message);
            this.route.navigate(['/forgotpassword']);
          }
        },
        error: (er) => {
          this.toast.error(WentWrong)
        }
      });
    }
  }


  // Timer For Otp Valid
  startTimer() {
    const duration = 120;
    let timerValue = duration;

    this.timer = setInterval(() => {
      timerValue--;
      this.remainingTime = timerValue;

      if (timerValue <= 0) {
        this.stopTimer();
        this.toast.error(Timeout);
        this.route.navigate(['/forgotpassword']);
      }
    }, 1000);
  }

  // Stop Timer Method
  stopTimer() {
    clearInterval(this.timer);
  }

  // Resend Otp function
  ResendOtp() {
    var Email: string = this.storageService.getValue('EncryptedEmail');
    var EmailModel = {
      emailId: this.commonService.Decrypt(Email)
    }
    this.commonService.doPost(this.apiURL.apiUrl.login.ForgotPassword, EmailModel).pipe().subscribe({
      next: async (Response: ApiResponse) => {
        console.log('API Response:', Response);
        if (Response.success) {
          this.toast.success(Response.message);
          console.log('Navigating to reset password');
          this.route.navigate(['/resetpassword']);
        } else {
          this.toast.error(Response.message);
          console.log('Navigating to forgot password');
          this.route.navigate(['/forgotpassword']);
        }
      },
      error: (er) => {
        this.toast.error(WentWrong);
      }
    });

  }
}


