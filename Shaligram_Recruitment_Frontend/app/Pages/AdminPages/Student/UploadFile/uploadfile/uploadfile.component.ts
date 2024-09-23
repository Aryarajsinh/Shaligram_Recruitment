import { Component, OnInit } from '@angular/core';
import { CommonService } from '../../../../../Service/Common/common.service';
import { ToastrService } from 'ngx-toastr';
import { ApiUrlHelper } from '../../../../../Common/apiUrlHelper';
import { Router } from '@angular/router';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApiResponse } from '../../../../../Models/CommonModel';
import { WentWrong } from '../../../../../Common/Messages';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';

import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-uploadfile',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    MatFormFieldModule,
    MatCardModule,
    MatInputModule,
    MatDialogModule,
    MatIconModule,
    MatSelectModule
  ],
  templateUrl: './uploadfile.component.html',
  styleUrl: './uploadfile.component.css'
})
export class UploadfileComponent implements OnInit {
  constructor(private CommonServ: CommonService, 
    private Toast: ToastrService, 
    private api: ApiUrlHelper, 
    private route: Router,    
    private dialogRef: MatDialogRef<UploadfileComponent>,
    ) { }
  ngOnInit(): void {
    this.GetBatchYears()
    this.GetCollegeNames()
  }

  // Basic Properties
  submitted = false;
  CollegeName:any;
  Years:any;
  selectedFile: File | null = null;
  // Student reactive Form
  IUploadFileForm = new FormGroup({
    CollegeName: new FormControl('' ,[Validators.required] ),
    Years: new FormControl('', [Validators.required]),
    UploadFile: new FormControl('', [Validators.required]),
  });
  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;    
    }
  }
  // Form Validation Controls
  get IUploadFileFormValidations() {
    return this.IUploadFileForm.controls;
  }  

  //CollegeName List Fetch
 GetCollegeNames() {
        this.CommonServ
          .doGet(this.api.apiUrl.Student.GetCollegeName)
          .subscribe({
            next: (response: ApiResponse) => {
              this.CollegeName = response.data;
            },
          });
      }

  //BatchYear List Fetch
 GetBatchYears() {
  console.log("HIT")
        this.CommonServ
          .doGet(this.api.apiUrl.Student.GetYears)
          .subscribe({
            next: (response: ApiResponse) => {
              this.Years = response.data;
            },
          });
      }
  // Submit Form Method
  SubmitUploadFile() {     
    this.submitted = true;    
    
    if (this.selectedFile) {
      // Create a FormData object to handle file and text data
      const formData = new FormData();
     
      // Append the form data fields
      formData.append('studentId', '0'); // Example: hardcoded value, replace as needed
      formData.append('collegeName', this.IUploadFileForm.value.CollegeName || '');
      formData.append('years', this.IUploadFileForm.value.Years || '');
      
      // Append the file
      formData.append('uploadFile', this.selectedFile);
  
      // Log form data for debugging
      console.log('FormData:', formData);
  
      // Send the formData object in the post request
      this.CommonServ.doPost(this.api.apiUrl.Student.UploadStudentFile, formData).pipe().subscribe({
        next: async (response: ApiResponse) => {
          if (response.success === true) {                     
            this.Toast.success(response.message);
            this.IUploadFileForm.reset();           
            this.dialogRef.close(true);  
          } else {
            this.Toast.error(response.message);
          }
        },
        error: (er) => {
          this.Toast.error('Something went wrong');
        }
      });
    }
  }
  
}

