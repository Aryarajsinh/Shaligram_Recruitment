using Microsoft.AspNetCore.Http;
using Shaligram_Recruitment.Model.ViewModels.CollegeBatch;
using Shaligram_Recruitment.Model.ViewModels.Pagination;
using Shaligram_Recruitment.Model.ViewModels.QuestionSet;
using Shaligram_Recruitment.Model.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Services.StudentProfile
{
    public interface IStudentProfileService
    {
        Task<List<StudentModel>> StudentList();
        Task<List<CollegeBatchModel>> GetCollegeName();
        Task<List<CollegeBatchModel>> GetCollegeBatchList();
        Task<List<CollegeBatchModel>> GetBetchYear();
        Task<List<QuestionSetModel>> GetQuestionSet();
        Task<StudentModel> StudentDetails(int id);
        Task<CollegeBatchModel> BatchDetails(int id);
        Task<QuestionSetModel> QuestionSetDetails(int id);
        List<CsvFileModel> ParseCsvFile(IFormFile file);
        Task<int> BatchEdit(CollegeBatchModel model); 
        Task<int> QuestionSetEdit(QuestionSetModel model); 
        Task<int> DeleteBatch(int id); 
        Task<int> DeleteStudent(int id);
        Task<int> DeleteQuestionSet(int id);
        Task<int> StudentAdd(StudentModel model);
        Task<int> QuestionSetAdd(QuestionSetModel model);
        Task <int> uploadFileData(List<CsvFileModel> model,CollegeBatchModel db);
        Task<int> CollegeBatchAdd(CollegeBatchModel model);
        Task<PagedResult> GetPagedUsers(string search, int page, int pageSize, string sortColumn, string sortDirection);
        Task<PagedResult> GetPageCollegeBatch(string search, int page, int pageSize, string sortColumn, string sortDirection);
        Task<PagedResult> GetPageQuestionSet(string search, int page, int pageSize, string sortColumn, string sortDirection);
    }
}
