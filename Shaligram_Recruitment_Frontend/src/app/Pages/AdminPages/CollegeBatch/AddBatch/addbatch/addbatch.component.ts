import { Component, OnInit } from '@angular/core';
import { ApiResponse } from '../../../../../Models/CommonModel';
import { WentWrong } from '../../../../../Common/Messages';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonService } from '../../../../../Service/Common/common.service';
import { ToastrService } from 'ngx-toastr';
import { ApiUrlHelper } from '../../../../../Common/apiUrlHelper';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-addbatch',
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
  templateUrl: './addbatch.component.html',
  styleUrl: './addbatch.component.css'
})
export class AddbatchComponent implements OnInit {

  constructor(private CommonServ: CommonService,
    private Toast: ToastrService,
    private api: ApiUrlHelper,    
    private dialogRef: MatDialogRef<AddbatchComponent>,
  ) { }
  ngOnInit(): void {
    this.GetCollegeNames();
  }

  // Basic Properties
  submitted = false;
  CollegeName:any;
  // Student reactive Form
  CollegeForm = new FormGroup({
    CollegeName: new FormControl('', [Validators.required]),
    Years: new FormControl('', [Validators.required]),
    BatchName: new FormControl('', [Validators.required]),
  });

  // Form Validation Controls
  get StudentFormValidations() {
    return this.CollegeForm.controls;
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
  // Submit Form Method
  SubmitBatch() {

    console.log(this.CollegeForm.value.CollegeName)

    this.submitted = true;
    if (this.CollegeForm.valid) {
      var CollegeBatchModel = {        
        collegeName: this.CollegeForm.value.CollegeName,
        years: this.CollegeForm.value.Years,
        batchName: this.CollegeForm.value.BatchName,
      }

      this.CommonServ.doPost(this.api.apiUrl.College.AddBatch, CollegeBatchModel).pipe().subscribe({
        next: async (response: ApiResponse) => {
          if (response.success == true) {
            this.Toast.success(response.message);
            this.CollegeForm.reset();
            this.dialogRef.close(true);            
          }
          else {
            this.Toast.error(response.message);
            this.dialogRef.close(true);   
          }
        },
        error: (er) => {
          this.Toast.error(WentWrong);
        }
      });
    }
  }
}

