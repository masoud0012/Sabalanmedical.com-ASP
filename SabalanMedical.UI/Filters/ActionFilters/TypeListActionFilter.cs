using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using SabalanMedical.Controllers;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO.ProductTypeDTO;

namespace SabalanMedical.UI.Filters.ActionFilters
{
    /// <summary>
    /// Returns All product types
    /// </summary>
    public class TypeListActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<TypeListActionFilter> _logger;
        private readonly IProductTypeService _productTypeService;
        private readonly IDiagnosticContext _diagnosticContext;
        public TypeListActionFilter(ILogger<TypeListActionFilter> logger,
            IDiagnosticContext diagnosticContext,
            IProductTypeService productTypeService)
        {
            _logger = logger;
            _productTypeService = productTypeService;
            _diagnosticContext = diagnosticContext;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{ActionName}", 
                nameof(TypeListActionFilter), nameof(OnActionExecutionAsync));
            if (context.Controller.GetType() == typeof(ProductsController))
            {
                var types= await _productTypeService.GetAllProductTypes();
                ((ProductsController)context.Controller).ViewBag.TypeList = types.Select(t => new SelectListItem() { Text=t.TypeNameEn,Value=t.Id.ToString()});
                _diagnosticContext.Set("AllTypes_From TypeListFilter", types);
            }
            else
            {
                _logger.LogError("No producttypes are added");
                throw new ArgumentNullException("No producttypes are added");
            }
           await next();
        }
    }
}
