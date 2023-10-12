using Microsoft.AspNetCore.Mvc.Filters;

namespace SabalanMedical.UI.Filters.ResultFilters
{
    public class TokenResltFilter : IResultFilter
    {
        private readonly ILogger<TokenResltFilter> _logger;
        public TokenResltFilter(ILogger<TokenResltFilter> logger)
        {
            _logger = logger;
        }
        public void OnResultExecuted(ResultExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{FilterMethod}",
                nameof(TokenResltFilter), nameof(OnResultExecuted)); 
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
           // context.HttpContext.Response.Cookies.Append("Token", "Masoud");
            _logger.LogInformation("{FilterName}.{FilterMethod}",
                nameof(TokenResltFilter), nameof(OnResultExecuting)); 

        }
    }
}
