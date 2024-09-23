import { Component, Inject } from '@angular/core';

import { CommonService } from '../../../../../Service/Common/common.service';
import { ApiUrlHelper } from '../../../../../Common/apiUrlHelper';
import { ToastrService } from 'ngx-toastr';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApiResponse } from '../../../../../Models/CommonModel';
import { WentWrong } from '../../../../../Common/Messages';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-edit-question-set',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,  
    MatCardModule,    
    MatDialogModule,
    MatIconModule,
    ReactiveFormsModule
  ],
  templateUrl: './edit-question-set.component.html',
  styleUrl: './edit-question-set.component.css'
})
export class EditQuestionSetComponent {
  constructor(
    private CommonServ: CommonService,
    private api: ApiUrlHelper,
    private toast: ToastrService,
    public dialogRef: MatDialogRef<EditQuestionSetComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) 
  {
    const decryptedId = this.CommonServ.Decrypt(this.data.id);
    console.log('Decrypted ID:', decryptedId);  // Log the decrypted ID

    // Validate the decrypted ID
    if (!isNaN(Number(decryptedId))) {
      this.questionsetid = Number.parseInt(decryptedId);
      console.log('Parsed Student ID:', this.questionsetid);
      this.GetQuestionSetData();
    } else {
      console.error('Decrypted ID is not a valid number');
    }
  }

  // Basic Properties
  submitted = false;
  questionsetid: any;
  SingleQuestionSetResponse: any;

  // Edit Student reactive Form
  DetailsQuestionSet = new FormGroup({
    QuestionSetName: new FormControl('', [Validators.required]),
  });

  // Form Validation Controls
  get StudentFormValidations() {
    return this.DetailsQuestionSet.controls;
  }

  // Get Single User Response
  GetQuestionSetData() {
    console.log("Get Question Set Callback");
    
    this.CommonServ.doGet(this.api.apiUrl.QuestionSet.GetQuestionSetById.replace("{data}", this.questionsetid)).pipe().subscribe({
      next: async (response: ApiResponse) => {
        console.log(response)
        this.SingleQuestionSetResponse = response.data;
        this.DetailsQuestionSet.patchValue({
          QuestionSetName: this.SingleQuestionSetResponse.questionSetName
        })
      },
      error: (er) => {
        this.toast.error(WentWrong)
      }
    })
  }

  // Submit Edit User
  SubmitEditQuestionSet() {
    this.submitted = true;
    if (this.DetailsQuestionSet.valid) {
      var QuestionSetModel = {
        questionsetid: this.questionsetid,
        questionSetName: this.DetailsQuestionSet.value.QuestionSetName
      }

      this.CommonServ.doPost(this.api.apiUrl.QuestionSet.AddQuestionSet, QuestionSetModel).pipe().subscribe({
        next: async (response: ApiResponse) => {
          if (response.success == true) {
            this.toast.success(response.message);
            this.DetailsQuestionSet.reset();
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

