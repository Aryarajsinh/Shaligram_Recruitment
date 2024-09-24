import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import Swal from 'sweetalert2';

import { StorageService } from '../../Service/Storage/storage.service';
import { CommonService } from '../../Service/Common/common.service';
import { ToastrService } from 'ngx-toastr';
import { ApiUrlHelper } from '../../Common/apiUrlHelper';
import { ApiResponse } from '../../Models/CommonModel';
import { NotLogOut, WentWrong } from '../../Common/Messages';
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  /**
   *
   */

  Email:any

  constructor(
    private commonService: CommonService,
    private Toast: ToastrService,
    private api: ApiUrlHelper,
    private storageService: StorageService,
    private route: Router
  ) {
    this.Email= this.commonService.Decrypt(this.storageService.getValue('EncryptedEmail'));
  }



  logout() {
    Swal.fire({
      title: 'Are you sure?',
      text: 'You Really want To LogOut!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, Logout',
      cancelButtonText: 'No, cancel!',
    }).then((result) => {

      if (result.isConfirmed) {
    
  const data = {
    EmailId: this.Email,
    Password:'abc'
  }
     
    console.log(data.EmailId)
    this.commonService.doPost(this.api.apiUrl.login.Logout,data).pipe().subscribe({
      next: (response: ApiResponse) => {
        console.log(response)
        if (response.success == true) {
          this.Toast.success(response.message);
          // Clear the storage (or token)
          this.storageService.clearStorage();

          // Redirect to login page after successful logout
          this.route.navigate(['login']);
        }
        else {
          this.Toast.error(response.message);
        }
      },
      error: (er) => {
        console.log(er)
        this.Toast.error(WentWrong);
      }
    }
    )
          } else if (result.dismiss === Swal.DismissReason.cancel) {
            this.Toast.error(NotLogOut);
          }
          // Redirect only if the user confirmed the action
          if (result.isConfirmed) {
            // Clear the storage (or token)
            this.storageService.clearStorage();

            // Redirect to login page after successful logout
            this.route.navigate(['login']);
          } 
        });
      }
    } 

