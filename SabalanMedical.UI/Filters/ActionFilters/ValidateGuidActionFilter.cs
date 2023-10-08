using Microsoft.AspNetCore.Mvc.Filters;
using SabalanMedical.Controllers;

namespace SabalanMedical.UI.Filters.ActionFilters
{
    public class ValidateGuidActionFilter : IActionFilter
    {
        private readonly ILogger<ValidateGuidActionFilter> _logger;
        public ValidateGuidActionFilter(ILogger<ValidateGuidActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{FilterMethod}", nameof(ValidateGuidActionFilter)
                            , nameof(OnActionExecuted));
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterName}.{FilterMethod}",nameof(ValidateGuidActionFilter)
                ,nameof(OnActionExecuting));
            if (context.ActionArguments.ContainsKey("Id"))
            {
                var Id = context.ActionArguments["Id"];
                if (Guid.TryParse(Id?.ToString(), out _) == false)
                {
                    _logger.LogError("Id is not a Guid");
                    throw new ArgumentException("Id is not a Guid");
                }
            }
            else
            {
                _logger.LogError("Id is nll");
                throw new ArgumentException("Id is not a Guid");
            }

        }
    }
}
