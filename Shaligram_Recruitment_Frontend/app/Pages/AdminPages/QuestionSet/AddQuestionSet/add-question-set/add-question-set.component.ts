import { Component } from '@angular/core';
import { CommonService } from '../../../../../Service/Common/common.service';
import { ToastrService } from 'ngx-toastr';
import { ApiUrlHelper } from '../../../../../Common/apiUrlHelper';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApiResponse } from '../../../../../Models/CommonModel';
import { WentWrong } from '../../../../../Common/Messages';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-add-question-set',
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
  templateUrl: './add-question-set.component.html',
  styleUrl: './add-question-set.component.css'
})
export class AddQuestionSetComponent {
  constructor(private CommonServ: CommonService,
    private Toast: ToastrService,
    private api: ApiUrlHelper,    
    private dialogRef: MatDialogRef<AddQuestionSetComponent>,
  ) { }

  // Basic Properties
  submitted = false;

  // Student reactive Form
  QuestionSetForm = new FormGroup({
    QuestionSetName: new FormControl('', [Validators.required]), 
  });

  // Form Validation Controls
  get StudentFormValidations() {
    return this.QuestionSetForm.controls;
  }
  
  // Submit Form Method
  SubmitBatch() {  

    this.submitted = true;
    if (this.QuestionSetForm.valid) {
      var QuestionSetModel = {        
        questionSetName: this.QuestionSetForm.value.QuestionSetName,   
      }

      this.CommonServ.doPost(this.api.apiUrl.QuestionSet.AddQuestionSet, QuestionSetModel).pipe().subscribe({
        next: async (response: ApiResponse) => {
          if (response.success == true) {
            this.Toast.success(response.message);
            this.QuestionSetForm.reset();
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


