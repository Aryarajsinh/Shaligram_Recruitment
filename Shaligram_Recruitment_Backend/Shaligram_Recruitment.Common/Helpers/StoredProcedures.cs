using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Common.Helpers
{
    public class StoredProcedures
    {
        public const string RegisterUser = "SP_RegisterUser";
        public const string UserLogin = "SP_UserLogin";
        public const string StudentList = "SP_StudentList";
        public const string CollegeNameList = "SP_CollegeNameList";
        public const string CollegeBatchList = "SP_CollegeBatchList";
        public const string BatchYears = "SP_GetBatchYears";
        public const string QuestionSet = "SP_GetQuestionSet";
        public const string StudentDetails = "SP_StudentDetails";
        public const string CollegebatchDetails = "SP_CollegebatchDetails";
        public const string QuestionSetDetails = "SP_QuestionSetDetails";
        public const string StudentAdd = "Sp_StudentAdd";
        public const string QuestionAdd = "SP_SaveQuestion";
        public const string QuestionSetAdd = "SP_QuestionSetAdd";
        public const string CollegeBatchAdd = "Sp_CollegeBatchAdd";
        public const string CollegeBatchEdit = "SP_CollegeBatchEdit";
        public const string QuestionSetEdit = "SP_QuestionSetEdit";
        public const string DeleteStudent = "SP_DeleteStudent";
        public const string DeleteQuestionSet = "SP_DeleteQuestionSet";
        public const string DeleteBatch = "SP_DeleteBatch";
        public const string ValidateToken = "SP_Validate_Token";
        public const string UpdateToken = "SP_UpdateToken";
        public const string UserDataByEmail = "SP_GetUserDatabyEmail";
        public const string VerifyOTP = "SP_VerifyOTP";
        public const string SaveOTP = "SP_SaveOTP";
        public const string UpdateOTP = "SP_UpdateOTP";
        public const string ResetPassword = "SP_UpdateUserPassword";
        public const string GetAllUsers = "sp_GetAll_User";
        public const string AddEditUser = "SP_Add_Edit_User";
        public const string GetUserById = "SP_Get_User_By_Id";
        public const string DeleteUser = "SP_Delete_User";
        public const string UserStatus = "SP_Status";
        public const string CheckUser = "Sp_CheckUser";
        public const string UpdateOtpByEmail = "SP_UpdateOtpByEmail";
        public const string LogOut = "SP_LogOut";


        #region Employee
        public const string GetEmployeeList = "SP_GetEmployeeList";
        public const string AddEditEmployee = "SP_AddEditEmployee";
        public const string DeleteEmployee = "SP_DeleteEmployee";
        public const string GetTechList = "SP_GetTechList";
        public const string GetStateListbyCountryId = "SP_GetStateListbyCountryId";
        public const string GetCityListbyStateId = "SP_GetCityListbyStateId";
        public const string GetCountrylist = "SP_GetCountrylist";
        public const string DataTableEmployeeList = "SP_DataTableEmployeeList";
        public const string GetEmployeebyId = "SP_GetEmployeebyId";
        public const string GetEmployeeCount = "SP_GetEmployeeCount";
        public const string GetCountryCount = "SP_GetCountryCount";
        public const string GetStateCount = "SP_GetStateCount";
        public const string GetCityCount = "SP_GetCityCount";
        #endregion
        public const string LoginData = "SP_GetUserData";
        public const string UpdateLoginDetails = "sp_SMS_Edit_Profile";
        public const string ChangePassword = "Sp_ChangePassword";
        public const string UpdateAdminPassword = "SP_ChangeAdminPassword";
    }
}
