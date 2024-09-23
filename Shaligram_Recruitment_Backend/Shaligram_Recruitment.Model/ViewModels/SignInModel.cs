using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Model.ViewModels
{
    public class SignInModel
    {
        //[Key]
        public int Id { get; set; }

        //[Required(ErrorMessage = "Please Enter Email")]
        //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string Email { get; set; }
        public string? FirstName { get; set; }

        //[Required(ErrorMessage = "Please Enter Password ")]
        public string Password { get; set; }
        public string? Token { get; set; }
        public bool? IsChecked { get; set; }
    }

    public class LoginRequestModel
    {
        [Required]
        public string EmailId { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
