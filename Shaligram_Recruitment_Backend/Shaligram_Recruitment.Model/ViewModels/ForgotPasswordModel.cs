using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Model.ViewModels
{
    public class ForgotPasswordModel
    {
        public int? UserId { get; set; }

        public string? Firstname { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
