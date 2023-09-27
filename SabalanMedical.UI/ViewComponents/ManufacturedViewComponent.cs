using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO.ProductsDTO;
using Services;

namespace SabalanMedical.UI.ViewComponents
{
    public class ManufacturedViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        public ManufacturedViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ProductResponse> allProducts = await _productService.GetAllProducts();

           List<ProductResponse> manufacturedProducts =allProducts.Where(t => t.isManufactured == true).ToList();
            return View(manufacturedProducts);//shared/Components/Manufactured/default.cshtml
        }
    }
}
