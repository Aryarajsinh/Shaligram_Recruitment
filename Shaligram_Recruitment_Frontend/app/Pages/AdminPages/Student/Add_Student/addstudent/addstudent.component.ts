import { Component } from '@angular/core';
import { CommonService } from '../../../../../Service/Common/common.service';
import { ToastrService } from 'ngx-toastr';
import { ApiUrlHelper } from '../../../../../Common/apiUrlHelper';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApiResponse } from '../../../../../Models/CommonModel';
import { WentWrong } from '../../../../../Common/Messages';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-addstudent',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    MatFormFieldModule,
    MatCardModule,
    MatInputModule,
    MatDialogModule,
    MatIconModule    
  ],
  templateUrl: './addstudent.component.html',
  styleUrl: './addstudent.component.css'
})
export class AddstudentComponent  { 

  constructor(private CommonServ: CommonService, 
    private Toast: ToastrService, 
    private api: ApiUrlHelper,    
    private dialogRef: MatDialogRef<AddstudentComponent>,
    ) { }

  // Basic Properties
  submitted = false;
  
  // Student reactive Form
  StudentForm = new FormGroup({
    StudentName: new FormControl('', [Validators.required]),
    CollegeName: new FormControl('' ,[Validators.required] ),
    EmailId: new FormControl('', [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')]),
    PhoneNumber: new FormControl('', [Validators.required]),
    BatchYear: new FormControl('', [Validators.required]),
  });

   // Form Validation Controls
   get StudentFormValidations() {
    return this.StudentForm.controls;
  }  
  // Submit Form Method
  SubmitStudent(){     
    this.submitted = true;
    if(this.StudentForm.valid){
      var StudentModel = {
        studentId: 0,
        studentName: this.StudentForm.value.StudentName,
        collegeName: this.StudentForm.value.CollegeName,   
        emailAddress: this.StudentForm.value.EmailId,
        phoneNumber: this.StudentForm.value.PhoneNumber,
        batchYear: this.StudentForm.value.BatchYear,           
      }
   
      this.CommonServ.doPost(this.api.apiUrl.Student.AddStudent, StudentModel).pipe().subscribe({
        next: async (response: ApiResponse) => {
          if (response.success == true) {                     
            this.Toast.success(response.message);
            this.StudentForm.reset();
            this.dialogRef.close(true);            
          }
          else {
            this.Toast.error(response.message);
          }
        },
        error: (er) => {
          this.Toast.error(WentWrong);
        }
      });
    }
  }
}

