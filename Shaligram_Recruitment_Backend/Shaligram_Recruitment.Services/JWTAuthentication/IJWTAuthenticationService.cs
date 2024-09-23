using Shaligram_Recruitment.Model.ViewModels.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Services.JWTAuthentication
{
    public interface IJWTAuthenticationService
    {
        string GenerateToken(string EmailAddress, string UserId, string SecretKey, double JWT_Validity_Mins);

        //Generate Token Model
        AccessTokenModel GenerateTokenModel(UserTokenModel userToken, string JWT_Secret, int JWT_Validity_Mins);

        //Get User Token Data
        UserTokenModel GetUserTokenData(string jwtToken);
    }
}
