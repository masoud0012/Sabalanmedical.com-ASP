using Microsoft.AspNetCore.Mvc.Filters;

namespace SabalanMedical.UI.Filters.ActionFilters
{
    public class ExecutedLogActionFilter:IActionFilter
    {
        private readonly ILogger<ExecutedLogActionFilter> _logger;
        public ExecutedLogActionFilter(ILogger<ExecutedLogActionFilter> logger)
        {

            _logger = logger;

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{FilterMethod}", nameof(ExecutedLogActionFilter)
                            , nameof(OnActionExecuted));
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterName}.{FilterMethod}", nameof(ExecutedLogActionFilter)
                          , nameof(OnActionExecuting));
            _logger.LogInformation($"{context.ActionDescriptor.DisplayName} just executed!");
        }
    }
}
