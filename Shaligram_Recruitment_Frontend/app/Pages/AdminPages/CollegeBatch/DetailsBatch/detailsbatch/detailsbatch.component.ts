import { Component, Inject } from '@angular/core';
import { ApiResponse } from '../../../../../Models/CommonModel';
import { WentWrong } from '../../../../../Common/Messages';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonService } from '../../../../../Service/Common/common.service';
import { ApiUrlHelper } from '../../../../../Common/apiUrlHelper';
import { ToastrService } from 'ngx-toastr';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-detailsbatch',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatCardModule,
    MatDialogModule,
    MatIconModule,
    MatInputModule
  ],
  templateUrl: './detailsbatch.component.html',
  styleUrl: './detailsbatch.component.css'
})
export class DetailsbatchComponent {
  constructor(

    private CommonServ: CommonService,
    private api: ApiUrlHelper,
    private toast: ToastrService,
    public dialogRef: MatDialogRef<DetailsbatchComponent>,
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

  // Get Single User Response
  GetUserData() {
    this.CommonServ.doGet(this.api.apiUrl.College.GetBatchById.replace("{data}", this.batchId)).pipe().subscribe({
      next: async (response: ApiResponse) => {
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

}

