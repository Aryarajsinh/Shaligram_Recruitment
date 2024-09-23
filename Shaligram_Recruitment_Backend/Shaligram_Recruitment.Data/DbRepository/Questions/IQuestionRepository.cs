using Shaligram_Recruitment.Model.ViewModels.Questions;
using Shaligram_Recruitment.Model.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Data.DbRepository.Questions
{
    public interface IQuestionRepository
    {
        Task<int> QuestionAdd(QuestionModel model);
    }
}
