using Shaligram_Recruitment.Data.DbRepository.AdminPanel;
using Shaligram_Recruitment.Data.DbRepository.AdminProfile;
using Shaligram_Recruitment.Model.ViewModels;
using Shaligram_Recruitment.Services.AdminProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Services.AdminPanel
{
    public class AdminPanelService: IAdminPanelService
    {
        #region Feilds
        private readonly IAdminPanelRepository _adminPanelServices; 
        #endregion

        #region Constructor
        public AdminPanelService(IAdminPanelRepository userPanelRepository)
        {
            _adminPanelServices = userPanelRepository;          
        }
        #endregion
        public async Task<SignInModel> AdminLogin(SignInModel login)
        {
            return await _adminPanelServices.AdminLogin(login);
        }
        public async Task<SignUpModel> GetUserDatabyEmail(string Email)
        {
            return await _adminPanelServices.GetUserDatabyEmail(Email);
        }
        public async Task<int> ValidateUserTokenData(int UserId, string jwtToken, DateTime TokenValidDate)
        {
            return await _adminPanelServices.ValidateUserTokenData(UserId, jwtToken, TokenValidDate);
        }
        public async Task<long> UpdateLoginToken(string Token, long UserId)
        {
            return await _adminPanelServices.UpdateLoginToken(Token, UserId);
        }
        public async Task<int> CheckUser(string email)
        {
            return await _adminPanelServices.CheckUser(email);
        }
        //public async Task<int> InsertData(SignUpModel signup)
        //{
        //    return await _adminPanelServices.InsertData(signup);
        //}
        public async Task<int> SaveOTP(int Code, int Userid)
        {
            return await _adminPanelServices.SaveOTP(Code, Userid);
        }
        public async Task<int> UpdateOTP(int Code, int Userid)
        {
            return await _adminPanelServices.UpdateOtp(Code, Userid);
        }
        public async Task<int> VerifyOTP(int Otp, string Email)
        {
            return await _adminPanelServices.VerifyOtp(Otp, Email);
        }
        public async Task<int> UpdatePassword(string NewPassword, string email)
        {
            return await _adminPanelServices.UpdatePassword(NewPassword, email);
        } 
        public async Task<int> UpdateAdminPassword(string NewPassword, string email)
        {
            return await _adminPanelServices.UpdateAdminPassword(NewPassword, email);
        }
        public async Task<int> UpdateOTPbyEmail(string Email)
        {
            return await _adminPanelServices.UpdateOTPbyEmail(Email);
        }
        public async Task<int> LogOut(string Email)
        {
            return await _adminPanelServices.LogOut(Email);
        }

     
    }
}
