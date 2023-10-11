using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using SabalanMedical.Controllers;
using ServiceContracts;

namespace SabalanMedical.UI.Filters.ActionFilters
{
    /// <summary>
    /// Checks if request paramatters are valid .if not valid it short circutes the action method
    /// </summary>
    public class CheckValidationActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<CheckValidationActionFilter> _logger;
        private readonly IProductTypeService _productTypeService;
        public CheckValidationActionFilter(ILogger<CheckValidationActionFilter> logger,
            IProductTypeService productTypeService)
        {
            _logger = logger;
            _productTypeService = productTypeService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{FilterMethod} befor logic",
                nameof(CheckValidationActionFilter), nameof(OnActionExecutionAsync));
            if (context.Controller is ProductsController productsController)
            {
                if (!productsController.ModelState.IsValid)
                {
                    var request = context.ActionArguments["request"];
                    _logger.LogError($"reaquest Validation failed in: {context.ActionDescriptor.DisplayName}");
                    var types = await _productTypeService.GetAllProductTypes();
                    productsController.ViewBag.TypeList = types.Select(t => new SelectListItem() { Text = t.TypeNameEn, Value = t.Id.ToString() });
                    productsController.ViewBag.Errors = context.ModelState.Values.SelectMany(t => t.Errors).Select(t => t.ErrorMessage).ToList();
                    context.Result = productsController.View("AddProduct", request);//Short circute
                }
                else
                {
                    await next();
                }
            }
            else
            {
                _logger.LogInformation("{FilterName}.{FilterMethod} After logic",
                     nameof(CheckValidationActionFilter), nameof(OnActionExecutionAsync));
                await next();
            }
        }
    }
}
