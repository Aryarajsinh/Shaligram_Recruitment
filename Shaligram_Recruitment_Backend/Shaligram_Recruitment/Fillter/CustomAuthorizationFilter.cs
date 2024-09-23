using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Shaligram_Recruitment.Fillter
{
    public class CustomAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                string? customerJWTToken = Convert.ToString(context.HttpContext.Session.GetString("_token"));
                if (!string.IsNullOrEmpty(customerJWTToken))
                {
                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    JwtSecurityToken? token = handler.ReadToken(customerJWTToken) as JwtSecurityToken;
                    if (token != null)
                    {
                        if (token.ValidTo < DateTime.UtcNow.AddMinutes(1))
                        {
                            context.HttpContext.Session.Clear();
                            context.HttpContext.Response.Cookies.Delete(".Admin.Session");
                            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                action = "SignIn",
                                controller = "AdminPanel",
                                returnUrl = context.HttpContext.Request.Path.Value
                            }));
                        }
                    }
                }
                else
                {
                    context.HttpContext.Session.Clear();
                    context.HttpContext.Response.Cookies.Delete(".Admin.Session");
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        action = "SignIn",
                        controller = "AdminPanel",
                        returnUrl = context.HttpContext.Request.Path.Value
                    }));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
