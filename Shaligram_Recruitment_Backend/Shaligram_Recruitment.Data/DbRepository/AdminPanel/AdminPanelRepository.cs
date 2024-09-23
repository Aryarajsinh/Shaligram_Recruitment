using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shaligram_Recruitment.Common.Helpers;
using Shaligram_Recruitment.Model.Config;
using Shaligram_Recruitment.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Data.DbRepository.AdminPanel
{
    public class AdminPanelRepository:BaseRepository, IAdminPanelRepository
    {
        #region Field
        public IConfiguration _Configuration;
        #endregion

        #region Constructor
        public AdminPanelRepository(IConfiguration configuration, IOptions<DataConfig> dataconfig) : base(dataconfig, configuration)
        {
            _Configuration = configuration;
        }
        #endregion
        #region UserLogin
        ///<summary>
        ///User Login 
        /// </summary>
        public async Task<SignInModel> AdminLogin(SignInModel login)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Email", login.Email);
            parameter.Add("@Password", login.Password);
            var data = await QueryFirstOrDefaultAsync<SignInModel>(StoredProcedures.UserLogin, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion

        #region Validate User Token Data
        ///<summary>
        ///Validate User Token Data
        /// </summary>
        public async Task<int> ValidateUserTokenData(int UserId, string jwtToken, DateTime TokenValidDate)
        {
            // Ensure TokenValidDate is in UTC
            if (TokenValidDate.Kind != DateTimeKind.Utc)
            {
                TokenValidDate = TimeZoneInfo.ConvertTimeToUtc(TokenValidDate);
            }

            var parameter = new DynamicParameters();
            parameter.Add("@userId", UserId);
            parameter.Add("@JWTToken", jwtToken);
            parameter.Add("@TokenValidateDate", TokenValidDate);

            // Log parameters for debugging
            Console.WriteLine($"Calling stored procedure with parameters: userId={UserId}, JWTToken={jwtToken}, TokenValidateDate={TokenValidDate}");

            // Call the stored procedure
            var data = await QueryFirstOrDefaultAsync<int>(StoredProcedures.ValidateToken, parameter, commandType: CommandType.StoredProcedure);

            // Log result for debugging
            Console.WriteLine($"Stored procedure returned: {data}");

            return data;
        }
        #endregion

        #region Update Login Token
        ///<summary>
        ///Update Login Token 
        /// </summary>
        public async Task<long> UpdateLoginToken(string Token, long UserId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@userId", UserId);
            parameter.Add("@Token", Token);
            var data = await QueryFirstOrDefaultAsync<long>(StoredProcedures.UpdateToken, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }

        public async Task<SignUpModel> GetUserDatabyEmail(string Email)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Email", Email);
            var data = await QueryFirstOrDefaultAsync<SignUpModel>(StoredProcedures.UserDataByEmail, parameter, commandType: CommandType.StoredProcedure);
            return data;

        }
        #endregion

        #region CheckUser
        public async Task<int> CheckUser(string email)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Email", email);
            var data = await QueryFirstOrDefaultAsync<int>(StoredProcedures.CheckUser, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion

        //#region InsertData
        //public async Task<int> InsertData(SignUpModel register)
        //{
        //    if (register == null)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        var parameter = new DynamicParameters();

        //        parameter.Add("@Firstname", (object)register.Firstname ?? DBNull.Value);
        //        parameter.Add("@Middlename", (object)register.Middlename ?? DBNull.Value);
        //        parameter.Add("@Lastname", register.Lastname);
        //        parameter.Add("@Email", register.Email);
        //        parameter.Add("@PhoneNumber", register.PhoneNumber);
        //        parameter.Add("@DateOfBirth", (object)register.Dob ?? DBNull.Value);
        //        parameter.Add("@AddressLineOne", register.AddressLineOne);
        //        parameter.Add("@AddressLineTwo", (object)register.AddressLineTwo ?? DBNull.Value);
        //        parameter.Add("@Country", register.Country);
        //        parameter.Add("@State", register.State);
        //        parameter.Add("@City", register.City);
        //        parameter.Add("@profilePicture ", (object)register.profilePhoto ?? DBNull.Value);
        //        parameter.Add("@Password ", register.Password);
        //        parameter.Add("@role", (object)register.role ?? DBNull.Value);
        //        parameter.Add("@Otp", (object)register.Otp ?? DBNull.Value);

        //        try
        //        {
        //            var data = await QueryFirstOrDefaultAsync<int>(StoredProcedures.InsertData, parameter, commandType: CommandType.StoredProcedure);
        //            return data;
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle exceptions (e.g., log the error)
        //            throw new Exception("An error occurred while fetching data.", ex);
        //        }
        //    }

        //}
        //#endregion 
        #region SaveOTP
        public async Task<int> SaveOTP(int Code, int Userid)
        {
            if (Userid == null && Code == null)
            {
                return 0;
            }
            else
            {
                var parameter = new DynamicParameters();

                parameter.Add("@Code", Code);
                parameter.Add("@Userid", Userid);

                try
                {
                    var data = await QueryFirstOrDefaultAsync<int>(StoredProcedures.SaveOTP, parameter, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while fetching data.", ex);
                }
            }
        }
        #endregion 

        #region UpdateOtp
        public async Task<int> UpdateOtp(int Code, int Userid)
        {
            if (Userid == null && Code == null)
            {
                return 0;
            }
            else
            {
                var parameter = new DynamicParameters();

                parameter.Add("@Code", Code);
                parameter.Add("@Userid", Userid);

                try
                {
                    var data = await QueryFirstOrDefaultAsync<int>(StoredProcedures.UpdateOTP, parameter, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while fetching data.", ex);
                }
            }
        }
        #endregion

        #region VarifyOtp
        public async Task<int> VerifyOtp(int Otp, string Email)
        {
            if (Otp == null && Email == null)
            {
                return 0;
            }
            else
            {
                var parameter = new DynamicParameters();

                parameter.Add("@Otp", Otp);
                parameter.Add("@Email", Email);

                try
                {
                    var data = await QueryFirstOrDefaultAsync<int>(StoredProcedures.VerifyOTP, parameter, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while fetching data.", ex);
                }
            }

        }
        #endregion

        #region UpdatePassword
        public async Task<int> UpdatePassword(string NewPassword, string email)
        {
            if (NewPassword == null && email == null)
            {
                return 0;
            }
            else
            {
                var parameter = new DynamicParameters();

                parameter.Add("@Newpassword", NewPassword);
                parameter.Add("@Email", email);

                try
                {
                    var data = await QueryFirstOrDefaultAsync<int>(StoredProcedures.ChangePassword, parameter, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while fetching data.", ex);
                }
            }

        }
        #endregion 
        #region UpdateAdminPassword
        public async Task<int> UpdateAdminPassword(string NewPassword, string email)
        {
            if (NewPassword == null && email == null)
            {
                return 0;
            }
            else
            {
                var parameter = new DynamicParameters();

                parameter.Add("@Newpassword", NewPassword);
                parameter.Add("@Email", email);

                try
                {
                    var data = await QueryFirstOrDefaultAsync<int>(StoredProcedures.UpdateAdminPassword, parameter, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while fetching data.", ex);
                }
            }

        }
        #endregion
        #region UpdatePassword
        public async Task<int> UpdateOTPbyEmail(string Email)
        {
            if (Email == null)
            {
                return 0;
            }
            else
            {
                var parameter = new DynamicParameters();

                parameter.Add("@Email", Email);

                try
                {
                    var data = await QueryFirstOrDefaultAsync<int>(StoredProcedures.UpdateOtpByEmail, parameter, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while fetching data.", ex);
                }
            }

        }
        #endregion  
        #region UpdatePassword
        public async Task<int> LogOut(string Email)
        {
            if (Email == null)
            {
                return 0;
            }
            else
            {
                var parameter = new DynamicParameters();

                parameter.Add("@Email", Email);

                try
                {
                    var data = await QueryFirstOrDefaultAsync<int>(StoredProcedures.LogOut, parameter, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while fetching data.", ex);
                }
            }

        }
        #endregion
    }
}
