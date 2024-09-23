using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Common.Helpers
{
    public class ErrorMessages
    {
        #region General
        public const string SomethingWentWrong = "Something went wrong ! please try again";
        public const string EmailExists = "Email is already use";
        public const string Error = "An Error Occured";
        public const string AccessDenied = "Access Denied!";
        #endregion

        #region Account
        public const string LoginSuccess = "Login Sucessful";
        public const string LogOutSuccess = "LogOut Sucessful";
       
        public const string InvalidEmailId = "Invalid Email Address";
        public const string InvalidCredential = "Invalid Credential";
        public const string EmailIsRequired = "EmailId is required.";
        public const string EnterValidEmail = "Please Enter Valid Email.";
        public const string ForgetPasswordSuccess = "Verification code Send Successfully";
        public const string ForgetPasswordError = "An error occured while sending email!";
        public const string UserError = "Email Id is Not Register ";
        public const string VerifyOPT = "OTP Verifiy Successfully!";
        public const string NotVerifyOtp = "OTP Was Not Verifiy..!!!";
        public const string WrongOPT = "OPT Does not Match!";
        public const string ResetPasswordSuccess = "Password Changed Successfully";
        public const string ConfirmPassword = "Password and Confirmation Password Does not match";
        public const string PasswordValidation = "Both password and confirm password are required";
        public const string ValidEmail = "Email Verifiy Successfully!";
        public const string InvalidEmail = "Email Does not Match!";
        public const string RegisterUserSuccess = "User Registreation Successfull";
        public const string RegisterUserFail = "User Registreation Failed";
        public const string RegisterUserSuccessOTP = "Verification code Send Successfully";
        public const string IncorrectOldPasword = "Old Password Does not Match";
        public const string PasswordChangeSuccess = "Password Change Successfull";
        #endregion

        //#region Student
        //public const string StudentList = "List Fetch Successfully";
        //public const string AddStudent = "New Employee Add Successfully";
        //public const string EditStudent= "Employee Edit Successfully";
        //public const string DeleteStudentSuccess = "Employee Delete Successfully";
        //public const string DeleteStudentFails = "Employee Delete Failed";
        //#endregion

        #region ExcelEmployeeUpload
        public const string ColumnNameMismatch = "Column Names are not proper. Please download template file and check sequence";
        public const string ColumnSequenceMissing = "Sequence of columns are not proper. Please download template file and check sequence";
        public const string NumberOfColumnsInvalid = "Number of columns are not proper. Please download template file and check columns";
        public const string InvalidEmailAddress = "Email Address is Invalid. Please Enter New Valid Email Address";
        public const string EmptyExcelFile = "No records to import";
        public const string MaximumRecordsLimit = "Maximum 100 records can be imported at one time";
        public const string MissingFile = "File is missing";
        public const string DataImportSuccess = "Records Imported SuccessFully";
        #endregion

        #region Country
        public const string CountryList = "List Fetch Successfully";
        #endregion

        #region State
        public const string StateList = "List Fetch Successfully";
        #endregion

        #region City
        public const string CityList = "List Fetch Successfully";
        #endregion

        #region Skills
        public const string SkillsList = "List Fetch Successfully";
        #endregion

        #region Profile
        public const string ProfileUpdateSuccess = "Profile Updated Successfully";
        public const string ProfileUpdateFail = "Profile Updated Failed";
        public const string ProfileNotFound = "Profile Not Found Please Contact Admin";
        #endregion

        #region Student
        public const string StudentList = "List Fetch Successfully";
        public const string AddStudent = "New Student Add Successfully";
        public const string EditStudent = "Student Edit Successfully";
        public const string DeleteStudentSuccess = "Student Delete Successfully";
        public const string DeleteStudentFails = "Student Delete Failed";
        public const string StudentDetailsById = "Student Details By Id";
        #endregion
        #region QuestionSet
        public const string QuestionSetList = "List Fetch Successfully";
        public const string AddQuestionSet = "New QuestionSet Add Successfully";
        public const string EditQuestionSet = "QuestionSet Edit Successfully";
        public const string DeleteQuestionSetSuccess = "QuestionSet Delete Successfully";
        public const string DeleteQuestionSetFails = "QuestionSet Delete Failed";
        public const string QuestionSetDetailsById = "QuestionSet Details By Id";
        #endregion 
        #region CollegeBatch
        public const string CollegeBatchList = "List Fetch Successfully";
        public const string AddCollegeBatch = "New CollegeBatch Add Successfully";
        public const string EditCollegeBatch = "CollegeBatch Edit Successfully";
        public const string DeleteCollegeBatchSuccess = "CollegeBatch Delete Successfully";
        public const string DeleteCollegeBatchFails = "CollegeBatch Delete Failed";
        public const string CollegeBatchDetailsById = "CollegeBatch Details By Id";
        #endregion
    }
}
