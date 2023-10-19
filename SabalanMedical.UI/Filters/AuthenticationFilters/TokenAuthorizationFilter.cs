using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SabalanMedical.UI.Filters.AuthenticationFilters
{
    public class TokenAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Request.Cookies.ContainsKey("Token") == false)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                /*                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                                {
                                    {"Controller","AccountController" },
                                    {"Action","Index" }
                                });*/

                return;
            }
            if (context.HttpContext.Request.Cookies["Token"] != "Masoud")
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);

                /*                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                                {
                                    {"Controller","Account" },
                                    {"Action","index" }
                                }); */
                return;
            }

        }
    }
}
