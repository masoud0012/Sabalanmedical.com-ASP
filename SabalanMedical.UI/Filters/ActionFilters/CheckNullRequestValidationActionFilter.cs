using Microsoft.AspNetCore.Mvc.Filters;

namespace SabalanMedical.UI.Filters.ActionFilters
{
    /// <summary>
    /// Checks the nullability of reqest argument. Throws ArgumentNullException if it is null;
    /// </summary>
    public class CheckNullRequestValidationActionFilter : IActionFilter
    {
        private readonly ILogger<CheckNullRequestValidationActionFilter> _logger;
        public CheckNullRequestValidationActionFilter(ILogger<CheckNullRequestValidationActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{FilterMethod}",
                            nameof(CheckNullRequestValidationActionFilter), nameof(OnActionExecuted));
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterName}.{FilterMethod}",
                nameof(CheckNullRequestValidationActionFilter),nameof(OnActionExecuting));
            if (!context.ActionArguments.ContainsKey("request"))
            {
                _logger.LogError("requset is null");
                throw new ArgumentNullException("Request is null");
            }
        }
    }
}
