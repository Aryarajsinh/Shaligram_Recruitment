using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shaligram_Recruitment.Model.ViewModels.CollegeBatch
{
    public class CollegeBatchModel
    {

        [Key]
        public int BatchId { get; set; } = 0;
       
        public string? CollegeName { get; set; }
     
        public string? Years { get; set; }
        public string? BatchName { get; set; }
     
        public IFormFile? UploadFile { get; set; }
    }
}
