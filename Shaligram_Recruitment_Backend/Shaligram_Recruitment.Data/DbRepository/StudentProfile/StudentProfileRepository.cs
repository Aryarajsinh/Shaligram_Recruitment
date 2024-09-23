using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shaligram_Recruitment.Common.Helpers;
using Shaligram_Recruitment.Model.Config;
using Shaligram_Recruitment.Model.ViewModels;
using Shaligram_Recruitment.Model.ViewModels.CollegeBatch;
using Shaligram_Recruitment.Model.ViewModels.QuestionSet;
using Shaligram_Recruitment.Model.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Data.DbRepository.StudentProfile
{
    public class StudentProfileRepository :BaseRepository,IStudentProfileRepository
    {
        #region Field
        public IConfiguration _Configuration;
        #endregion

        #region Constructor
        public StudentProfileRepository(IConfiguration configuration, IOptions<DataConfig> dataconfig) : base(dataconfig, configuration)
        {
            _Configuration = configuration;
        }
        #endregion

# region StudentList
        public async Task<List<StudentModel>> StudentList()
        {          
            var data = await QueryAsync<StudentModel>(StoredProcedures.StudentList, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        #endregion
        # region CollegeNameList
        public async Task<List<CollegeBatchModel>> CollegeNameList()
        {          
            var data = await QueryAsync<CollegeBatchModel>(StoredProcedures.CollegeNameList, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        #endregion
        # region CollegeBatchList
        public async Task<List<CollegeBatchModel>> CollegeBatchList()
        {          
            var data = await QueryAsync<CollegeBatchModel>(StoredProcedures.CollegeBatchList, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        #endregion    
        # region BatchYears
        public async Task<List<CollegeBatchModel>> BatchYears()
        {          
            var data = await QueryAsync<CollegeBatchModel>(StoredProcedures.BatchYears, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        #endregion 
        # region QuestionSet
        public async Task<List<QuestionSetModel>> QuestionSet()
        {          
            var data = await QueryAsync<QuestionSetModel>(StoredProcedures.QuestionSet, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        #endregion

        #region StudentDetails
        public async Task<StudentModel> StudentDetails(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@StudentId", id);
            var data = await QueryFirstOrDefaultAsync<StudentModel>(StoredProcedures.StudentDetails, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion 
        #region CollegeBatchDetails
        public async Task<CollegeBatchModel> CollegeBatchDetails(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@BatchId", id);
            var data = await QueryFirstOrDefaultAsync<CollegeBatchModel>(StoredProcedures.CollegebatchDetails, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion 
        #region QuestionSetDetails
        public async Task<QuestionSetModel> QuestionSetDetails(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@QuestionSetId", id);
            var data = await QueryFirstOrDefaultAsync<QuestionSetModel>(StoredProcedures.QuestionSetDetails, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion  
        #region StudentDetails
        public async Task<int> DeleteStudent(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@StudentId", id);
            var data = await ExecuteAsync<int>(StoredProcedures.DeleteStudent, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion  
        #region DeleteQuestionSet
        public async Task<int> DeleteQuestionSet(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@QuestionSetId", id);
            var data = await ExecuteAsync<int>(StoredProcedures.DeleteQuestionSet, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion  
        #region DeleteBatch
        public async Task<int> DeleteBatch(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@BatchId", id);
            var data = await ExecuteAsync<int>(StoredProcedures.DeleteBatch, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion  
        #region StudentDetails
        public async Task<int> StudentAdd(StudentModel model)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@StudentName", model.StudentName);
            parameter.Add("@CollegeName", model.CollegeName);
            parameter.Add("@Batch", model.BatchYear);
            parameter.Add("@Email", model.EmailAddress);
            parameter.Add("@PhoneNumber", model.PhoneNumber);
            var data = await ExecuteAsync<int>(StoredProcedures.StudentAdd, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion 
        #region QuestionSetAdd
        public async Task<int> QuestionSetAdd(QuestionSetModel model)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@QuestionSetName", model.QuestionSetName);       
            parameter.Add("@QuestionSetId", model.QuestionSetId);       
            var data = await ExecuteAsync<int>(StoredProcedures.QuestionSetAdd, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion  
        #region uploadFileData
        public async Task<int> uploadFileData(List<CsvFileModel> model, CollegeBatchModel db)
        {
            // Convert the list to a DataTable
            DataTable studentTable = new DataTable("dbo.StudentType");
            studentTable.Columns.Add("StudentName", typeof(string));
            studentTable.Columns.Add("CollegeName", typeof(string));
            studentTable.Columns.Add("BatchYear", typeof(string));
            studentTable.Columns.Add("EmailAddress", typeof(string));
            studentTable.Columns.Add("PhoneNumber", typeof(string));

            foreach (var record in model)
            {
                studentTable.Rows.Add(record.StudentName, db.CollegeName, db.Years, record.EmailAddress, record.PhoneNumber);
            }

            // Use Dapper to pass the DataTable as a parameter
            var parameter = new DynamicParameters();
            parameter.Add("@StudentData", studentTable.AsTableValuedParameter("dbo.StudentType"));

            // Execute the stored procedure
            var result = await ExecuteAsync<int>(StoredProcedures.StudentAdd, parameter, commandType: CommandType.StoredProcedure);

            return result;
            
        }
        #endregion
        #region CollegeBatch
        public async Task<int> CollegeBatchAdd(CollegeBatchModel model)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@CollegeName", model.CollegeName);
            parameter.Add("@Years", model.Years);
            parameter.Add("@BatchName", model.BatchName);
            parameter.Add("@BatchId", model.BatchId);
          
            var data = await ExecuteAsync<int>(StoredProcedures.CollegeBatchAdd, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion   
        #region UpdateCollegeBatch
        public async Task<int> CollegeBatchEdit(CollegeBatchModel model)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@CollegeName", model.CollegeName);
            parameter.Add("@Years", model.Years);
            parameter.Add("@BatchName", model.BatchName);
            parameter.Add("@BatchId", model.BatchId);          
            var data = await ExecuteAsync<int>(StoredProcedures.CollegeBatchEdit, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion  
        #region QuestionSetEdit
        public async Task<int> QuestionSetEdit(QuestionSetModel model)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@QuestionSetName", model.QuestionSetName);       
            parameter.Add("@QuestionSetId", model.QuestionSetId);          
            var data = await ExecuteAsync<int>(StoredProcedures.QuestionSetEdit, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
        #endregion
    }
}
