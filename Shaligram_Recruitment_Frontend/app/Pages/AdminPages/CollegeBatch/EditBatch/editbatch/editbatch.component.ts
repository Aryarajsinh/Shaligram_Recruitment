import { Component, Inject } from '@angular/core';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { CommonService } from '../../../../../Service/Common/common.service';
import { ApiUrlHelper } from '../../../../../Common/apiUrlHelper';

import { ToastrService } from 'ngx-toastr';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { ApiResponse } from '../../../../../Models/CommonModel';

import { WentWrong } from '../../../../../Common/Messages';

import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-editbatch',
  standalone: true,
  imports: [
      MatFormFieldModule,
    MatInputModule,  
    MatCardModule,    
    MatDialogModule,
    MatIconModule,
    ReactiveFormsModule
  ],
  templateUrl: './editbatch.component.html',
  styleUrl: './editbatch.component.css'
})
export class EditbatchComponent {
 
  constructor(
    private routing: Router, 
    private CommonServ: CommonService, 
    private api: ApiUrlHelper, 
    private toast: ToastrService,   
    public dialogRef: MatDialogRef<EditbatchComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
    
    ) { 
      const decryptedId = this.CommonServ.Decrypt(this.data.id);
      console.log('Decrypted ID:', decryptedId);  // Log the decrypted ID
    
    // Validate the decrypted ID
    if (!isNaN(Number(decryptedId))) {
      this.batchId = Number.parseInt(decryptedId);
      console.log('Parsed Student ID:', this.batchId);
      this.GetUserData();
    } else {
      console.error('Decrypted ID is not a valid number');
    }
    }
 
  // Basic Properties
  submitted = false;
  batchId: any;
  SingleBatchResponse: any;
  
  // Edit Student reactive Form
  DetailsCollegeBatch = new FormGroup({
    CollegeName: new FormControl('', [Validators.required]),
    Years: new FormControl('', [Validators.required]),
    BatchName: new FormControl('', [Validators.required]),
  });

    // Form Validation Controls
    get StudentFormValidations() {
      return this.DetailsCollegeBatch.controls;
    }

  // Get Single User Response
  GetUserData(){
    this.CommonServ.doGet(this.api.apiUrl.College.GetBatchById.replace("{data}" ,this.batchId )).pipe().subscribe({
      next: async (response:ApiResponse)=>{
        console.log(response)
        this.SingleBatchResponse = response.data;
        this.DetailsCollegeBatch.patchValue({
          CollegeName: this.SingleBatchResponse.collegeName,
          Years: this.SingleBatchResponse.years,
          BatchName: this.SingleBatchResponse.batchName,    
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
    if(this.DetailsCollegeBatch.valid){        
      var CollegeBatchModel = {
        batchId:this.batchId,
        collegeName: this.DetailsCollegeBatch.value.CollegeName,
        years: this.DetailsCollegeBatch.value.Years,
        batchName: this.DetailsCollegeBatch.value.BatchName,
      }
      console.log(CollegeBatchModel)
      this.CommonServ.doPost(this.api.apiUrl.College.AddBatch, CollegeBatchModel).pipe().subscribe({
        next: async (response: ApiResponse) => {
          if (response.success == true) {
            this.toast.success(response.message);
            this.DetailsCollegeBatch.reset();
            this.dialogRef.close(true);
            
          }
          else {
            this.toast.error(response.message);
            this.dialogRef.close(false);
          }
        },
        error: (er) => {
          this.toast.error(WentWrong);
        }
      });
    }
  }

  

}

