using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Shaligram_Recruitment.Model.ViewModels.Tokens;


namespace Shaligram_Recruitment.Services.JWTAuthentication
{
    public class JWTAuthenticationService : IJWTAuthenticationService
    {
        #region Generate Token
        public string GenerateToken(string EmailAddress, string UserId, string SecretKey, double JWT_Validity_Mins)
        {
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("EmailAddress",EmailAddress) ,
                    new Claim ("UserID",UserId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(JWT_Validity_Mins),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)), SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
        #endregion

        #region Generate Token Model
        public AccessTokenModel GenerateTokenModel(UserTokenModel userToken, string JWTSecretKey, int JWTValidityMinutes)
        {
            AccessTokenModel accessTokenVM = new AccessTokenModel();

            string serializeToken = JsonConvert.SerializeObject(userToken, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JWTSecretKey);

            DateTime tokenGeneratedOnUTC = DateTime.UtcNow;
            int tokenValidityInMins = JWTValidityMinutes;
            DateTime tokenExpiresOnUTC = tokenGeneratedOnUTC.AddMinutes(tokenValidityInMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {

                    new Claim(ClaimTypes.UserData, serializeToken)
                }),
                Expires = DateTime.UtcNow.AddMinutes(JWTValidityMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            accessTokenVM.Token = tokenString;
            accessTokenVM.ValidityInMin = tokenValidityInMins;
            accessTokenVM.ExpiresOnUTC = tokenExpiresOnUTC;
            accessTokenVM.UserId = userToken.UserId;

            return accessTokenVM;
        }
        #endregion

        #region Get User TokenData
        public UserTokenModel GetUserTokenData(string jwtToken)
        {
            UserTokenModel userTokenData = new UserTokenModel();
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(jwtToken);
            IEnumerable<Claim> claims = securityToken.Claims;

            if (claims != null && claims.ToList().Count > 0)
            {
                var claimData = claims.ToList().FirstOrDefault().Value;
                userTokenData = JsonConvert.DeserializeObject<UserTokenModel>(claimData);
                //userTokenData.userId = Convert.ToInt32(claimData[1].Value);
                //userTokenData.EmailId = claims.ToList().FirstOrDefault().Value;

                userTokenData.TokenValidTo = securityToken.ValidTo;
            }
            return userTokenData;




        }
        #endregion
    }
}
