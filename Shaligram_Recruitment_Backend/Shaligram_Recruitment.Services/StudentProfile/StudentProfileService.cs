using Dapper;
using Shaligram_Recruitment.Data.DbRepository.AdminPanel;
using Shaligram_Recruitment.Data.DbRepository.StudentProfile;
using Shaligram_Recruitment.Model.ViewModels;
using Shaligram_Recruitment.Model.ViewModels.Pagination;
using Shaligram_Recruitment.Model.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shaligram_Recruitment.Model.ViewModels.CollegeBatch;
using Microsoft.AspNetCore.Http;
using System.Formats.Asn1;
using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using Shaligram_Recruitment.Model.ViewModels.QuestionSet;

namespace Shaligram_Recruitment.Services.StudentProfile
{
    public class StudentProfileService:IStudentProfileService
    {
        #region Feilds
        private readonly IStudentProfileRepository _studentServices;
        #endregion

        #region Constructor
        public StudentProfileService(IStudentProfileRepository studentRepository)
        {
            _studentServices = studentRepository;
        }
        #endregion

        #region StudentList
        public async Task<List<StudentModel>> StudentList()
        {
            return await _studentServices.StudentList();
        }
        #endregion 
        #region CollegeList
        public async Task<List<CollegeBatchModel>> GetCollegeName()
        {
            return await _studentServices.CollegeNameList();
        }
        #endregion 
        #region CollegeBatchList
        public async Task<List<CollegeBatchModel>> GetCollegeBatchList()
        {
            return await _studentServices.CollegeBatchList();
        }
        #endregion
        #region BetchYear
        public async Task<List<CollegeBatchModel>> GetBetchYear()
        {
            return await _studentServices.BatchYears();
        }
        #endregion
        #region GetQuestionSet
        public async Task<List<QuestionSetModel>> GetQuestionSet()
        {
            return await _studentServices.QuestionSet();
        }
        #endregion

        #region StudentDetails
        public async Task<StudentModel> StudentDetails(int id)
        {
            return await _studentServices.StudentDetails(id);
        }
        #endregion
        #region CollegeBatchDetails
        public async Task<CollegeBatchModel> BatchDetails(int id)
        {
            return await _studentServices.CollegeBatchDetails(id);
        }
        #endregion   
        #region QuestionSetDetails
        public async Task<QuestionSetModel> QuestionSetDetails(int id)
        {
            return await _studentServices.QuestionSetDetails(id);
        }
        #endregion 
        #region DeleteStudent
        public async Task<int> DeleteStudent(int id)
        {
            return await _studentServices.DeleteStudent(id);
        }
        #endregion
        #region DeleteQuestionSet
        public async Task<int> DeleteQuestionSet(int id)
        {
            return await _studentServices.DeleteQuestionSet(id);
        }
        #endregion 
        #region DeleteBatch
        public async Task<int> DeleteBatch(int id)
        {
            return await _studentServices.DeleteBatch(id);
        }
        #endregion
        #region Add Student
        public async Task<int> StudentAdd(StudentModel model)
        {
            return await _studentServices.StudentAdd(model);
        }
        #endregion 
        #region Add QuestionSetAdd
        public async Task<int> QuestionSetAdd(QuestionSetModel model)
        {
            return await _studentServices.QuestionSetAdd(model);
        }
        #endregion 
        #region Upload StudentList
        public async Task<int> uploadFileData(List<CsvFileModel> model, CollegeBatchModel db)
        {
            return await _studentServices.uploadFileData(model,db);
        }
        #endregion 
        #region Add College Batch
        public async Task<int> CollegeBatchAdd(CollegeBatchModel model)
        {
            return await _studentServices.CollegeBatchAdd(model);
        }
        #endregion 
        #region Update College Batch
        public async Task<int> BatchEdit(CollegeBatchModel model)
        {
            return await _studentServices.CollegeBatchEdit(model);
        }
        #endregion 
        #region Update College Batch
        public async Task<int> QuestionSetEdit(QuestionSetModel model)
        {
            return await _studentServices.QuestionSetEdit(model);
        }
        #endregion

        public async Task<PagedResult> GetPagedUsers(string search, int page, int pageSize, string sortColumn, string sortDirection)
        {
            using (var connection = new SqlConnection("Server=192.168.1.38;Database=ShaligramRecruitment_Aryarajsinh;User Id=sa;password=123456;TrustServerCertificate=True"))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Search", search); // Pass the search string
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@PageNumber", page);
                parameters.Add("@SortColumn", sortColumn); // New parameter for sorting
                parameters.Add("@SortDirection", sortDirection); // New parameter for sorting
                parameters.Add("@TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var users = await connection.QueryAsync<StudentModel>(
                    "GetPagedUsers",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                int totalRecords = parameters.Get<int>("@TotalRecords");

                return new PagedResult
                {
                    TotalRecords = totalRecords,
                    PageSize = pageSize,
                    CurrentPage = page,
                    Users = users.ToList()
                };
            }
        }
            public async Task<PagedResult> GetPageCollegeBatch(string search, int page, int pageSize, string sortColumn, string sortDirection)
        {
            using (var connection = new SqlConnection("Server=192.168.1.38;Database=ShaligramRecruitment_Aryarajsinh;User Id=sa;password=123456;TrustServerCertificate=True"))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Search", search); // Pass the search string
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@PageNumber", page);
                parameters.Add("@SortColumn", sortColumn); // New parameter for sorting
                parameters.Add("@SortDirection", sortDirection); // New parameter for sorting
                parameters.Add("@TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var CollegeData = await connection.QueryAsync<CollegeBatchModel>(
                    "Sp_GetPagedCollegeBatch",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                int totalRecords = parameters.Get<int>("@TotalRecords");

                return new PagedResult
                {
                    TotalRecords = totalRecords,
                    PageSize = pageSize,
                    CurrentPage = page,
                    CollegeBatch = CollegeData.ToList()
                };
            }
        }  
        public async Task<PagedResult> GetPageQuestionSet(string search, int page, int pageSize, string sortColumn, string sortDirection)
        {
            using (var connection = new SqlConnection("Server=192.168.1.38;Database=ShaligramRecruitment_Aryarajsinh;User Id=sa;password=123456;TrustServerCertificate=True"))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Search", search); // Pass the search string
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@PageNumber", page);
                parameters.Add("@SortColumn", sortColumn); // New parameter for sorting
                parameters.Add("@SortDirection", sortDirection); // New parameter for sorting
                parameters.Add("@TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var QuestionSet = await connection.QueryAsync<QuestionSetModel>(
                    "SP_GetPagedQuestionSet",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                int totalRecords = parameters.Get<int>("@TotalRecords");

                return new PagedResult
                {
                    TotalRecords = totalRecords,
                    PageSize = pageSize,
                    CurrentPage = page,
                    QuestionSet = QuestionSet.ToList()
                };
            }
        }

        public List<CsvFileModel> ParseCsvFile(IFormFile file)
        {
            var records = new List<CsvFileModel>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                records = csv.GetRecords<CsvFileModel>().ToList();
            }

            return records;
        }

    }
}
