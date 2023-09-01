using Entities;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Rotativa.AspNetCore;
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
            ViewBag.Title = "تولیدات ما";
            return View(allManufacturedProducts);
        }

        [Route("[action]/{productUrl}")]
        public async Task<IActionResult> OurProductDetails(string productUrl)
        {
            List<ProductResponse> allProducts = await _productService.GetAllProducts();
            ProductResponse? Product = allProducts.FirstOrDefault(t => t.ProductUrl == productUrl);
            if (Product == null)
            {
                throw new ArgumentNullException(nameof(productUrl));
            }

            return View(Product);
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Store()
        {
            List<ProductResponse> allProducts = await _productService.GetAllProducts();
            return View(allProducts.OrderBy(t => t.TypeId));
        }
        [Route("[action]/{productUrl}")]
        [HttpGet]
        public async Task<IActionResult> Product(string productUrl)
        {
            if (productUrl == null)
            {
                throw new ArgumentNullException(nameof(productUrl));
            }
            ProductResponse? product = await _productService.GetProductByProductUrl(productUrl);
            if (product == null)
            {
                throw new ArgumentException(nameof(productUrl));
            }
            return View(product);
        }
        [Route("[action]/{typeEn}")]
        public async Task<IActionResult> ProductTypes(string typeEn)
        {
            ViewBag.Title = typeEn;
            var allTypes = await _productTypeService.GetAllProductTypes();
            Guid? typeID = allTypes.FirstOrDefault(t => t.TypeNameEn == typeEn)?.TypeId;
            if (typeID == null)
            {
                throw new ArgumentNullException(nameof(typeEn));
            }
            ViewBag.Type = typeEn;
            List<ProductResponse>? allProducts = await _productService.GetAllProducts();
            List<ProductResponse>? Products = allProducts.Where(t => t.TypeId == typeID).ToList();
            return View(Products);
        }

        [Route("[action]")]
        public void TestSelenium()
        {
            var chromOption = new ChromeOptions();
            chromOption.AddArgument("--headless");
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://sabalanmedical.ir/");

            //IWebElement searchBox = driver.FindElement(By.ClassName("nav-fill "));

            //searchBox.SendKeys("Selenium WebDriver");
            //searchBox.SendKeys(Keys.Enter);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // Define your jQuery code
            string jqueryCode = "$('.ourOptions i').css('color', '#9999');";

            // Execute the jQuery code
            js.ExecuteScript(jqueryCode);
        }

    }
}
