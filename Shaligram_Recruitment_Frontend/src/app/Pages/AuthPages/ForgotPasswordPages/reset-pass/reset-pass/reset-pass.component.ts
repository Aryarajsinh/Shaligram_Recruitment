import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonService } from '../../../../../Service/Common/common.service';
import { ApiUrlHelper } from '../../../../../Common/apiUrlHelper';
import { ToastrService } from 'ngx-toastr';
import { StorageService } from '../../../../../Service/Storage/storage.service';
import { PassMatchError, WentWrong } from '../../../../../Common/Messages';
import { ApiResponse } from '../../../../../Models/CommonModel';

@Component({
  selector: 'app-reset-pass',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule],
  templateUrl: './reset-pass.component.html',
  styleUrl: './reset-pass.component.css'
})
export class ResetPassComponent implements OnInit {
  ngOnInit(): void {

  }

  constructor(private CommonServ: CommonService,
    private apiURL: ApiUrlHelper,
    private toastr: ToastrService,
    private router: Router,
    private storageService: StorageService
  ) { }

  // Common Properties
  submitted = false;
  isNotMatched = false;
  message: string='';
  Email: string='';
  showPassword: boolean = false;


  // Reset Pass Form
  ResetPassForm = new FormGroup({
    Password: new FormControl('', [Validators.required]),
    ConfPasswod: new FormControl('', [Validators.required])
  })



  //Giving Form Controls For Validation
  get ResetPassFormValidation() {
    return this.ResetPassForm.controls;
  }

  // Checking If Its Valid Form
  PassValidCheck() {

    this.submitted = true;
    if (this.ResetPassForm.valid) {
      var Pass = this.ResetPassForm.value.Password;
      var ConfPass = this.ResetPassForm.value.ConfPasswod;
      if (Pass == ConfPass) {        
        this.SubmitResetPassForm();
      }
      else {
        this.isNotMatched = true;
        if (this.isNotMatched) {
          this.message = PassMatchError;
        }
      }
    }
  }

  //Submit Form 
  SubmitResetPassForm() {
    if (this.ResetPassForm.valid) {
      console.log("Submit Reset");
      
      this.Email = this.storageService.getValue('EncryptedEmail');

      this.Email = this.CommonServ.Decrypt(this.Email)
      //Model for Post method
      var ResetPassModel = {
        Email: this.Email,
        NewPassword: this.ResetPassForm.value.Password,
        ConfirmPassword: this.ResetPassForm.value.ConfPasswod
      }
     
      
      //Post service from common Service for reseting password
      this.CommonServ.doPost(this.apiURL.apiUrl.login.ResetPassword, ResetPassModel).pipe().subscribe({
        next: async (Response: ApiResponse) => {
          
          if (Response.success) {
            this.toastr.success(Response.message);
            this.router.navigate(['/login']);
          }
          else {
            this.toastr.error(Response.message);
          }
        },
        error: (er) => {
          this.toastr.error(WentWrong);
        }
      });
    }
  }
  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }
}
