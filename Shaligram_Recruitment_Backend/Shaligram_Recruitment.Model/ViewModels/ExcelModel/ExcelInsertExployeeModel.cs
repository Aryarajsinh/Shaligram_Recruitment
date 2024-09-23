using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Model.ViewModels.ExcelModel
{
    public class ExcelInsertExployeeModel
    {
        public string EmployeeName { get; set; }
        public string EmailId { get; set; }
        public long? TotalRecords { get; set; }
        public string errorMessage { get; set; }
    }
    public class ExcelInsertResponseModel
    {
        public List<ExcelInsertExployeeModel> excelInsertEmployeeListModel { get; set; }
        public string errorMessage { get; set; }
    }
    public class ExcelInsertEmployeeListModel
    {
        public List<ExcelInsertExployeeModel> excelInsertExployeeModels { get; set; }
    }
    public class GenerateExcel
    {
        public IFormFile ExcelFile { get; set; }
    }
}
