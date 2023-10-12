using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SabalanMedical.UI.Controllers;

namespace SabalanMedical.UI.Filters.AuthenticationFilters
{
    public class TokenAutherizastionFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Request.Cookies.ContainsKey("Token") == false)
            {
                //context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"Controller","Account" },
                    {"Action","index" }
                });
                               
                return;
            }
            if (context.HttpContext.Request.Cookies["Token"] != "Masoud")
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"Controller","Account" },
                    {"Action","index" }
                }); return;
            }

        }
    }
}
