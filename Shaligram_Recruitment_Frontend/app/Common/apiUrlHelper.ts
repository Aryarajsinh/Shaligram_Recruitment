import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiUrlHelper {
  public apiUrl = {
    login:{
      LoginUser: 'Admin/login',
      ForgotPassword: 'Admin/forgot-password',
      VerifyVerficationCode: 'Admin/VerificationCode',
      ResetPassword: 'Admin/reset-password',
      EmailVerification: 'account/email-verification',
      RegisterUser:'account/register-user',
      userRegistrationVerification: 'account/send-user-register-otp',
      verifyuserRegistrationVerification: 'account/user-register-Verification',
      changePassword : 'account/change-password',
      LoginUserWithFacebook:'account/facebook-login', 
      Logout:'Admin/Logout'
    },
    Employee:{
      EmployeeList:'Employee/employee-list',
      EmployeeFullList :'Employee/employee-full-list',
      AddEmployee:'Employee/add-edit-employee',
      DeleteEmployee: 'employee/delete-employee/{data}',
      GetSingleEmployee: 'employee/employee-details-by-id/{data}',
      GetExcel:'employee/generate-excel',
      GetPdf:'employee/generate-pdf-dinktopdf'
    },
    CommonGet:{
      Country:'General/country-list',
      State:'General/state-list/{data}',
      City:'General/city-list/{data}',
      Skill: 'General/skill-list'
    },
    Profile:{
      GetDetail:"profile/user-profile/{data}",
      EditDetail:"profile/edit-profile"
    },
    Dashboard:{
      GraphUrl:"general/graph-details"
    },
    Student:{
      AddStudent:"Student/AddEditStudent",
      GetCollegeName:"Student/GetCollegeName",
      GetYears:"Student/GetBatchYears",
      UploadStudentFile:"Student/UploadFile",
      GetStudentList:'Student/getstudentlist',
      GetStudentById: "Student/student-details-by-id/{data}",
      DeleteStudent: "Student/delete-student/{StudentId}",
      StudentListWithPagination: 'student/getstudentlistbypagination'
    },
    College:{
      AddBatch:"CollegeBatch/AddEditCollegebatch",
      GetBatchList:"CollegeBatch/collegebatch-full-list",
      GetBatchById: "CollegeBatch/collegebatch-details-by-id/{data}",
      DeleteBatch: "CollegeBatch/delete-collegebatch/{BatchId}",    
    },
    QuestionSet:{
      AddQuestionSet:"QuestionSet/add-edit-questionSet",
      GetQuestionSetList:"QuestionSet/questionSet-full-list",
      GetQuestionSetById: "QuestionSet/questionSet-details-by-id/{data}",
      DeleteQuestionSet: "QuestionSet/delete-questionSet/{QuestionSetId}",    
    }
  };
}
