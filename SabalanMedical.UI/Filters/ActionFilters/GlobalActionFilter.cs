using Microsoft.AspNetCore.Mvc.Filters;

namespace SabalanMedical.UI.Filters.ActionFilters
{
    public class GlobalActionFilter : IActionFilter
    {
        private readonly ILogger<GlobalActionFilter> _logger;
        private readonly string _key;
        private readonly object _value;
        public GlobalActionFilter(ILogger<GlobalActionFilter> logger,string Key,Object Value)
        {
            _logger = logger;
            _key = Key;
            _value = Value;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterName}.{FilterMethod}",
                nameof(GlobalActionFilter), nameof(OnActionExecuting));
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{FilterMethod}",
                nameof(GlobalActionFilter), nameof(OnActionExecuted));
        }

     
    }
}
