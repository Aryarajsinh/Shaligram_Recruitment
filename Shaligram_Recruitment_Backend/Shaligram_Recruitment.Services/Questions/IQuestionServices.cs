using Shaligram_Recruitment.Model.ViewModels.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Services.Questions
{
    public interface IQuestionServices
    {
        Task<int> QuestionAdd(QuestionModel model);
    }
}
