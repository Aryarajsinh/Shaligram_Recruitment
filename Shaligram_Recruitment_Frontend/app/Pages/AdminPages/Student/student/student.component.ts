import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { CommonService } from '../../../../Service/Common/common.service';
import { ApiUrlHelper } from '../../../../Common/apiUrlHelper';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ApiResponse } from '../../../../Models/CommonModel';
import { NotDeleted, WentWrong } from '../../../../Common/Messages';
import Swal from 'sweetalert2';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDialog } from '@angular/material/dialog';
import { EditstudentComponent } from '../Edit_Student/editstudent/editstudent.component';
import { AddstudentComponent } from '../Add_Student/addstudent/addstudent.component';
import { UploadfileComponent } from '../UploadFile/uploadfile/uploadfile.component';
@Component({
  selector: 'app-student',
  standalone: true,
  imports: [
    MatPaginatorModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSortModule,    
  ],
  templateUrl: './student.component.html',
  styleUrl: './student.component.css'
})
export class StudentComponent implements OnInit, AfterViewInit {
  dataSource: MatTableDataSource<any> = new MatTableDataSource<any>();
  pageSize: number = 10;
  pageSizeOptions: number[] = [3, 10, 25, 100];

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  ngOnInit(): void {
    this.LoadStudentData();
  }

  constructor(private CommonServ: CommonService,
    private api: ApiUrlHelper,
    private route: Router,
    private commonService: CommonService,
    private toaster: ToastrService,
    private dialog: MatDialog
  ) {
    this.LoadStudentData();
  }


  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  displayedColumns: string[] = ['studentId', 'studentName', 'collegeName', 'batchYear', 'emailAddress', 'phoneNumber', 'action'];
  // dataSource: MatTableDataSource<any> = new MatTableDataSource();


  LoadStudentData() {
    this.commonService.doGet(this.api.apiUrl.Student.GetStudentList).subscribe({
      next: (response: ApiResponse) => {

        if (response && response.data) {

          // Setting the data source and integrating paginator and sort
          this.dataSource = new MatTableDataSource(response.data);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
        }
      },
      error: (err) => {
        // Handle any errors here
        this.route.navigate(['/login'])
        console.error('Error loading student data:', err);
      }
    });
  }
  SearchValueChange(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }
  DeleteUser(id: number, name: string) {
    let DeleteId: number = id;
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

        this.commonService.doDelete(this.api.apiUrl.Student.DeleteStudent.replace("{StudentId}", DeleteId.toString()), DeleteId).pipe().subscribe({
          next: (response: ApiResponse) => {
            if (response.success == true) {
              this.LoadStudentData();
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

  AddStudent() {  
      // Open the dialog and pass data to it
      const dialogRef = this.dialog.open(AddstudentComponent, {
        width: '700px',  // You can customize the size of the dialog
          // Passing the encrypted ID to the dialog
      });  
      // After the dialog is closed
      dialogRef.afterClosed().subscribe(result => {       
        if (result) {
          this.LoadStudentData()
        }
      })
    }
    
    UploadFile() {  
      // Open the dialog and pass data to it
      console.log("Uploading file call");
      
      const dialogRef = this.dialog.open(UploadfileComponent, {
        width: '700px',  // You can customize the size of the dialog
          // Passing the encrypted ID to the dialog
      });  
      // After the dialog is closed
      dialogRef.afterClosed().subscribe(result => {       
        if (result) {
          console.log("Uploading file call dialog completed");          
         this.LoadStudentData();
        }
      })
    }
  // Redirect To Edit page
  EditUser(EditId: any) {
    var id = this.CommonServ.Encrypt(EditId);
    console.log(id);
      // Open the dialog and pass data to it
      const dialogRef = this.dialog.open(EditstudentComponent, {
        width: '700px',  // You can customize the size of the dialog
        data: { id: id }  // Passing the encrypted ID to the dialog
      });
  
      // After the dialog is closed
      dialogRef.afterClosed().subscribe(result => {
        console.log('The dialog was closed', result);
        if (result) {
          this.LoadStudentData()
        }
      })
    }  
  
}
