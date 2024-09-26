using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Eval;
using NPOI.SS.Formula.Functions;
using Shaligram_Recruitment.Common.CommonMethods;
using Shaligram_Recruitment.Common.EmailNotification;
using Shaligram_Recruitment.Common.Helpers;
using Shaligram_Recruitment.Model.AppSetting;
using Shaligram_Recruitment.Model.SMTPSettings;
using Shaligram_Recruitment.Model.ViewModels;
using Shaligram_Recruitment.Model.ViewModels.Tokens;
using Shaligram_Recruitment.Services.AdminPanel;
using Shaligram_Recruitment.Services.JWTAuthentication;
using System.Web;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static Shaligram_Recruitment.Common.EmailNotification.EmailNotification;
using static System.Net.WebRequestMethods;

namespace Shaligram_Recruitment_Api.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        #region Fields
        private IConfiguration _config;
        private readonly IAdminPanelService _adminPanelServices;
        private IJWTAuthenticationService _jwtAuthenticationServices;
        private readonly AppSetting _appSettings;
        private readonly SMTPSettings _smtpSettings;
        
        #endregion

        #region Constructor
        public AdminController(IConfiguration config, IAdminPanelService adminPanelServices, IJWTAuthenticationService authenticationServices, IOptions<AppSetting> appSettings, IOptions<SMTPSettings> smtpSettings)
        {
            _config = config;
            _adminPanelServices = adminPanelServices;
            _jwtAuthenticationServices = authenticationServices;
            _appSettings = appSettings.Value;
            _smtpSettings = smtpSettings.Value;

        }
        #endregion

