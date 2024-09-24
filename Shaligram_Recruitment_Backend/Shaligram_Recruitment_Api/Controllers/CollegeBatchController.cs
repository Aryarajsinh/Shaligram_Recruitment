using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shaligram_Recruitment.Common.Helpers;
using Shaligram_Recruitment.Model.ViewModels;
using Shaligram_Recruitment.Model.ViewModels.CollegeBatch;
using Shaligram_Recruitment.Model.ViewModels.Pagination;
using Shaligram_Recruitment.Services.StudentProfile;

namespace Shaligram_Recruitment_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollegeBatchController : ControllerBase
    {
        private readonly IStudentProfileService _collegeBatchService;

        public CollegeBatchController(IStudentProfileService studentProfileService)
        {
            _collegeBatchService = studentProfileService;
        }

        [HttpPost("collegebatch-list")]
        public async Task<ApiResponse<CollegeBatchModel>> CollegebatchList(string search = "", int page = 1, int pageSize = 10, string sortColumn = "batchId", string sortDirection = "asc")
        {
            ApiResponse<CollegeBatchModel> response = new ApiResponse<CollegeBatchModel> { Data = new List<CollegeBatchModel>() };
            try
            {
                sortDirection = (sortDirection.ToLower() == "desc") ? "desc" : "asc";
                PagedResult CollegeList = await _collegeBatchService.GetPageCollegeBatch(search, page, pageSize, sortColumn, sortDirection);
                if (CollegeList != null)
                {
                    response.Data = CollegeList.CollegeBatch;
                    response.TotalRecords = CollegeList.TotalRecords;
                    response.Success = true;
                    response.Message = ErrorMessages.CollegeBatchList;
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
        [HttpGet("collegebatch-full-list")]
        public async Task<ApiResponse<CollegeBatchModel>> CollegebatchFullList()
        {
            ApiResponse<CollegeBatchModel> response = new ApiResponse<CollegeBatchModel> { Data = new List<CollegeBatchModel>() };
            try
            {
                List<CollegeBatchModel> studentList = await _collegeBatchService.GetCollegeBatchList();
                if (studentList != null && studentList.Count > 0)
                {
                    response.Data = studentList;
                    response.Success = true;
                    response.Message = ErrorMessages.CollegeBatchList;
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
        [HttpPost("AddEditCollegebatch")]
        public async Task<BaseApiResponse> AddEditCollegebatch([FromBody] CollegeBatchModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                long result = await _collegeBatchService.CollegeBatchAdd(model);
                if (result == 1)
                {
                    if (model.BatchId > 0)
                    {
                        response.Success = true;
                        response.Message = ErrorMessages.EditCollegeBatch;
                    }
                    else
                    {
                        response.Success = true;
                        response.Message = ErrorMessages.AddCollegeBatch;
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
        /// <summary>
        /// Soft Delete Employee In Database
        /// </summary>
        /// <param name="BatchId"></param>
        /// <returns></returns>
        [HttpDelete("delete-collegebatch/{BatchId}")]
        public async Task<BaseApiResponse> DeleteCollegebatch(int BatchId)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                long result = await _collegeBatchService.DeleteBatch(BatchId);
                if (result == 1)
                {
                    response.Success = true;
                    response.Message = ErrorMessages.DeleteCollegeBatchSuccess;
                }
                else
                {
                    response.Success = true;
                    response.Message = ErrorMessages.DeleteCollegeBatchFails;
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
        /// <param name="BatchId"></param>
        /// <returns></returns>
        [HttpGet("collegebatch-details-by-id/{BatchId}")]
        public async Task<ApiPostResponse<CollegeBatchModel>> GetCollegebatchById(int BatchId)
        {
            ApiPostResponse<CollegeBatchModel> response = new ApiPostResponse<CollegeBatchModel>();
            try
            {
                var result = await _collegeBatchService.BatchDetails(BatchId);
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
