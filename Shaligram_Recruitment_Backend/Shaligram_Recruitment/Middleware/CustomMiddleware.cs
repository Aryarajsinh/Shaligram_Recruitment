
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



namespace Shaligram_Recruitment.Middleware
{
    public class CustomMiddleware
    {
        /// <summary>
        /// Feilds
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly SMTPSettings _smtpClient;


        /// <summary>
        /// Counstuctor
        /// </summary>
        public CustomMiddleware(RequestDelegate next,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IJWTAuthenticationService jwtAuthenticationService, IOptions<SMTPSettings> options)
        {
            _next = next;
            _hostingEnvironment = hostingEnvironment;
            _jwtAuthenticationService = jwtAuthenticationService;
            _smtpClient = options.Value;
        }
        /// <summary>
        /// Middeleware Invoke Method
        /// </summary>
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
                DeleteOldReqResLogFiles();
                // Check JWT Token validity expiry
                string jwtToken = context.Session.GetString("_token");
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    UserTokenModel userTokenModel = _jwtAuthenticationService.GetUserTokenData(jwtToken);
                    var result = await _accountService.ValidateUserTokenData(userTokenModel.UserId, jwtToken, userTokenModel.TokenValidTo);
                    if (result == (int)GlobalEnum.Success)
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
                AddReqResLogsToLoggerFile(context);
                await _next(context);
            }
            catch (Exception ex)
            {
                //if(ex.Message == '@TempData["Exception"]')
                //{

                //}
                bool success = await SendExceptionEmail(ex, context);
                //AddExceptionLogsToFiles(ex, context, success);
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
        /// Add error logs in folder 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="context"></param>
        public void AddExceptionLogsToFiles(Exception ex, HttpContext context, bool success)
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "ExceptionLogs");
            var exfilePath = Path.Combine(_hostingEnvironment.WebRootPath, "ExceptionLogs", "Exception_" + Path.GetFileName(DateTime.Now.ToString("dd_MM_yyyy") + ".txt"));
            string jsonLst = CommonMethods.GetKeyValues(context);
            if (!Directory.Exists(path))
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
            sw.WriteLine("Error Message: " + ex.Message);
            sw.WriteLine("InnerException: " + ex.InnerException);
            sw.WriteLine("StackTrace: " + ex.StackTrace);
            sw.WriteLine("Request Params: " + jsonLst);
            if (ex.InnerException != null)
            {
                sw.WriteLine("Exception: " + ex.InnerException.InnerException);
            }
        }
        /// <summary>
        /// Send Exception
        /// </summary>
        public async Task<bool> SendExceptionEmail(Exception ex, HttpContext context)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_smtpClient.FromEmail),
                Subject = "Error Email"
            };
            email.To.Add(MailboxAddress.Parse("harmaaryarajsinh2003@gmail.com"));

            var builder = new BodyBuilder
            {
                HtmlBody = $"<h2>Requested URL : {context.Request.Path.Value}</h2><br>" +
                           $"<h2>Exception : {ex.Message}</h2><br>" +
                           $"<h2>InnerException : {ex.InnerException?.Message}</h2><br>"
            };
            email.Body = builder.ToMessageBody();

            try
            {
                using var smtp = new SmtpClient();
                var useSsl = bool.Parse(_smtpClient.EmailEnableSsl); // Ensure this is converted correctly
                var port = Convert.ToInt32(_smtpClient.EmailPort);

                await smtp.ConnectAsync(_smtpClient.EmailHostName, port, useSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto);
                await smtp.AuthenticateAsync(_smtpClient.FromEmail, _smtpClient.EmailAppPassword);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception smtpEx)
            {
                // Log the exception using your logging mechanism
                Console.WriteLine($"SMTP Exception: {smtpEx.Message}");
                return false;
            }  
        }
        private void AddReqResLogsToLoggerFile(HttpContext context)
        {
            try
            {
                var exfilePath = Path.Combine(_hostingEnvironment.WebRootPath, "ReqResLogs", "ReqResLog_" + Path.GetFileName(DateTime.Now.ToString("dd_MM_yyyy") + ".txt"));
                string jsonLst = CommonMethods.GetKeyValues(context);
                if (!File.Exists(exfilePath))
                {
                    var myFile = File.Create(exfilePath);
                    myFile.Close();
                }
                using StreamWriter sw = File.AppendText(exfilePath);
                sw.WriteLine("");
                sw.WriteLine("--------------------------------- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ----------------------------------");
                sw.WriteLine("Requested URL: " + context.Request.Path.Value);
                sw.WriteLine("Request Params: " + jsonLst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
