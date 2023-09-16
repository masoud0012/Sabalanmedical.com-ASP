using Entities;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.ProductDescriptionDTO;
using ServiceContracts.DTO.ProductImageDTO;
using ServiceContracts.DTO.ProductPropertyDTO;
using ServiceContracts.DTO.ProductsDTO;
using ServiceContracts.DTO.ProductTypeDTO;
using System.IO;

namespace SabalanMedical.Controllers
{
    [Route("[Controller]")]
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _environement;
        private readonly IProductService _productService;
        private readonly IProductTypeService _productTypeService;
        private readonly IProductImageService _productImageService;
        private readonly IProductDescService _productDescService;
        private readonly IProductPropertyService _productPropertyService;
        public ProductsController(IProductService productService,
            IProductTypeService productTypeService,
            IProductImageService productImageService,
            IProductDescService productDescService,
            IProductPropertyService productPropertyService,
            IWebHostEnvironment environment)
        {
            _productService = productService;
            _productTypeService = productTypeService;
            _productImageService = productImageService;
            _productDescService = productDescService;
            _productPropertyService = productPropertyService;
            _environement = environment;
        }
        #region Products
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            List<ProductResponse> productResponses = await _productService.GetAllProducts();
            List<ProductTypeResponse> types = await _productTypeService.GetAllProductTypes();
            ViewBag.TypeList = types?.Select(t => new SelectListItem() { Text = t.TypeNameEn, Value = t.TypeId.ToString() });
            return View(productResponses);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            var allTypes = await _productTypeService.GetAllProductTypes();

