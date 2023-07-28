using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO.ProductsDTO;
using ServiceContracts;
using ServiceContracts.DTO.ProductTypeDTO;

namespace SabalanMedical.ViewComponents
{
    public class ProductTypesViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IProductTypeService _productTypeService;
        private readonly List<ProductResponse>? _products;
        private readonly List<ProductTypeResponse>? _productsTypes;
        public ProductTypesViewComponent(IProductService productService, IProductTypeService productTypeService)
        {
            _productService=productService;
            _productTypeService = productTypeService;
            _products=_productService.GetAllProducts();
            _productsTypes = _productTypeService.GettAllProductTypes();

        }
        public async Task<IViewComponentResult> InvokeAsync(string param)
        {
            ViewBag.Products = _products.Where(t => t.isManufactured == true).ToList();
            ViewBag.Types = _productsTypes;
            ViewBag.param=param;
            return View();
        }
    }
}
