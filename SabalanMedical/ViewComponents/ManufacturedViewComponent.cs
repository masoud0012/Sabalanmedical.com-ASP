using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO.ProductsDTO;
using Services;

namespace SabalanMedical.ViewComponents
{
    public class ManufacturedViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        public ManufacturedViewComponent(IProductService productService, IProductImageService imageService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ProductResponse> allProducts = _productService.GetAllProducts()
                .Where(t=>t.isManufactured==true).ToList();
            
            return View(allProducts);//shared/Components/Manufactured/default.cshtml
        }
    }
}
