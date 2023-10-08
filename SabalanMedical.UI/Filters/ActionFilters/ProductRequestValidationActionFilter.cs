using Azure.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts;
using SabalanMedical.Controllers;

namespace SabalanMedical.UI.Filters.ActionFilters
{
    public class ProductRequestValidationActionFilter : IActionFilter
    {
        private readonly ILogger<ProductRequestValidationActionFilter> _logger;
        public ProductRequestValidationActionFilter(ILogger<ProductRequestValidationActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("OnActionExecuted filter");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("ProductRequestValidationActionFilter on executing");
            if (!context.ActionArguments.ContainsKey("request"))
            {
                _logger.LogError("requset is null");
                throw new ArgumentNullException("Request is null");
            }
        }
    }
}
