using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;
using Shaligram_Recruitment.Common.GlobalEnum;
using Shaligram_Recruitment.Common.Helpers;
using Shaligram_Recruitment.Common.Messages;
using Shaligram_Recruitment.Model.AppSetting;
using Shaligram_Recruitment.Model.SMTPSettings;
using Shaligram_Recruitment.Model.ViewModels;
using Shaligram_Recruitment.Model.ViewModels.Tokens;
using Shaligram_Recruitment.Services.AdminPanel;
using Shaligram_Recruitment.Services.JWTAuthentication;

namespace Shaligram_Recruitment.Controllers
{
    public class AdminPanelController : Controller
    {
        #region Fields
        private readonly IAdminPanelService _adminPanelServices;
        private IJWTAuthenticationService _jwtAuthenticationServices;
        private readonly AppSetting _appSettings;
        private readonly SMTPSettings _smtpSettings;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion

        #region Constructor
        public AdminPanelController(IWebHostEnvironment webHostEnvironment, IAdminPanelService adminPanelServices, IJWTAuthenticationService authenticationServices, IOptions<AppSetting> appSettings, IOptions<SMTPSettings> smtpSettings)
        {
            _adminPanelServices = adminPanelServices;
            _jwtAuthenticationServices = authenticationServices;
            _appSettings = appSettings.Value;
            _smtpSettings = smtpSettings.Value;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel signIn)
        {
            if (ModelState.IsValid)
            {
                var user = await _adminPanelServices.AdminLogin(signIn);
                if (user != null)
                {
                    UserTokenModel objTokenData = new UserTokenModel();
                    objTokenData.EmailId = user.Email;
                    objTokenData.UserId = user.Id != 0 ? user.Id : 0;
                    objTokenData.Firstname = user.FirstName;
                    AccessTokenModel objAccessTokenData = new AccessTokenModel();
                    objAccessTokenData = _jwtAuthenticationServices.GenerateTokenModel(objTokenData, _appSettings.JWTSecretKey, _appSettings.JWTValidityMinutes);
                    var data = await _adminPanelServices.UpdateLoginToken(objAccessTokenData.Token, objAccessTokenData.UserId);
                    string secretkey = _appSettings.JWTSecretKey;
                    int JWTTime = _appSettings.JWTValidityMinutes;
                    string JWTToken = objAccessTokenData.Token;
                    HttpContext.Session.SetString("_token", JWTToken);
                    HttpContext.Session.SetString("_Useremail", user.Email);
                    HttpContext.Session.SetString("_Firstname", user.FirstName);
                    HttpContext.Session.SetString("_UserId", Convert.ToString(user.Id));
                    //HttpContext.Session.SetString("_UserLogo", user.profilePhoto);
                    var checkstoredsession = HttpContext.Session.GetString("_token");
                    TempData["Msg"] = LoginMessages.LoginSuccessfully;
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    TempData["Login"] = LoginMessages.InvalidUser;
                    return View(signIn);
                }
            }
            else
            {
                return View(signIn);
            }
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Forgot Password Get Method
        /// </summary>
        #region ForgotPassword
        [HttpPost]

        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPassword)
        {
            var data = await _adminPanelServices.GetUserDatabyEmail(forgotPassword.Email);
            if (data != null)
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_smtpSettings.FromEmail);
                email.To.Add(MailboxAddress.Parse(data.Email));
                email.Subject = ForgotPasswordMessage.EmailSubject;
                var builder = new BodyBuilder();

                Random rand = new Random();
                int Code = rand.Next(100000, 1000000);

                await _adminPanelServices.UpdateOTP(Code, data.Id);

                string filePath = Directory.GetCurrentDirectory() + ".Common\\TemplateFormat\\ForgotPasswordEmail.html";
                string emailTemplateText = await GetEmailBody.GetEmailBodyText(filePath);
                emailTemplateText = string.Format(emailTemplateText, data.Email, Code);
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = emailTemplateText;
                var image = emailBodyBuilder.LinkedResources.Add(Directory.GetCurrentDirectory() + "\\wwwroot\\img\\shaligram.png");
                image.ContentId = "EmailLogo";

                email.Body = emailBodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(_smtpSettings.EmailHostName, Convert.ToInt32(_smtpSettings.EmailPort), SecureSocketOptions.StartTls);
                smtp.Authenticate(_smtpSettings.FromEmail, _smtpSettings.EmailAppPassword);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

                HttpContext.Session.SetString("email", data.Email);
                TempData["Email"] = data.Email;
                return RedirectToAction("Verification");
            }
            else
            {
                TempData["Error"] = ForgotPasswordMessage.EmailNotRegister;
                return View();
            }
        }
#endregion

        #region Verification
        /// <summary>
        /// OTP Verification Method
        /// </summary>

        public IActionResult Verification()
        {
            return View();
        }

        public async Task<JsonResult> VerificationOPT(OTPModel data)
        {
            int otp = data.OTP;
            string email = HttpContext.Session.GetString("email");
            var result = await _adminPanelServices.VerifyOTP(otp, email);
            var JsonString = JsonConvert.SerializeObject(result);

            return Json(JsonString);
        }
        #endregion

        #region Reset Password Method
        /// <summary>
        /// Reset Password Get Method
        /// </summary>

        public IActionResult ResetPassword()
        {
            return View();
        }

        /// <summary>
        /// Reset Password Post Method
        /// </summary>

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            string email = HttpContext.Session.GetString("email");
            var data = await _adminPanelServices.UpdatePassword(model.NewPassword, email);
            if (data == (int)GlobalEnum.Duplicate)
            {
                TempData["Password"] = changePasswordMessages.ChangePassword;
                return View();
            }
            else
            {
                TempData["Password"] = changePasswordMessages.PasswordChanges;
                return RedirectToAction("SignIn");
            }
        }
        /// <summary>
        /// User LogOut Method
        /// </summary>


        #endregion

        #region Update OTP By Email
        public async Task<JsonResult> UpdateOTPbyEmail()
        {
            string Email = TempData["Email"].ToString();
            var result = await _adminPanelServices.UpdateOTPbyEmail(Email);
            return Json(result);
        }
        #endregion

        public IActionResult LogOut()
        {
            TempData["Logout"] = LoginMessages.LogOut;
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn");
        }

    }
}
