
using Microsoft.AspNetCore.Http;
using System.Text;
using Newtonsoft.Json;
using System.Security.Cryptography;

using Microsoft.Extensions.Primitives;

using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Web;

using Shaligram_Recruitment.Model.ViewModels;
using Shaligram_Recruitment.Common.Helpers;

namespace Shaligram_Recruitment.Common.CommonMethods
{
    public class CommonMethods
    {
        #region GetKeyValues
        public static string GetKeyValues(HttpContext context)
        {
            string json = string.Empty;
            if (context.Request != null && context.Request.HasFormContentType)
            {
                List<ReqResponseKeyValue> lstReqResponse = new List<ReqResponseKeyValue>();

                var data = context.Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString()).ToList();

                for (int i = 0; i < data.Count - 1; i++)
                {
                    ReqResponseKeyValue obj = new ReqResponseKeyValue();
                    obj.Key = data[i].Key.ToString();
                    obj.Value = data[i].Value.ToString();

                    lstReqResponse.Add(obj);
                }

                if (lstReqResponse.Count > 0)
                {
                    json = JsonConvert.SerializeObject(lstReqResponse);
                }
            }
            return json;
        }
        #endregion

        #region Get Hash
        public static byte[] GetHash(string plainPassword, string salt)
        {
            byte[] byteArray = Encoding.Unicode.GetBytes(string.Concat(salt, plainPassword));
            SHA256Managed sha256 = new SHA256Managed();
            byte[] hashedBytes = sha256.ComputeHash(byteArray);
            return hashedBytes;
        }
        #endregion

        #region GetKeyValues
        public static ParamValue GetKeyValue(HttpContext context)
        {
            ParamValue paramValues = new ParamValue();
            var headerValue = string.Empty;
            var queryString = string.Empty;
            var jsonString = string.Empty;
            StringValues outValue = string.Empty;

            // for from header value
            if (context.Request.Headers.TryGetValue(Constants.RequestModel, out outValue))
            {
                headerValue = outValue.FirstOrDefault();
                JObject jsonobj = JsonConvert.DeserializeObject<JObject>(headerValue);
                if (jsonobj != null)
                {
                    Dictionary<string, string> keyValueMap = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, JToken> keyValuePair in jsonobj)
                    {
                        keyValueMap.Add(keyValuePair.Key, keyValuePair.Value.ToString());
                    }
                    List<ReqResponseKeyValue> keyValueMapNew = keyValueMap.ToList().Select(i => new ReqResponseKeyValue
                    {
                        Key = i.Key,
                        Value = i.Value
                    }).ToList();
                    jsonString = JsonConvert.SerializeObject(keyValueMapNew);
                }
            }

            // for from query value
            if (context.Request.QueryString.HasValue)
            {
                var dict = HttpUtility.ParseQueryString(context.Request.QueryString.Value);
                queryString = System.Text.Json.JsonSerializer.Serialize(
                                    dict.AllKeys.ToDictionary(k => k, k => dict[k]));
            }


            paramValues.HeaderValue = jsonString;
            paramValues.QueryStringValue = queryString;
            return paramValues;

        }
        #endregion

        public static string GenerateNewRandom()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1)
            {
                r = GenerateNewRandom();
            }
            return r;
        }
        public static IList<string> requiredColumns = new[] { "EmployeeName", "EmailId" };
        public static bool IsValidEmail(string email)
        {
            // Regular expression pattern for email validation
            const string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
