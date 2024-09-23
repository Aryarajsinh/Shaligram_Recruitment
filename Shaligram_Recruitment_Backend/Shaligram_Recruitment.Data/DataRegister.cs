using Shaligram_Recruitment.Data.DbRepository.AdminPanel;
using Shaligram_Recruitment.Data.DbRepository.AdminProfile;
using Shaligram_Recruitment.Data.DbRepository.Questions;
using Shaligram_Recruitment.Data.DbRepository.StudentProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Data
{
    public class DataRegister
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var dictionary = new Dictionary<Type, Type>()
            {
                { typeof(IAdminPanelRepository),typeof(AdminPanelRepository)},
                { typeof(IAdminProfileRepository),typeof(AdminProfileRepository)},
                { typeof(IStudentProfileRepository),typeof(StudentProfileRepository)},
                { typeof(IQuestionRepository),typeof(QuestionRepository)},
              
            };
            return dictionary;
        }
    }
}
