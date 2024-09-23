import { Component, ViewChild } from '@angular/core';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { CommonService } from '../../../../Service/Common/common.service';
import { ApiUrlHelper } from '../../../../Common/apiUrlHelper';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { ApiResponse } from '../../../../Models/CommonModel';
import Swal from 'sweetalert2';
import { NotDeleted, WentWrong } from '../../../../Common/Messages';
import { AddQuestionSetComponent } from '../../QuestionSet/AddQuestionSet/add-question-set/add-question-set.component';
import { EditQuestionSetComponent } from '../../QuestionSet/EditQuestionSet/edit-question-set/edit-question-set.component';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
@Component({
  selector: 'app-questionset',
  standalone: true,
  imports: [
    MatPaginatorModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSortModule, 
    CommonModule,
    ReactiveFormsModule,
  ],
  templateUrl: './questionset.component.html',
  styleUrl: './questionset.component.css'
})
export class QuestionsetComponent {
  dataSource: MatTableDataSource<any> = new MatTableDataSource<any>();
  pageSize: number = 10;
  pageSizeOptions: number[] = [3, 10, 25, 100];

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  ngOnInit(): void {
    this.LoadQuestionSetData();
  }

  constructor(
    private api: ApiUrlHelper,   
    private commonService: CommonService,
    private toaster: ToastrService,
    private dialog: MatDialog
  ) {
    this.LoadQuestionSetData();
  }


  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  displayedColumns: string[] = ['questionSetId', 'questionSetName','action'];

  LoadQuestionSetData() {
    this.commonService.doGet(this.api.apiUrl.QuestionSet.GetQuestionSetList).subscribe({
      next: (response: ApiResponse) => {
        if (response && response.data) {
          this.dataSource = new MatTableDataSource(response.data);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
        }
      },
      error: (err) => {
        // Handle any errors here
        console.error('Error loading student data:', err);
      }
    });
  }
  SearchValueChange(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }
  DeleteQuestionSet(id: number, name: string) {
    let QuestionSetId: number = id;    
    Swal.fire({
      title: 'Are you sure?',
      text: `You Really Want To Delete User ${name}`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, cancel!',
    }).then((result) => {
      if (result.isConfirmed) {
        console.log("User deleted successfully");
        console.log(QuestionSetId)
        this.commonService.doDelete(this.api.apiUrl.QuestionSet.DeleteQuestionSet.replace("{QuestionSetId}", QuestionSetId.toString()), QuestionSetId).pipe().subscribe({
          next: (response: ApiResponse) => {
            if (response.success == true) {
              this.LoadQuestionSetData();
              this.toaster.success(response.message);
            }
            else {
              this.toaster.error(response.message);
            }
          },
          error: (er) => {
            this.toaster.error(WentWrong);
          }
        }
        )
      } else if (result.dismiss === Swal.DismissReason.cancel) {
        this.toaster.error(NotDeleted);
      }
    });
  }

  AddQuestionSet() {  
      // Open the dialog and pass data to it
      const dialogRef = this.dialog.open(AddQuestionSetComponent, {
        width: '700px',  // You can customize the size of the dialog
          // Passing the encrypted ID to the dialog
      });  
      // After the dialog is closed
      dialogRef.afterClosed().subscribe(result => {       
        if (result) {        
          this.LoadQuestionSetData()
        }
       
      })
    }
 
  EditQuestionSet(Id: any) {
    var id = this.commonService.Encrypt(Id);
    console.log(id);
      // Open the dialog and pass data to it
      const dialogRef = this.dialog.open(EditQuestionSetComponent, {
        width: '700px',  // You can customize the size of the dialog
        data: { id: id }  // Passing the encrypted ID to the dialog
      });
  
      // After the dialog is closed
      dialogRef.afterClosed().subscribe(result => {
        console.log('The dialog was closed', result);
        if (result) {
          this.LoadQuestionSetData()
        }
      })
    }  
}
