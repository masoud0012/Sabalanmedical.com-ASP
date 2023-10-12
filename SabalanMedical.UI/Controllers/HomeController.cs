using Microsoft.AspNetCore.Mvc;
using SabalanMedical.UI.Filters.ActionFilters;
using SabalanMedical.UI.Filters.ResultFilters;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO.ProductsDTO;

namespace SabalanMedical.Controllers
{
    [TypeFilter(typeof(ExecutedLogActionFilter))]
    [TypeFilter(typeof(TokenResltFilter))]
    public class HomeController : Controller
    {
        private readonly IProductTypeService _productTypeService;
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        private readonly IProductDescService _productDescService;
        private readonly IProductPropertyService _productPropertyService;
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(IProductTypeService productTypeService,
            IProductService productService,
            IProductImageService productImageService,
            IProductDescService productDescService,
            IProductPropertyService productPropertyService,
            ILogger<HomeController> logger)
        {
            _logger = logger;
            _productTypeService = productTypeService;
            _productService = productService;
            _productImageService = productImageService;
            _productDescService = productDescService;
            _productPropertyService = productPropertyService;
        }

        [Route("/")]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            List<ProductResponse> allProducts = await _productService.GetAllProducts();
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
        public async Task<IActionResult> OurProducts()
        {
            List<ProductResponse> allProducts = await _productService.GetAllProducts();
            IEnumerable<ProductResponse> allManufacturedProducts = allProducts.Where(t => t.isManufactured == true);
            _logger.LogDebug($"{allManufacturedProducts.Count()} was founde as Manufactured products");
            ViewBag.Title = "تولیدات ما";
            return View(allManufacturedProducts);
        }

        [Route("[action]/{productUrl}")]
        public async Task<IActionResult> OurProductDetails(string productUrl)
        {
            _logger.LogDebug($"Try to finde info for Product Url: {productUrl}");
            ProductResponse? Product = await _productService.GetProductByProductUrl(productUrl);
            if (Product == null)
            {
                _logger.LogError($"No product was fond for {productUrl}");
                throw new ArgumentNullException(nameof(productUrl));
            }
            return View(Product);
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Store()
        {
            List<ProductResponse> allProducts = await _productService.GetAllProducts();
            _logger.LogDebug($"{allProducts.Count()} products was found");
            return View(allProducts.OrderBy(t => t.TypeId));
        }
        [Route("[action]/{productUrl}")]
        [HttpGet]
        public async Task<IActionResult> Product(string productUrl)
        {
            _logger.LogDebug($"product url is :{productUrl}");
            if (productUrl == null)
            {
                _logger.LogError("Url is null!");
                throw new ArgumentNullException(nameof(productUrl));
            }
            ProductResponse? product = await _productService.GetProductByProductUrl(productUrl);
            if (product == null)
            {
                _logger.LogError($"No product was found for url: {productUrl}");
                throw new ArgumentException(nameof(productUrl));
            }
            _logger.LogDebug($"product for the url: {productUrl} is  :{product.ProductNameEn}");
            return View(product);
        }
        [Route("[action]/{typeEn}")]
        public async Task<IActionResult> ProductTypes(string? typeEn)
        {
            _logger.LogDebug($"Type is: {typeEn}");
            if (typeEn is null)
            {
                _logger.LogError($"No Id found for typeEn :{typeEn}");
                throw new ArgumentNullException(nameof(typeEn));
            }
            List<ProductResponse>? products = await _productService.GetProductByTypeName(typeEn);
            if (products is null)
            {
                _logger.LogError($"No prodcts was found for typeEn :{typeEn}");
                throw new ArgumentNullException(nameof(typeEn));
            }
            _logger.LogDebug($"{products.Count} products was found for type of {typeEn}");
            ViewBag.Title = typeEn;
            ViewBag.Type = typeEn;
            return View(products);
        }
    }
}
