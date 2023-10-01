
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace SabalanMedical.Core.Filters.ActionFilters
{
    public class ProductListActionFilter : IActionFilter
    {
        private readonly ILogger<ProductListActionFilter> _logger;
        public ProductListActionFilter(ILogger<ProductListActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //To Do after executing a acyion
            _logger.LogInformation("ProductList Action (onActionExecuted) Filter executed");

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //To Do befor executnig an action
            _logger.LogInformation("ProductList Action (onActionExecuting) Filter is execting");
        }
    }
}
