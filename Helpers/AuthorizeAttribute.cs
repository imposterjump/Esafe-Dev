using BankProject.Data.Enums;
using BankProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace BankProject.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public Role role { get; set; }
        
        public AuthorizeAttribute(Role _role)
        {
            this.role = _role;
        }
        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            Console.WriteLine("Inside authorization");
            var user2 = (Admin)context.HttpContext.Items["Client"];
            Console.WriteLine("i am hereeeee" + user2.ToString());
            var user =(User) context.HttpContext.Items["Client"];

             Console.WriteLine("i am hereeeee"+user.ToString());
            
            
            if (user == null ||user.Role!=this.role)
            {
               
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized, Access Denied" }) { StatusCode = StatusCodes.Status401Unauthorized };
                
            }
        }
    }
}
