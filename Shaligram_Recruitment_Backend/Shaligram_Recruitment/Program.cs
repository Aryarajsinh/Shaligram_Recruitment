using Shaligram_Recruitment.Model.Config;
using Shaligram_Recruitment;
using Shaligram_Recruitment.Logger;
using Shaligram_Recruitment.Model.AppSetting;
using Shaligram_Recruitment.Model.SMTPSettings;
using Shaligram_Recruitment.Services.JWTAuthentication;
using Shaligram_Recruitment.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<DataConfig>(builder.Configuration.GetSection("ConnectionStrings"));
RegisterServices.RegisterService(builder.Services);
builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("SMTPSettings"));
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSingleton<IJWTAuthenticationService, JWTAuthenticationService>();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("SmtpClient"));
var jwtSettings = builder.Configuration.GetSection("AppSettings").Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true, // Ensures that the token expiration is validated
        ValidateIssuerSigningKey = true, // Validates the secret key used to sign the token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.JWTSecretKey)),
        ClockSkew = TimeSpan.Zero // Override the default clock skew of 5 minutes
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseMiddleware<CustomMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AdminPanel}/{action=SignIn}/{id?}");

app.Run();

public class JwtSettings
{
    public string JWTSecretKey { get; set; }
    public int JWTValidityMinutes { get; set; }
}