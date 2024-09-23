using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Model.ViewModels.Questions
{
    public class QuestionModel
    {
        public string QuestionType { get; set; }
        public string QuestionSet { get; set; }
        public string QuestionText { get; set; }
        public List<AnswerOptionModel> AnswerOptions { get; set; }
        public AnswerOptionModel AnswerOption { get; set; }
    }

    public class AnswerOptionModel
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }

}
