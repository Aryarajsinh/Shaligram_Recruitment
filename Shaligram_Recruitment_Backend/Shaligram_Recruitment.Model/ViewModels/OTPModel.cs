using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Model.ViewModels
{
    public class OTPModel
    {
        [Required(ErrorMessage = "OTP is Required")]
        public int OTP { get; set; }
        public string Email { get; set; }
    }
}
