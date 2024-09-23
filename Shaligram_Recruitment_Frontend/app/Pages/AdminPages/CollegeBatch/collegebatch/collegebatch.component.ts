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
import { AddbatchComponent } from '../AddBatch/addbatch/addbatch.component';
import { EditbatchComponent } from '../EditBatch/editbatch/editbatch.component';
import { DetailsbatchComponent } from '../DetailsBatch/detailsbatch/detailsbatch.component';

@Component({
  selector: 'app-collegebatch',
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
  templateUrl: './collegebatch.component.html',
  styleUrl: './collegebatch.component.css'
})
export class CollegebatchComponent {
  dataSource: MatTableDataSource<any> = new MatTableDataSource<any>();
  pageSize: number = 10;
  pageSizeOptions: number[] = [3, 10, 25, 100];

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  ngOnInit(): void {
    this.LoadBatchData();
  }

  constructor(private CommonServ: CommonService,
    private api: ApiUrlHelper,   
    private commonService: CommonService,
    private toaster: ToastrService,
    private dialog: MatDialog
  ) {
    this.LoadBatchData();
  }


  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  displayedColumns: string[] = ['batchId', 'collegeName', 'years', 'batchName','action'];

  LoadBatchData() {
    this.commonService.doGet(this.api.apiUrl.College.GetBatchList).subscribe({
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
        console.error('Error loading student data:', err);
      }
    });
  }
  SearchValueChange(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }
  DeleteBatch(id: number, name: string) {
    let BatchId: number = id;
    
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
        console.log(BatchId)
        this.commonService.doDelete(this.api.apiUrl.College.DeleteBatch.replace("{BatchId}", BatchId.toString()), BatchId).pipe().subscribe({
          next: (response: ApiResponse) => {
            if (response.success == true) {
              this.LoadBatchData();
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

  AddBatch() {  
      // Open the dialog and pass data to it
      const dialogRef = this.dialog.open(AddbatchComponent, {
        width: '700px',  // You can customize the size of the dialog
          // Passing the encrypted ID to the dialog
      });  
      // After the dialog is closed
      dialogRef.afterClosed().subscribe(result => {       
        if (result) {        
          this.LoadBatchData()
        }
       
      })
    }
  // Redirect To Edit page
  DetailsBatch(Id: any) {
    var id = this.CommonServ.Encrypt(Id);   
      // Open the dialog and pass data to it
      const dialogRef = this.dialog.open(DetailsbatchComponent, {
        width: '700px',  // You can customize the size of the dialog
        data: { id: id }  // Passing the encrypted ID to the dialog
      });
  
      // After the dialog is closed
      dialogRef.afterClosed().subscribe(result => {
        console.log('The dialog was closed', result);
        if (result) {
          this.LoadBatchData()
        }
      })
    }  
  EditBatch(Id: any) {
    var id = this.CommonServ.Encrypt(Id);
    console.log(id);
      // Open the dialog and pass data to it
      const dialogRef = this.dialog.open(EditbatchComponent, {
        width: '700px',  // You can customize the size of the dialog
        data: { id: id }  // Passing the encrypted ID to the dialog
      });
  
      // After the dialog is closed
      dialogRef.afterClosed().subscribe(result => {
        console.log('The dialog was closed', result);
        if (result) {
          this.LoadBatchData()
        }
      })
    }  
}
