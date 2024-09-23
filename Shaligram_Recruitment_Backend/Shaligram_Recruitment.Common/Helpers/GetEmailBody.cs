using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Common.Helpers
{
    public class GetEmailBody
    {
        public async static Task<string> GetEmailBodyText(string email)
        {
            var data = await File.ReadAllTextAsync(email);
            return data;
        }
    }
}
