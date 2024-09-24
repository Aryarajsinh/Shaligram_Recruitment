using Shaligram_Recruitment.Model.ViewModels.CollegeBatch;
using Shaligram_Recruitment.Model.ViewModels.QuestionSet;
using Shaligram_Recruitment.Model.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Model.ViewModels.Pagination
{
    public class PagedResult
    {
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public List<StudentModel> Users { get; set; }
        public List<CollegeBatchModel> CollegeBatch { get; set; }
        public List<QuestionSetModel> QuestionSet { get; set; }
        public QuestionSetModel QuestionSets { get; set; }
        public CollegeBatchModel Batches { get; set; }
        [Required]
        public StudentModel Student { get; set; }
    }
    public class DataTableAjaxPostModel
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string SortDirection { get; set; }
        public string SortColumn { get; set; }
        public Search Search { get; set; }
    }

    public class Search
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class ServerSidePage
    {
        public string search { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }
    }
    

}
