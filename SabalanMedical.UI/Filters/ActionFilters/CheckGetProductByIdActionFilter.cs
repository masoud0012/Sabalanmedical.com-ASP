using Microsoft.AspNetCore.Mvc.Filters;
using SabalanMedical.Controllers;
using ServiceContracts;

namespace SabalanMedical.UI.Filters.ActionFilters
{
    /// <summary>
    /// Checks if GetProductById methos returns null or not based on given Guid arguments
    /// </summary>
    public class CheckGetProductByIdActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<CheckGetProductByIdActionFilter> _logger;
        private readonly IProductService _productService;
        public CheckGetProductByIdActionFilter(ILogger<CheckGetProductByIdActionFilter> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{FilterMethod} befor logic",
                    nameof(CheckGetProductByIdActionFilter), nameof(OnActionExecutionAsync));
            if (context.ActionArguments.ContainsKey("Id"))
            {
                var productResponse = await _productService.GetProductById((Guid)context.ActionArguments["Id"]);
                if (productResponse is null)
                {
                    _logger.LogError($"No product was found for id={context.ActionArguments["Id"]}");
                    context.Result = ((ProductsController)context.Controller).View("Index");
                }
                else
                {
                    await next();
                }
            }
            else
            {
                context.Result = ((ProductsController)context.Controller).View("Index");
            }
        }
    }
}
