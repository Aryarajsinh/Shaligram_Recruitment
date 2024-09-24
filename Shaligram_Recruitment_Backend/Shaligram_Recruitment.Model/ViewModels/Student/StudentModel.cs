using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Model.ViewModels.Student
{
    public class StudentModel
    {
        [Key]
        public int? StudentId { get; set; }

        public string? StudentName { get; set; }
        
        public string? CollegeName { get; set; }
        
        public string? BatchYear{ get; set; }
       
        public string? EmailAddress { get; set; }
        
        public string? PhoneNumber { get; set; }
        public int? TotalRecord { get; set; }
   
    }

    public class CsvFileModel
    {

        public string? StudentName { get; set; }               

        public string? EmailAddress { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
