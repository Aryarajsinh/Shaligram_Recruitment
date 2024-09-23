using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shaligram_Recruitment.Common.Helpers;
using Shaligram_Recruitment.Model.ViewModels.Student;
using Shaligram_Recruitment.Model.ViewModels;
using Shaligram_Recruitment.Services.StudentProfile;
using Shaligram_Recruitment.Model.ViewModels.QuestionSet;

namespace Shaligram_Recruitment_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionSetController : ControllerBase
    {
        private readonly IStudentProfileService _questionSetService;

        public QuestionSetController(IStudentProfileService studentProfileService)
        {
            _questionSetService = studentProfileService;
        }
        [HttpPost("questionSet-list")]
        public async Task<ApiResponse<QuestionSetModel>> QuestionSetList(CommonPaginationModel model)
        {
            ApiResponse<QuestionSetModel> response = new ApiResponse<QuestionSetModel> { Data = new List<QuestionSetModel>() };
            try
            {
                List<QuestionSetModel> studentList = await _questionSetService.GetQuestionSet();
                if (studentList != null && studentList.Count > 0)
                {
                    response.Data = studentList;
                    response.Success = true;
                    response.Message = ErrorMessages.QuestionSetList;
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
        [HttpGet("questionSet-full-list")]
        public async Task<ApiResponse<QuestionSetModel>> QuestionSetFullList()
        {
            ApiResponse<QuestionSetModel> response = new ApiResponse<QuestionSetModel> { Data = new List<QuestionSetModel>() };
            try
            {
                List<QuestionSetModel> studentList = await _questionSetService.GetQuestionSet();
                if (studentList != null && studentList.Count > 0)
                {
                    response.Data = studentList;
                    response.Success = true;
                    response.Message = ErrorMessages.QuestionSetList;
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
        [HttpPost("add-edit-questionSet")]
        public async Task<BaseApiResponse> AddEditquestionSet([FromBody]QuestionSetModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                long result = await _questionSetService.QuestionSetAdd(model);
                if (result == 1)
                {
                    if (model.QuestionSetId > 0)
                    {
                        response.Success = true;
                        response.Message = ErrorMessages.EditQuestionSet;
                    }
                    else
                    {
                        response.Success = true;
                        response.Message = ErrorMessages.AddQuestionSet;
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
        /// <param name="StudentId"></param>
        /// <returns></returns>
        [HttpDelete("delete-questionSet/{QuestionSetId}")]
        public async Task<BaseApiResponse> DeleteEmployee(int QuestionSetId)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                long result = await _questionSetService.DeleteQuestionSet(QuestionSetId);
                if (result == 1)
                {
                    response.Success = true;
                    response.Message = ErrorMessages.DeleteQuestionSetSuccess;
                }
                else
                {
                    response.Success = false;
                    response.Message = ErrorMessages.DeleteQuestionSetFails;
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
        /// <param name="QuestionSetId"></param>
        /// <returns></returns>
        [HttpGet("questionSet-details-by-id/{QuestionSetId}")]
        public async Task<ApiPostResponse<QuestionSetModel>> GetQuestionSetById(int QuestionSetId)
        {
            ApiPostResponse<QuestionSetModel> response = new ApiPostResponse<QuestionSetModel>();
            try
            {
                var result = await _questionSetService.QuestionSetDetails(QuestionSetId);
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
