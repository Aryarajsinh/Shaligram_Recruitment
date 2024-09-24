using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Shaligram_Recruitment.Common.Helpers;
using Shaligram_Recruitment.Model.ViewModels;
using Shaligram_Recruitment.Model.ViewModels.CollegeBatch;
using Shaligram_Recruitment.Model.ViewModels.Pagination;
using Shaligram_Recruitment.Model.ViewModels.Student;
using Shaligram_Recruitment.Services.StudentProfile;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shaligram_Recruitment_Api.Controllers
{
    [Route("api/Student")]
    [ApiController]
    
    public class StudentController : ControllerBase
    {
        private readonly IStudentProfileService _studentProfileService;

        public StudentController(IStudentProfileService studentProfileService)
        {
            _studentProfileService = studentProfileService;
        }

        /// <summary>
        /// Get student List With Server side pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns>list</returns>
        [HttpPost("student-list")]
        public async Task<ApiPostResponse<List<StudentModel>>> studentList(ServerSidePage model)
        {            
            ApiPostResponse<List<StudentModel>> response = new ApiPostResponse<List<StudentModel>>();
            try
            {
                model.sortDirection = (model.sortDirection.ToLower() == "desc") ? "desc" : "asc";
                PagedResult studentList = await _studentProfileService.GetPagedUsers(model.search, model.page, model.pageSize, model.sortColumn, model.sortDirection);
                if (studentList != null)
                {
                    response.Data = studentList.Users;
                    response.TotalRecords = studentList.TotalRecords;
                    response.Success = true;
                    response.Message = ErrorMessages.StudentList;
                }
                else
                {
                    response.Success = false;
                    response.Message = ErrorMessages.SomethingWentWrong;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.SomethingWentWrong;
                Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            return response;
        }
        /// <summary>
        /// Get Employee Full List Without pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet("getstudentlist")]
        
        public async Task<ApiResponse<StudentModel>> studentFullList()
        {
            ApiResponse<StudentModel> response = new ApiResponse<StudentModel> { Data = new List<StudentModel>() };
            try
            {
                List<StudentModel> studentList = await _studentProfileService.StudentList();
                if (studentList != null && studentList.Count > 0)
                {
                    response.Data = studentList;
                    response.Success = true;
                    response.Message = ErrorMessages.StudentList;
                }
                else
                {
                    response.Success = false;
                    response.Message = ErrorMessages.SomethingWentWrong;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.SomethingWentWrong;
                Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            return response;
        } /// <summary>
        /// Get Employee Full List Without pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCollegeName")]
        public async Task<ApiResponse<CollegeBatchModel>> GetCollegeName()
        {
            ApiResponse<CollegeBatchModel> response = new ApiResponse<CollegeBatchModel> { Data = new List<CollegeBatchModel>() };
            try
            {
                List<CollegeBatchModel> CollegeName = await _studentProfileService.GetCollegeName();
                if (CollegeName != null && CollegeName.Count > 0)
                {
                    response.Data = CollegeName;
                    response.Success = true;
                    response.Message = ErrorMessages.StudentList;
                }
                else
                {
                    response.Success = false;
                    response.Message = ErrorMessages.SomethingWentWrong;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.SomethingWentWrong;
                Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            return response;
        } /// <summary>
        /// Get Employee Full List Without pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBatchYears")]
        public async Task<ApiResponse<CollegeBatchModel>> GetBatchYears()
        {
            ApiResponse<CollegeBatchModel> response = new ApiResponse<CollegeBatchModel> { Data = new List<CollegeBatchModel>() };
            try
            {
                List<CollegeBatchModel> BatchYear = await _studentProfileService.GetBetchYear();
                if (BatchYear != null && BatchYear.Count > 0)
                {
                    response.Data = BatchYear;
                    response.Success = true;
                    response.Message = ErrorMessages.StudentList;
                }
                else
                {
                    response.Success = false;
                    response.Message = ErrorMessages.SomethingWentWrong;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.SomethingWentWrong;
                Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            return response;
        }
        /// <summary>
        /// Add Edit Employee details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("AddEditStudent")]
        public async Task<BaseApiResponse> AddEditStudent(StudentModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                long result = await _studentProfileService.StudentAdd(model);
                if (result == 1)
                {
                    if (model.StudentId > 0)
                    {
                        response.Success = true;
                        response.Message = ErrorMessages.EditStudent;
                    }
                    else
                    {
                        response.Success = true;
                        response.Message = ErrorMessages.AddStudent;
                    }
                }
                else if (result == 2)
                {
                    response.Success = false;
                    response.Message = ErrorMessages.EmailExists;
                }
                else
                {
                    response.Success = false;
                    response.Message = ErrorMessages.SomethingWentWrong;
                    Response.StatusCode = StatusCodes.Status500InternalServerError;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.SomethingWentWrong;
                Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        [HttpPost("UploadFile")]
        public async Task<BaseApiResponse> UploadFile(CollegeBatchModel db)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {

                // Attempt to parse the uploaded CSV file
                var records = _studentProfileService.ParseCsvFile(db.UploadFile);
                

                // Call the service method to upload the parsed data to the database
                var data=await _studentProfileService.uploadFileData(records,db);
                if (data != 0)
                {
                    response.Success = true;
                    response.Message = ErrorMessages.EditStudent;
                }
              
                else
                {
                    response.Success = false;
                    response.Message = "This Data Already Exist";
                    //response.Success = false;
                    //response.Message = "Invalid file or form data. Please upload the correct CSV file.";

                }
            }
            catch (CsvHelperException ex)
            {
                // Handle CSV parsing exceptions
                response.Success = false;
                response.Message = "Error in parsing the CSV file. Please check the file format.";

            }
            catch (Exception ex)
            {
                // Handle general exceptions
                response.Success = false;
                response.Message = "An error occurred while uploading the file. Please try again.";

            }
            return response;
        }
        /// <summary>
        /// Soft Delete Employee In Database
        /// </summary>
        /// <param name="StudentId"></param>
        /// <returns></returns>
        [HttpDelete("delete-student/{StudentId}")]
        public async Task<BaseApiResponse> DeleteEmployee(int StudentId)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                long result = await _studentProfileService.DeleteStudent(StudentId);
                if (result == 1)
                {
                    response.Success = true;
                    response.Message = ErrorMessages.DeleteStudentSuccess;
                }
                else
                {
                    response.Success = true;
                    response.Message = ErrorMessages.DeleteStudentFails;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.SomethingWentWrong;
                Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        /// <summary>
        /// Get Employee By Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("student-details-by-id/{StudentId}")]
        public async Task<ApiPostResponse<StudentModel>> GetEmployeeById(int StudentId)
        {
            ApiPostResponse<StudentModel> response = new ApiPostResponse<StudentModel>();
            try
            {
                var result = await _studentProfileService.StudentDetails(StudentId);
                if (result != null)
                {
                    response.Success = true;
                    response.Data = result;
                }
                else
                {
                    response.Success = false;
                    response.Message = ErrorMessages.SomethingWentWrong;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.SomethingWentWrong;
                Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

    }
}

