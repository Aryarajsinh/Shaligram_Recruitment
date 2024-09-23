using Shaligram_Recruitment.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Data.DbRepository.AdminPanel
{
    public interface IAdminPanelRepository
    {
        Task<SignInModel> AdminLogin(SignInModel login);

        //Validate User Token Date
        Task<int> ValidateUserTokenData(int UserId, string jwtToken, DateTime TokenValidDate);

        //Update Login Token
        Task<long> UpdateLoginToken(string Token, long UserId);

        //Get UserData By Email
        Task<SignUpModel> GetUserDatabyEmail(string Email);

        Task<int> CheckUser(string email);
        //Task<int> InsertData(SignUpModel signup);
        Task<int> SaveOTP(int Code, int Userid);
        Task<int> UpdateOtp(int Code, int Userid);
        Task<int> VerifyOtp(int Otp, string Email);
        Task<int> UpdatePassword(string NewPassword, string email);
        Task<int> UpdateAdminPassword(string NewPassword, string email);
        Task<int> UpdateOTPbyEmail(string Email);
        Task<int> LogOut(string Email);
    }
}
