using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO.ProductsDTO;
using Services;

namespace SabalanMedical.ViewComponents
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
            

            return View(allProducts.Where(t => t.isManufactured == true).ToList());//shared/Components/Manufactured/default.cshtml
        }
    }
}
