using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Common.GlobalEnum
{
    public enum GlobalEnum
    {
        Added = 0,
        Duplicate = 1,
        DoesNotExists = 2,
        Updated = 3,
        Error = 4,
        Success = 5,
        Fail = 6,
        Delete = 7,
        InActive = 8,
        Active = 9
    }
    public enum UserPanelEnum
    {
        UserExist = -1,
        UserRegister = 1
    }
    public enum EditProfileEnum
    {
        Success = 0
    }
    public enum ChangePasswordEnum
    {
        Success = 0,
        PasswordNotMatch = 1
    }
}
