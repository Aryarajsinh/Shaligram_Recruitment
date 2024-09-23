using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shaligram_Recruitment.Common.Helpers;
using Shaligram_Recruitment.Model.Config;
using Shaligram_Recruitment.Model.ViewModels.Questions;
using Shaligram_Recruitment.Model.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Data.DbRepository.Questions
{
    public class QuestionRepository:BaseRepository,IQuestionRepository
    {
        #region Field
        public IConfiguration _Configuration;
        #endregion

        #region Constructor
        public QuestionRepository(IConfiguration configuration, IOptions<DataConfig> dataconfig) : base(dataconfig, configuration)
        {
            _Configuration = configuration;
        }
        #endregion

        #region StudentDetails
        public async Task<int> QuestionAdd(QuestionModel model)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionType", model.QuestionType);
            parameters.Add("@QuestionSet", model.QuestionSet);
            parameters.Add("@QuestionText", model.QuestionText);
            parameters.Add("@AnswerOptions", model.AnswerOptions);

            var result = await ExecuteAsync<int>(StoredProcedures.QuestionAdd, parameters, commandType: CommandType.StoredProcedure);
            return result; // Returns the new question ID
        }
        #endregion 
    }
}
