using Shaligram_Recruitment.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Services.AdminPanel
{
    public interface IAdminPanelService
    {
        Task<SignInModel> AdminLogin(SignInModel login);
        Task<SignUpModel> GetUserDatabyEmail(string Email);
        Task<long> UpdateLoginToken(string Token, long UserId);
        Task<int> ValidateUserTokenData(int UserId, string jwtToken, DateTime TokenValidDate);
        Task<int> CheckUser(string email);
        //Task<int> InsertData(SignUpModel signup);
        Task<int> SaveOTP(int Code, int Userid);
        Task<int> UpdateOTP(int Code, int Userid);
        Task<int> VerifyOTP(int Otp, string Email);
        Task<int> UpdatePassword(string NewPassword, string email);
        Task<int> UpdateAdminPassword(string NewPassword, string email);
        Task<int> UpdateOTPbyEmail(string Email);
        Task<int> LogOut(string Email);
    }
}
