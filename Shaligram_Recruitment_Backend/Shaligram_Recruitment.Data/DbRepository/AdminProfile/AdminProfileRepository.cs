using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shaligram_Recruitment.Common.Helpers;
using Shaligram_Recruitment.Model.Config;
using Shaligram_Recruitment.Model.ViewModels.UserProfile;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Data.DbRepository.AdminProfile
{
    public class AdminProfileRepository : BaseRepository, IAdminProfileRepository
    {
        #region Field
        public IConfiguration _Configuration;
        #endregion

        #region Constructor
        public AdminProfileRepository(IConfiguration configuration, IOptions<DataConfig> dataconfig) : base(dataconfig, configuration)
        {
            _Configuration = configuration;
        }
        #endregion
        #region Update User Profile
        ///<summary>
        ///Update User Profile
        /// </summary>
        public async Task<int> UpdateProfile(ProfileUpdateModel model)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@ID", model.Id);
            parameter.Add("@UserName", model.Firstname);
            parameter.Add("@LastName", model.Lastname);
            parameter.Add("@Email", model.Email);
            var data = await ExecuteAsync<int>(StoredProcedures.UpdateLoginDetails, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion

        #region Edit User Profile
        ///<summary>
        ///Edit Profile
        /// </summary>
        public async Task<ProfileUpdateModel> EditProfile(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Userid", id);
            var data = await QueryFirstOrDefaultAsync<ProfileUpdateModel>(StoredProcedures.LoginData, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion

        #region Change Password
        ///<summary>
        ///Change Password 
        /// </summary>
        public async Task<int> ChangePassword(ChangePasswordModel model)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Userid", model.Id);
            parameter.Add("@Old_Password", model.OldPassword);
            parameter.Add("@New_Password", model.NewPassword);
            var data = await QueryFirstOrDefaultAsync<int>(StoredProcedures.ResetPassword, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion
    }
}

