using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO.ProductsDTO;

namespace SabalanMedical.UI.ViewComponents
{
    public class ProductTableViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        public ProductTableViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync(Guid typeID, string searchBy, string searchKey)
        {
            List<ProductResponse> allProducts = new List<ProductResponse>();
            if (typeID == Guid.Empty)
            {
                allProducts = await _productService.GetFilteredProduct(searchBy, searchKey);
            }
            else
            {
                allProducts = await _productService.GetFilteredProduct(typeID, searchBy, searchKey);
            }
            return View(allProducts);
        }
    }
}
