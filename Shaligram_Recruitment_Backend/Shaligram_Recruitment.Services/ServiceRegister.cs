using Shaligram_Recruitment.Services.AdminPanel;
using Shaligram_Recruitment.Services.AdminProfile;
using Shaligram_Recruitment.Services.JWTAuthentication;
using Shaligram_Recruitment.Services.Questions;
using Shaligram_Recruitment.Services.StudentProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Services
{
    public class ServiceRegister
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var ServiceDictionary = new Dictionary<Type, Type>()
            {
                {typeof(IAdminPanelService),typeof(AdminPanelService) },
                {typeof(IAdminProfileService),typeof(AdminProfileService) },
                {typeof(IJWTAuthenticationService),typeof(JWTAuthenticationService)},
                {typeof(IStudentProfileService),typeof(StudentProfileService)},
                {typeof(IQuestionServices),typeof(QuestionServices)}
            };
            return ServiceDictionary;
        }
    }
}
