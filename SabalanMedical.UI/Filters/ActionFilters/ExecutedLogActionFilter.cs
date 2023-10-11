using Microsoft.AspNetCore.Mvc.Filters;

namespace SabalanMedical.UI.Filters.ActionFilters
{
    /// <summary>
    ///Specifies method of the filter and informs entering to a specific action method
    /// </summary>
    public class ExecutedLogActionFilter : IActionFilter
    {
        private readonly ILogger<ExecutedLogActionFilter> _logger;
        public ExecutedLogActionFilter(ILogger<ExecutedLogActionFilter> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// after excecuting method
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{FilterMethod}", nameof(ExecutedLogActionFilter)
                            , nameof(OnActionExecuted));
        }
        /// <summary>
        /// befor executing method
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterName}.{FilterMethod}", nameof(ExecutedLogActionFilter)
                          , nameof(OnActionExecuting));
            _logger.LogInformation($"{context.ActionDescriptor.DisplayName} just executed!");
        }
    }
}
