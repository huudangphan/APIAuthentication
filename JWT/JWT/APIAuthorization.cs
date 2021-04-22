using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JWT
{
    [AttributeUsage(AttributeTargets.Class)]
    
    public class APIAuthorization: Attribute, IAuthorizationFilter 
    {
      
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {        
            var token = filterContext.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (check(token))
            {
                return;
            }
            else
            {                
                filterContext.HttpContext.Response.Headers.Add("AuthStatus", "NotAuthorized");
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";              
                filterContext.Result = new JsonResult("NotAuthorized")
                {
                    Value = new
                    {
                        Status = "Error",
                        Message = "Invalid Token"
                    },
                };
            }
        }
        public bool check(string token)
        {
            // kiem tra tung token trong listToken khong
            foreach (var item in Startup.listToken)
            {
                if (item == token )
                    return true;
            }
            return false;
        }

        //public int? ValidateJwtToken(string token)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.UTF32.GetBytes("MyAPIKey");
        //    try
        //    {
        //        tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            ClockSkew = TimeSpan.Zero
        //        }, out SecurityToken validatedToken);
        //        var jwtToken = (JwtSecurityToken)validatedToken;
        //        int id = Int32.Parse(jwtToken.Claims.First(a => a.Type == "id").Value);
        //        // tra ve username 
        //        return id;
        //    }
        //    catch
        //    {
        //        // return null if validation fails
        //        return null;
        //    }
        //}
    }
}
