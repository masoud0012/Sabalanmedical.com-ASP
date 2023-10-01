using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
namespace TestProject.UI.ViewComponents
{
    public class ProductTypesViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IProductTypeService _productTypeService;
        public ProductTypesViewComponent(IProductService productService,
            IProductTypeService productTypeService)
        {
            _productService = productService;
            _productTypeService = productTypeService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string param)
        {
            var allProducts = await _productService.GetAllProducts();
            ViewBag.Products = allProducts.Where(t => t.isManufactured == true).ToList();
            ViewBag.Types = await _productTypeService.GetAllProductTypes();
            ViewBag.param = param;
            return View();
        }
    }
}
