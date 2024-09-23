using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shaligram_Recruitment.Model.AppSetting;
using Shaligram_Recruitment.Model.Config;
using Shaligram_Recruitment.Model.SMTPSettings;
using Shaligram_Recruitment.Services.JWTAuthentication;
using Shaligram_Recruitment_Api;
using Shaligram_Recruitment_Api.Logger;
using Shaligram_Recruitment_Api.Middleware;
using System.ComponentModel;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make session cookie HTTP only
    options.Cookie.IsEssential = true; // Ensure the cookie is always sent
});

builder.Services.AddControllers();

builder.Services.AddDistributedMemoryCache(); // Adds a default in-memory cache

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Shaligam Sample Codes API",
        Description = "Shaligam Sample Codes .NET Core Web API"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

RegisterServices.RegisterService(builder.Services);
builder.Services.Configure<DataConfig>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("SMTPSettings"));
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSingleton<IJWTAuthenticationService, JWTAuthenticationService>();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllRequests", builder =>
    {
        builder.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .WithExposedHeaders("content-disposition");
    });
});
var app = builder.Build();
app.UseCors("AllRequests");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Shaligam Sample Codes API v1");
    });
}
app.UseHttpsRedirection();
app.UseSession();
app.UseMiddleware<CustomMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();

app.Run();
