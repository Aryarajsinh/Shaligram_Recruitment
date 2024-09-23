using Shaligram_Recruitment.Model.ViewModels;
using Shaligram_Recruitment.Model.ViewModels.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Data.DbRepository.AdminProfile
{
    public interface IAdminProfileRepository
    {

        //for Update Profile
        Task<int> UpdateProfile(ProfileUpdateModel model);

        //for Edit Profile
        Task<ProfileUpdateModel> EditProfile(int id);


        //For Change Password
        Task<int> ChangePassword(ChangePasswordModel model);
    }
}
