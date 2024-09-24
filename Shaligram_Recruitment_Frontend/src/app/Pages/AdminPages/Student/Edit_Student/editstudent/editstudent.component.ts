// import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { ApiResponse } from '../../../../../Models/CommonModel';
import { WentWrong } from '../../../../../Common/Messages';
import {Router } from '@angular/router';
import { CommonService } from '../../../../../Service/Common/common.service';
import { ApiUrlHelper } from '../../../../../Common/apiUrlHelper';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { Component, Inject } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
@Component({
  selector: 'app-editstudent',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,  
    MatFormFieldModule,
    MatCardModule ,
    MatDialogModule,
    MatIconModule  , 
    MatInputModule
  ],
  templateUrl: './editstudent.component.html',
  styleUrl: './editstudent.component.css'
})
export class EditstudentComponent { 

  constructor(
    private routing: Router, 
    private CommonServ: CommonService, 
    private api: ApiUrlHelper, 
    private toast: ToastrService,   
    public dialogRef: MatDialogRef<EditstudentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
    
    ) { 
      const decryptedId = this.CommonServ.Decrypt(this.data.id);
      console.log('Decrypted ID:', decryptedId);  // Log the decrypted ID
    
    // Validate the decrypted ID
    if (!isNaN(Number(decryptedId))) {
      this.studentId = Number.parseInt(decryptedId);
      console.log('Parsed Student ID:', this.studentId);
      this.GetUserData();
    } else {
      console.error('Decrypted ID is not a valid number');
    }
    }
 

  // Basic Properties
  submitted = false;  
  studentId:any;
  SingleStudentResponse:any;


    // Edit Student reactive Form
    EditStudentForm = new FormGroup({
      StudentName: new FormControl('', [Validators.required]),
      CollegeName: new FormControl('' ,[Validators.required] ),
      EmailId: new FormControl('', [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')]),
      PhoneNumber: new FormControl('', [Validators.required]),
      BatchYear: new FormControl('', [Validators.required]),

 
    });

    // Form Validation Controls
    get StudentFormValidations() {
      return this.EditStudentForm.controls;
    }

  // Get Single User Response
  GetUserData(){
    this.CommonServ.doGet(this.api.apiUrl.Student.GetStudentById.replace("{data}" ,this.studentId )).pipe().subscribe({
      next: async (response:ApiResponse)=>{
        console.log(response)
        this.SingleStudentResponse = response.data;     
        this.EditStudentForm.patchValue({
          StudentName: this.SingleStudentResponse.studentName,
          CollegeName: this.SingleStudentResponse.collegeName,
          EmailId: this.SingleStudentResponse.emailAddress,         
          PhoneNumber: this.SingleStudentResponse.phoneNumber,         
          BatchYear: this.SingleStudentResponse.batchYear,         
        })
      },
      error: (er) => {
        this.toast.error(WentWrong)
      }
    })
  }

  // Submit Edit User
  SubmitEditUser(){
    this.submitted = true;
    if(this.EditStudentForm.valid){    
    
      var StudentModel = {
        studentId: this.studentId,
        studentName: this.EditStudentForm.value.StudentName,
        CollegeName: this.EditStudentForm.value.CollegeName,   
        studentEmail: this.EditStudentForm.value.EmailId,
        PhoneNumber: this.EditStudentForm.value.PhoneNumber,
        BatchYear: this.EditStudentForm.value.BatchYear,        
      }

      this.CommonServ.doPost(this.api.apiUrl.Student.AddStudent, StudentModel).pipe().subscribe({
        next: async (response: ApiResponse) => {
          if (response.success == true) {
            this.toast.success(response.message);
            this.dialogRef.close(true);  
          }
          else {
            this.toast.error(response.message);
          }
        },
        error: (er) => {
          this.toast.error(WentWrong);
        }
      });
    }
  }

  

}

