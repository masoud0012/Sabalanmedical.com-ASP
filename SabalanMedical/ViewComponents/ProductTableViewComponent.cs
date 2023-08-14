using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO.ProductsDTO;

namespace SabalanMedical.ViewComponents
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
            List<ProductResponse> allProducts =await _productService.GetAllProducts();
            List<ProductResponse> filteredProducts = allProducts;

            if (typeID == Guid.Empty && String.IsNullOrEmpty(searchKey) && String.IsNullOrEmpty(searchBy))
            {
                ViewBag.products = allProducts;
            }

            else
            {
                if (typeID!=Guid.Empty)
                {
                    filteredProducts = allProducts.Where(t => t.TypeId == typeID).ToList();
                }
                if (!String.IsNullOrEmpty(searchKey))
                {
                    switch (searchBy)
                    {
                        case ("ProductNameEn"):
                            filteredProducts = filteredProducts.Where(t => t.ProductNameEn.
                            Contains(searchKey, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                        case ("ProductNameFr"):
                            filteredProducts = filteredProducts.Where(t => t.ProductNameFr.
                            Contains(searchKey, StringComparison.OrdinalIgnoreCase)).ToList();
                            break;
                    };
                }
            }
            ViewBag.products = filteredProducts;
            return View();
        }
    }
}