            ViewBag.TypeList = allTypes?.Select(t => new SelectListItem() { Text = t.TypeNameEn, Value = t.TypeId.ToString() });
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductAddRequest request)
        {
            if (!ModelState.IsValid)
            {

                ViewBag.Types = await _productTypeService.GetAllProductTypes();

                ViewBag.Errors = ModelState.Values.SelectMany(t => t.Errors).Select(t => t.ErrorMessage).ToList();
                return View();
            }
            await _productService.AddProduct(request);
            return RedirectToAction("Index", "Products");
        }
        [Route("[action]/{ProductId}")]
        [HttpGet]
        public async Task<IActionResult> EditProduct(Guid ProductId)
        {
            ProductResponse? response = await _productService.GetProductById(ProductId);
            if (response is null)
            {
                return RedirectToAction("index", "Products");
            }
            var allTypes = await _productTypeService.GetAllProductTypes();
            ViewBag.TypeList = allTypes?.Select(t => new SelectListItem() { Text = t.TypeNameEn, Value = t.TypeId.ToString() });
            return View(response.ToProductUpdateRequest());
        }
        [Route("[action]/{ProductId}")]
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductUpdateRequest request)
        {
            if (request is null || _productService.GetProductById(request.ProductID) == null)
            {
                return RedirectToAction("index", "Products");
            }
            if (ModelState.IsValid)
            {
                await _productService.UpdateProduct(request);
                return RedirectToAction("index", "Products");
            }
            else
            {
                ViewBag.Errors = ModelState.Values.SelectMany(t => t.Errors).Select(t => t.ErrorMessage).ToList();
                var allTypes = await _productTypeService.GetAllProductTypes();
                ViewBag.TypeList = allTypes?.Select(t =>
                new SelectListItem() { Text = t.TypeNameEn, Value = t.TypeId.ToString() });
                return RedirectToAction("index", "Products");
            }
        }

        [Route("[action]/{ProductId}")]
        [HttpGet]
        public async Task<IActionResult> DeleteProduct(Guid? ProductId)
        {
            if (ProductId == null)
            {
                return RedirectToAction("Index", "Products");
            }
            ProductResponse? response = await _productService.GetProductById(ProductId);
            if (response == null)
            {
                return RedirectToAction("Index", "Products");
            }
            return View(response);
        }

        [Route("[action]/{ProductId}")]
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(ProductResponse? product)
        {
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            ProductResponse? response = await _productService.GetProductById(product.ProductId);
            if (response == null)
            {
                return RedirectToAction("Index", "Products");
            }

            await _productService.DeleteProduct(product.ProductId);

            return RedirectToAction("Index", "Products");
        }
        #endregion

        #region Description
        [Route("[action]/{productID}")]
        [HttpGet]
        public async Task<IActionResult> ProductDescriptions(Guid productID)
        {
            List<ProductDescResponse>? descs = await _productDescService.GetProductDescByProductID(productID);
            TotalDTO dto = new TotalDTO()
            {
                ProductResponses = await _productService.GetProductById(productID),
                ProductDescResponses = descs
            };
            return View(dto);
        }


        [Route("[action]/{ProductId}")]
        [HttpGet]
        public async Task<IActionResult> AddDescription(Guid ProductId)
        {
            ProductResponse? Product = await _productService.GetProductById(ProductId);
            if (Product == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Product = Product;
            return View();
        }

        [Route("[action]/{ProductId}")]
        [HttpPost]
        public async Task<IActionResult> AddDescription(ProductDescAddRequest Desc)
        {
            if (!ModelState.IsValid)
            {
                ProductResponse? Product = await _productService.GetProductById(Desc.ProductID);
                ViewBag.Product = Product;
                return View();
            }
            await _productDescService.AddProductDesc(Desc);
            return RedirectToAction("ProductDescriptions", new { productID = Desc.ProductID });
        }


        [Route("[action]/DescriptionId")]
        [HttpGet]
        public async Task<IActionResult> DeleteDescription(Guid DescriptionId)
        {
            ProductDescResponse? desc = await _productDescService.GetProductDescByDescID(DescriptionId);
            if (desc == null)
            {
                return RedirectToAction("Index");
            }
            return View(desc);
        }

        [Route("[action]/DescriptionId")]
        [HttpPost]
        public IActionResult DeleteDescription(ProductDescResponse Description)
        {
            if (Description == null || _productDescService.GetProductDescByDescID(Description.DesctiptionID) == null)
            {
                return RedirectToAction("Index");
            }
            _productDescService.DeleteProductDesc(Description.DesctiptionID);
            return RedirectToAction("ProductDescriptions", new { productID = Description.ProductID });
        }

        [Route("[action]/DescriptionId")]
        [HttpGet]
        public async Task<IActionResult> EditDescription(Guid DescriptionId)
        {
            ProductDescResponse? desc = await _productDescService.GetProductDescByDescID(DescriptionId);
            if (desc == null)
            {
                return RedirectToAction("Index");
            }
            return View(desc.ToProductDescUpdateRequest());
        }

        [Route("[action]/DescriptionId")]
        [HttpPost]
        public IActionResult EditDescription(ProductDescResponse Desc)
        {
            if (Desc == null)
            {
                return RedirectToAction("Index");
            }
            _productDescService.UpdateProductDesc(Desc.ToProductDescUpdateRequest());
            return RedirectToAction("ProductDescriptions", new { productID = Desc.ProductID });
        }
        #endregion

        #region Images
        [Route("[action]/{ProductId}")]
        public async Task<IActionResult> ProductImages(Guid? ProductId)
        {
            if (ProductId == null)
            {
                return RedirectToAction("Index");
            }
            ProductResponse? product = await _productService.GetProductById(ProductId);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [Route("[action]/ImageId")]
        [HttpGet]
        public async Task<IActionResult> DeleteImage(Guid? ImageId)
        {
            if (ImageId == null)
            {
                throw new ArgumentNullException(nameof(ImageId));
            }
            ProductImageResponse? Image = await _productImageService.GetProductImageByImageID(ImageId);
            if (Image == null)
            {
                return RedirectToAction("Index");
            }
            await _productImageService.DeleteProductImage(ImageId);
            string? fileName = Image.ImageUrl;
            if (fileName == null)
            {
                return RedirectToAction("Index");
            }
            string path = Path.Combine(_environement.WebRootPath, $"images/products/{fileName}");
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();

            }
            return RedirectToAction("ProductImages", new { ProductId = Image.ProductID });
        }

        [Route("[action]/ProductId")]
        [HttpPost]
        public async Task<IActionResult> AddImage(ProductImageAddRequest Image, IFormFile imageFile)
        {
            ProductResponse? productResponse = await _productService.GetProductById(Image.ProductID);
            if (productResponse == null)
            {
                return RedirectToAction("index");
            }
            if (imageFile.Length == 0 || imageFile is null)
            {
                return RedirectToAction("ProductImages", new { ProductId = productResponse.ProductId });
            }
            if (imageFile.Length > 0)
            {
                Random random = new Random();
                string[] exctension = imageFile.FileName.Split('.');
                string extension = exctension[exctension.Length - 1];
                string fileName = productResponse.ProductUrl + "-" + random.Next() + "." + extension;
                string path = Path.Combine(_environement.WebRootPath, $"images/products/{fileName}");
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                ProductImageAddRequest request = new ProductImageAddRequest()
                {
                    ProductID = Image.ProductID,
                    ImageUrl = fileName
                };
                _productImageService.AddProductImage(request);
            }
            return RedirectToAction("ProductImages", new { ProductId = productResponse.ProductId });
        }
        #endregion
        #region Properties
        [Route("[action]/{productID}")]
        [HttpGet]
        public async Task<IActionResult> ProductProperties(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                return RedirectToAction("Index");
            }
            var productResponses = await _productService.GetProductById(productId);
            List<ProductPropertyResponse>? response = await _productPropertyService.GetProductPropertiesByProductID(productId);
            TotalDTO dto = new TotalDTO()
            {
                ProductResponses = productResponses,
                ProductPropertyResponses = response
            };

            return View(dto);
        }

        [Route("[action]/{productId}")]
        [HttpGet]
        public async Task<IActionResult> AddProperty(Guid? productId)
        {
            if (productId == Guid.Empty)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Product = await _productService.GetProductById(productId);
            return View();
        }

        [Route("[action]/{productId}")]
        [HttpPost]
        public async Task<IActionResult> AddProperty(ProductPropertyAddRequest request)
        {
            if (request == null)
            {
                return RedirectToAction("index");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _productPropertyService.AddProductProperty(request);
            return RedirectToAction("ProductProperties", new { productId = request.ProductID });
        }

        [Route("[action]/{propertyId}")]
        [HttpGet]
        public async Task<IActionResult> EditProperty(Guid? propertyId)
        {
            if (propertyId == null)
            {
                return RedirectToAction("index");
            }
            var request = await _productPropertyService.GetProductPropertyByPropertyID(propertyId);
            return View(request.ToProductPropertyUpdateRequest());
        }

        [Route("[action]/{propertyId}")]
        [HttpPost]
        public async Task<IActionResult> EditProperty(ProductPropertyUpdateRequest? request)
        {
            if (request == null)
            {
                return RedirectToAction("index");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _productPropertyService.UpdateProductProperty(request);
            return RedirectToAction("ProductProperties", new { productId = request.ProductID });
        }

        [Route("[action]/{propertyId}")]
        [HttpGet]
        public async Task<IActionResult> DeleteProperty(Guid? propertyId)
        {
            if (propertyId == null)
            {
                return RedirectToAction("index");
            }
            ProductPropertyResponse request = await _productPropertyService.GetProductPropertyByPropertyID(propertyId);

            return View(request);
        }

        [Route("[action]/{propertyId}")]
        [HttpPost]
        public async Task<IActionResult> DeleteProperty(ProductPropertyResponse response)
        {
            if (response == null)
            {
                return RedirectToAction("index");
            }
            var request = await _productPropertyService.GetProductPropertyByPropertyID(response.propertyID);
            if (request == null)
            {
                return RedirectToAction("index");
            }
            await _productPropertyService.DeleteProductProperty(request.propertyID);
            return RedirectToAction("ProductProperties", new { productId = request.ProductID });
        }
        #endregion


        [Route("{action}")]
        [HttpPost]
        public async Task<ActionResult> GetFilteredProducts(Guid typeId, string searchBy, string searchKey)
        {

            return ViewComponent("ProductTable", new { typeID = typeId, searchBy = searchBy, searchKey = searchKey });
        }

        [Route("[action]")]
        public async Task<IActionResult> ProductToPDF()
        {
            List<ProductResponse> products = await _productService.GetAllProducts();
            return new ViewAsPdf(products);
        }
        [Route("[action]")]
        public async Task<IActionResult> ProductToCSV()
        {
            MemoryStream memoryStream = await _productService.ProductToCsv();
            return File(memoryStream, "application/octet-stream", "product.csv");
        }
    }
}
