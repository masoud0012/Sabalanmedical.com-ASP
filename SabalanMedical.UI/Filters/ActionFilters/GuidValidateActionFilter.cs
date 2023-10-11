using Microsoft.AspNetCore.Mvc.Filters;
using SabalanMedical.Controllers;

namespace SabalanMedical.UI.Filters.ActionFilters
{
    /// <summary>
    /// Checks the Guid input argument. enter to the action method if Guid is valid else throws ArgumentException 
    /// </summary>
    public class GuidValidateActionFilter : IActionFilter
    {
        private readonly ILogger<GuidValidateActionFilter> _logger;
        public GuidValidateActionFilter(ILogger<GuidValidateActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{FilterMethod}", nameof(GuidValidateActionFilter)
                            , nameof(OnActionExecuted));
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterName}.{FilterMethod}",nameof(GuidValidateActionFilter)
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
