
using Microsoft.Extensions.Options;
using MimeKit;
using Shaligram_Recruitment.Common.CommonMethods;
using Shaligram_Recruitment.Common.GlobalEnum;
using Shaligram_Recruitment.Model.SMTPSettings;
using Shaligram_Recruitment.Model.ViewModels.Tokens;
using Shaligram_Recruitment.Services.AdminPanel;
using Shaligram_Recruitment.Services.JWTAuthentication;
using System.Net.Mail;
using System.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MimeKit.IO;
using MailKit.Security;
using Microsoft.Net.Http.Headers;
using Shaligram_Recruitment.Common.Helpers;
using System.Net;
using Shaligram_Recruitment_Api.Logger;



namespace Shaligram_Recruitment_Api.Middleware
{
    public class CustomMiddleware
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private readonly RequestDelegate _next;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly SMTPSettings _smtpSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public CustomMiddleware(RequestDelegate next,
            IHttpContextAccessor httpContextAccessor,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
            IJWTAuthenticationService jwtAuthenticationService,
            IOptions<SMTPSettings> smtpSettings,
            ILoggerManager logger)
        {
            _next = next;
            _hostingEnvironment = hostingEnvironment;
            _jwtAuthenticationService = jwtAuthenticationService;
            _smtpSettings = smtpSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        #endregion

        #region Methods
        public async Task Invoke(HttpContext context, IAdminPanelService _accountService)
        {
            try
            {
                var HttpContextBody = context.Request.Body;
                string requestBody = string.Empty;

                context.Request.EnableBuffering();

                using (var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: -1,
                    leaveOpen: true))
                {
                    var body = await reader.ReadToEndAsync();

                    context.Request.Headers.Add("requestModel", body);
                    context.Request.Body.Position = 0;
                }
                // Delete files from folder for logs of request and response older than 7 days.
                //DeleteOldReqResLogFiles();
                var token = context.Request.Headers[HeaderNames.Authorization].ToString();
                // Check JWT Token validity expiry
                if (!string.IsNullOrEmpty(token))
                {
                    string jwtToken = context.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

                    UserTokenModel userTokenModel = _jwtAuthenticationService.GetUserTokenData(jwtToken);
                    var result = await _accountService.ValidateUserTokenData(userTokenModel.UserId, jwtToken, userTokenModel.TokenValidTo);
                    if (result == 5)
                    {
                        if (userTokenModel != null)
                        {
                            if (userTokenModel.TokenValidTo < DateTime.UtcNow.AddMinutes(1))
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                return;
                            }
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return;
                    }

                }
                //AddReqResLogsToLoggerFile(context);
                await _next(context);

            }
            catch (Exception ex)
            {

                // Add error logs in folder
                bool success = await SendExceptionEmail(ex, context);
                AddExceptionLogsToFiles(ex, context, success);
                await HandleExceptionAsync(context, ex);
            }

            }

        /// <summary>
        /// Delete files from error logs folder which is older than 7 days.
        /// </summary>
        public void DeleteOldReqResLogFiles()
        {
            var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "ReqResLogs");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string[] files = Directory.GetFiles(directoryPath);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.LastAccessTime < DateTime.Now.AddDays(-7))
                {
                    fi.Delete();
                }
            }
        }
        /// <summary>
        /// Add Exception to log file
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="context"></param>
        /// <param name="success"></param>
        public void AddExceptionLogsToFiles(Exception ex, HttpContext context, bool success)
        {
            //string DirectoryPath = "/Logs/";
            //var BasePath = Path.Combine(_hostingEnvironment.WebRootPath, DirectoryPath);

            try
            {
                var exfilePath = Path.Combine(_hostingEnvironment.WebRootPath, "ExceptionLogs", "Execption_" + Path.GetFileName(DateTime.Now.ToString("dd_MM_yyyy") + ".txt"));
                string path = Path.Combine(_hostingEnvironment.WebRootPath, "ExceptionLogs");

                //var FileName = Path.GetExtension(DateTime.Now.ToString("dd_MM_yyyy"));
                if (!(Directory.Exists(path)))
                {
                    Directory.CreateDirectory(path);
                }

                if (!File.Exists(exfilePath))
                {
                    var myFile = File.Create(exfilePath);
                    myFile.Close();
                }

                using StreamWriter sw = File.AppendText(exfilePath);
                sw.WriteLine("");
                sw.WriteLine("--------------------------------- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ----------------------------------");
                sw.WriteLine("Requested URL: " + context.Request.Path.Value);
                sw.WriteLine("Exception: " + ex.Message);
                sw.WriteLine("InnerException: " + ex.InnerException);
                sw.WriteLine("Exception Email sent: " + success);
                if (ex.InnerException != null)
                {
                    sw.WriteLine("Exception: " + ex.InnerException.InnerException);
                }
            }
            catch (Exception ex1)
            {
                throw ex1;
            }
        }

        /// <summary>
        /// Send exception email to admin
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> SendExceptionEmail(Exception ex, HttpContext context)
        {

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_smtpSettings.FromEmail);
            email.To.Add(MailboxAddress.Parse(Constants.adminEmailAddress));
            email.Subject = "Error Email";
            var builder = new BodyBuilder();
            builder.HtmlBody = $"<h2>Requested URL : {context.Request.Path.Value}</h2><br><h2>Execption : {ex.Message}</h2><br><h2>InnerException : {ex.InnerException}</h2><br> ";
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_smtpSettings.EmailHostName, Convert.ToInt32(_smtpSettings.EmailPort), SecureSocketOptions.StartTls);
            smtp.Authenticate(_smtpSettings.FromEmail, _smtpSettings.EmailAppPassword);
            var success = await smtp.SendAsync(email);
            smtp.Disconnect(true);
            return true;
        }

        /// <summary>
        /// Handle Exception for middleware
        /// </summary>
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(new BaseApiResponse()
            {
                Success = false,
                Message = exception.Message
            }.ToString());

        }


        /// <summary>
        /// Add Request response log to log file
        /// </summary>
        /// <param name="context"></param>
        //private async void AddReqResLogsToLoggerFile(HttpContext context)
        //{
        //	try
        //	{
        //		var exfilePath = Path.Combine(_hostingEnvironment.WebRootPath, "ReqResLogs", "ReqResLog_" + Path.GetFileName(DateTime.Now.ToString("dd_MM_yyyy") + ".txt"));

        //		//var FileName = Path.GetExtension(DateTime.Now.ToString("dd_MM_yyyy"));
        //		if (!File.Exists(exfilePath))
        //		{
        //			var myFile = File.Create(exfilePath);
        //			myFile.Close();
        //		}
        //		using StreamWriter sw = File.AppendText(exfilePath);
        //		sw.WriteLine("");
        //		sw.WriteLine("--------------------------------- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ----------------------------------");
        //		sw.WriteLine("Requested URL: " + context.Request.Path.Value);
        //		sw.WriteLine("Context request: " + context.Request.ContentType);
        //	}
        //	catch (Exception ex)
        //	{
        //		throw ex;
        //	}
        //}
        #endregion
    }
}
