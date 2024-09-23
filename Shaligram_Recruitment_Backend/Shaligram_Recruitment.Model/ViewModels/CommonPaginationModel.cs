using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Model.ViewModels
{
    public class CommonPaginationModel
    {
        [Required]
        public long PageNumber { get; set; }
        [Required]
        public long PageSize { get; set; }
        public string SearchData { get; set; }
        public string OrderBy { get; set; }
        public string SortType { get; set; }
    }
}
