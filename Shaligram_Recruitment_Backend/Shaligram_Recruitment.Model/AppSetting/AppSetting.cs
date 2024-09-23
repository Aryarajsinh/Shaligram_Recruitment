using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Model.AppSetting
{
    public class AppSetting
    {
        public string JWTSecretKey { get; set; }
        public int JWTValidityMinutes { get; set; }
    }
}