// chane
        [HttpPost("login")]
        public async Task<ApiPostResponse<SignInModel>> LoginUser([FromBody] SignInModel model)
        {

            ApiPostResponse<SignInModel> response = new ApiPostResponse<SignInModel>() { Data = new SignInModel() };
            try
            {

                SignInModel result = await _adminPanelServices.AdminLogin(model);

                if (result != null && result.Id > 0 && result.Email==model.Email && result.Password==model.Password)
                {

                    UserTokenModel objTokenData = new UserTokenModel();
                    objTokenData.EmailId = model.Email;
                    objTokenData.UserId = result.Id != null ? result.Id : 0;
                    objTokenData.Firstname = result.FirstName;
                    HttpContext.Session.SetString("email", model.Email);
                    HttpContext.Session.SetInt32("id",result.Id != null ? result.Id : 0);
                  
                    Console.WriteLine(HttpContext.Session.GetString("email"));
                    AccessTokenModel objAccessTokenData = new AccessTokenModel();
                    Console.WriteLine(HttpContext.Session.GetString("id"));
                    objAccessTokenData = _jwtAuthenticationServices.GenerateTokenModel(objTokenData, _appSettings.JWTSecretKey, _appSettings.JWTValidityMinutes);
                    var data = await _adminPanelServices.UpdateLoginToken(objAccessTokenData.Token, objAccessTokenData.UserId);

                    response.Message = ErrorMessages.LoginSuccess;
                    response.Success = true;
                    //response.Data = EncryptionDecryption.GetEncrypt(JsonConvert.SerializeObject(result));
                    response.Data.Token = objAccessTokenData.Token;
                    response.Data.Id = result.Id;
                    response.Data.Email = result.FirstName;
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = ErrorMessages.InvalidCredential ;
                    return response;
                }



            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.SomethingWentWrong;
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return response;
            }
        }
        [HttpPost("forgot-password")]
        public async Task<BaseApiResponse> ForgetPassword([FromBody] ForgotPasswordModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            HttpContext.Session.SetString("UserEmail", model.Email);
            try
            {
                var result = await _adminPanelServices.GetUserDatabyEmail(model.Email);
                if (result != null)
                {
                    string EncryptedUserId = HttpUtility.UrlEncode(EncryptionDecryption.GetEncrypt(result.Id.ToString()));
                      
                    EmailNotification.EmailSetting setting = new EmailSetting
                    {
                        EmailEnableSsl = Convert.ToBoolean(_smtpSettings.EmailEnableSsl),
                        EmailHostName = _smtpSettings.EmailHostName,
                    EmailAppPassword = _smtpSettings.EmailAppPassword,
                        EmailPort = Convert.ToInt32(_smtpSettings.EmailPort),
                        FromEmail = _smtpSettings.FromEmail,
                        FromName = _smtpSettings.FromName,
                        EmailUsername = _smtpSettings.EmailUsername,
                    };
                    string RandomNumer = CommonMethods.GenerateNewRandom();
                    int OtpRandom = Convert.ToInt32(RandomNumer);

                    string emailBody = string.Empty;
                    string BasePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.ExceptionReportPath);

                    if (!Directory.Exists(BasePath))
                    {
                        Directory.CreateDirectory(BasePath);
                    }
                    bool isSuccess = false;

                    string LastForgetPasswordSend = Convert.ToString(String.Format("{0:yyyy-MM-dd HH:mm:ss}", result.Password));
                    string LastForgetPasswordDateTime = HttpUtility.UrlEncode(EncryptionDecryption.GetEncrypt(LastForgetPasswordSend));

                    using (StreamReader reader = new StreamReader(Path.Combine(BasePath, Constants.ForgetPasswordEmailtem)))
                    {
                        emailBody = reader.ReadToEnd();
                    }
                    string Client_URL = Convert.ToString(_config["Data:WebAppURL"]);
                    emailBody = emailBody.Replace("##userName##", result.Firstname);
                    emailBody = emailBody.Replace("##LogoURL##", Constants.logoUrl);
                    emailBody = emailBody.Replace("##OTP##", RandomNumer);
                    isSuccess = await Task.Run(() => SendMailMessage(model.Email, null, null, Constants.resetpasswordcode, emailBody, setting, null));
                    int issaveopt = await _adminPanelServices.SaveOTP(OtpRandom, result.Id);
                    //int issaveopt = await _adminPanelServices.VerifyOTP(OtpRandom, result.Email);

                    if (isSuccess == true && issaveopt == 1)
                    {
                        
                        response.Message = ErrorMessages.ForgetPasswordSuccess;
                        response.Success = true;
                    }
                    else
                    {
                        response.Message = ErrorMessages.ForgetPasswordError;
                        response.Success = false;
                        Response.StatusCode = StatusCodes.Status500InternalServerError;
                    }
                    return response;

                }
                else
                {
                    response.Message = ErrorMessages.UserError;
                    response.Success = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.SomethingWentWrong;
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return response;
            }

        }

        /// <summary>
		/// Verify Verafication code send to user
		/// </summary>
		/// <param name="model"></param>
		/// <returns>boolen</returns>
		[HttpPost("VerificationCode")]
        public async Task<BaseApiResponse> VerificationCode(OTPModel model)
        {   
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                int otp = model.OTP;
                string email = HttpContext.Session.GetString("UserEmail");
                var result = await _adminPanelServices.VerifyOTP(otp, model.Email);

                if (result == 1)
                {
                    response.Message = ErrorMessages.VerifyOPT;
                    response.Success = true;
                }
                else
                {
                    response.Message = ErrorMessages.NotVerifyOtp;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.SomethingWentWrong;
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return response;
            }
            return response;
        }

        /// <summary>
		/// Reset Account Password
		/// </summary>
		/// <param name="model"></param>
		/// <returns>Boolen</returns>
		[HttpPost("reset-password")]
        public async Task<BaseApiResponse> ResetPassword([FromBody] ResetPasswordModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                #region Validation 
                if (string.IsNullOrEmpty(model.Email))
                {
                    response.Message = ErrorMessages.EmailIsRequired;
                    response.Success = false;
                    return response;
                }
                if (!CommonMethods.IsValidEmail(model.Email))
                {
                    response.Message = ErrorMessages.EnterValidEmail;
                    response.Success = false;
                    return response;
                }
                if (string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.ConfirmPassword))
                {
                    response.Message = ErrorMessages.PasswordValidation;
                    response.Success = false;
                    return response;
                }
                if (model.NewPassword != model.ConfirmPassword)
                {
                    response.Message = ErrorMessages.ConfirmPassword;
                    response.Success = false;
                    return response;
                }
                #endregion
                string hashed = EncryptionDecryption.Hash(EncryptionDecryption.GetEncrypt(model.NewPassword));
                string[] segments = hashed.Split(":");
                string EncryptedHash = EncryptionDecryption.GetEncrypt(segments[0]);
                string EncryptedSalt = EncryptionDecryption.GetEncrypt(segments[1]);
         
                int result = await _adminPanelServices.UpdateAdminPassword(model.NewPassword, model.Email); ;
                if (result == 1)
                {
                    response.Message = ErrorMessages.ResetPasswordSuccess;
                    response.Success = true;
                }
                else
                {
                    response.Message = ErrorMessages.SomethingWentWrong;
                    response.Success = false;
                    Response.StatusCode = StatusCodes.Status403Forbidden;
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
        /// Verify Verafication code send to user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>boolen</returns>
         
        [HttpPost("Logout")]
        public async Task<BaseApiResponse> LogOut(LoginRequestModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                int result = await _adminPanelServices.LogOut(model.EmailId);

                if (result == 1)
                {
                    response.Message = ErrorMessages.LogOutSuccess;
                    response.Success = true;
                }
                else
                {
                    response.Message = ErrorMessages.SomethingWentWrong;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorMessages.SomethingWentWrong;
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return response;
            }
            return response;
        }


    }


}


