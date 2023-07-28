using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.ProductDescriptionDTO;
using ServiceContracts.DTO.ProductImageDTO;
using ServiceContracts.DTO.ProductPropertyDTO;
using ServiceContracts.DTO.ProductsDTO;
using ServiceContracts.DTO.ProductTypeDTO;

namespace SabalanMedical.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductTypeService _productTypeService;
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        private readonly IProductDescService _productDescService;
        private readonly IProductPropertyService _productPropertyService;
        public HomeController(IProductTypeService productTypeService,
            IProductService productService,
            IProductImageService productImageService,
            IProductDescService productDescService,
            IProductPropertyService productPropertyService)
        {
            _productTypeService = productTypeService;
            _productService = productService;
            _productImageService = productImageService;
            _productDescService = productDescService;
            _productPropertyService = productPropertyService;
        }

        [Route("/")]
        [Route("[action]")]
        public IActionResult Index()
        {
            List<ProductResponse> allProducts = _productService.GetAllProducts();
            ViewBag.Title = "صفحه اول";
            return View(allProducts);
        }
        [Route("[action]")]
        public IActionResult Contact()
        {
            ViewBag.Title = "تماس با ما";
            return View();
        }
        [Route("[action]")]
        public IActionResult About()
        {
            ViewBag.Title = "درباره ما";
            return View();
        }

        [Route("[action]")]
        public IActionResult Cooperation()
        {
            ViewBag.Title = "همکاری با ما";
            return View();
        }

        [Route("[action]")]
        public IActionResult OurProducts()
        {
            IEnumerable<ProductResponse> allManufacturedProducts = _productService.GetAllProducts().Where(t => t.isManufactured == true);
            ViewBag.Desc = _productDescService.GetAllProductDesc(); ;
            ViewBag.Title = "تولیدات ما";
            return View(allManufacturedProducts);
        }

        [Route("[action]/{productUrl}")]
        public IActionResult OurProductDetails(string productUrl)
        {
            ProductResponse? Product = _productService.GetAllProducts().FirstOrDefault(t => t.ProductUrl == productUrl);
            if (Product==null)
            {
                throw new ArgumentNullException(nameof(productUrl));
            }
            TotalDTO ProductDetails = new TotalDTO()
            {
                ProductResponses = Product,
                ProductDescResponses= _productDescService.GetProductDescByProductID(Product.ProductId),
                ProductImageResponses=_productImageService.GetProductImagesByProductID(Product.ProductId),
                ProductPropertyResponses=_productPropertyService.GetProductPropertiesByProductID(Product.ProductId),
            };
          
            return View(ProductDetails);
        }
        [Route("[action]")]
        [HttpGet]
        public IActionResult Store()
        {
            IEnumerable<ProductResponse> allProducts = _productService.GetAllProducts().OrderBy(t => t.TypeId);
            ViewBag.Type = _productTypeService.GettAllProductTypes();
            return View(allProducts);
        }
        [Route("[action]/{productUrl}")]
        [HttpGet]
        public IActionResult Product(string productUrl)
        {
            ProductResponse? product = _productService.GetProductByProductUrl(productUrl);
            if (product == null)
            {
                throw new ArgumentException(nameof(productUrl));
            }
            TotalDTO productDTOP = new TotalDTO()
            {
                ProductResponses = product,
                ProductDescResponses = _productDescService.GetProductDescByProductID(product.ProductId),
                ProductImageResponses = _productImageService.GetProductImagesByProductID(product.ProductId),
                ProductPropertyResponses = _productPropertyService.GetProductPropertiesByProductID(product.ProductId)
            };
            return View(productDTOP);
        }
        [Route("[action]/{typeEn}")]
        public IActionResult ProductTypes(string typeEn)
        {
            ViewBag.Title = typeEn;
            Guid? typeID = _productTypeService.GettAllProductTypes()?.FirstOrDefault(t => t.TypeNameEn == typeEn)?.TypeId;
            if (typeID == null)
            {
                throw new ArgumentNullException(nameof(typeEn));
            }
            ViewBag.Type = typeEn;
            List<ProductResponse>? Products = _productService.GetAllProducts().Where(t => t.TypeId == typeID).ToList();
            return View(Products);
        }
    }
}
