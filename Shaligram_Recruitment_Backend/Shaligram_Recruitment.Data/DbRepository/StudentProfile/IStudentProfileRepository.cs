using Shaligram_Recruitment.Model.ViewModels.CollegeBatch;
using Shaligram_Recruitment.Model.ViewModels.QuestionSet;
using Shaligram_Recruitment.Model.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Data.DbRepository.StudentProfile
{
    public interface IStudentProfileRepository
    {
        Task<List<StudentModel>> StudentList();
        Task<List<CollegeBatchModel>> CollegeNameList();
        Task<List<CollegeBatchModel>> CollegeBatchList();
        Task<List<CollegeBatchModel>> BatchYears();
        Task<List<QuestionSetModel>> QuestionSet();
        Task<StudentModel> StudentDetails(int id);
        Task<CollegeBatchModel> CollegeBatchDetails(int id);
        Task<QuestionSetModel> QuestionSetDetails(int id);
        Task<int> DeleteStudent(int id);
        Task<int> DeleteQuestionSet(int id);
        Task<int> DeleteBatch(int id);
        Task<int> StudentAdd(StudentModel id);
        Task<int> QuestionSetAdd(QuestionSetModel id);
        Task<int> uploadFileData(List<CsvFileModel> model, CollegeBatchModel db);
        Task<int> CollegeBatchAdd(CollegeBatchModel id);
        Task<int> CollegeBatchEdit(CollegeBatchModel id);
        Task<int> QuestionSetEdit(QuestionSetModel id);
    }
}
