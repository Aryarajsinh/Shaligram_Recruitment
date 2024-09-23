using Shaligram_Recruitment.Data.DbRepository.Questions;
using Shaligram_Recruitment.Data.DbRepository.StudentProfile;
using Shaligram_Recruitment.Model.ViewModels.Questions;
using Shaligram_Recruitment.Model.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Services.Questions
{
    public class QuestionServices : IQuestionServices
    {
        #region Feilds
        private readonly IQuestionRepository _questionServices;
        #endregion

        #region Constructor
        public QuestionServices(IQuestionRepository questionServices)
        {
            _questionServices = questionServices;
        }
        #endregion
        #region Add Student
        public async Task<int> QuestionAdd(QuestionModel model)
        {
            return await _questionServices.QuestionAdd(model);
        }
        #endregion 
    }
}
