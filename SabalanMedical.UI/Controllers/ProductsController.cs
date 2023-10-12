using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using SabalanMedical.UI.Filters.ActionFilters;
using SabalanMedical.UI.Filters.AuthenticationFilters;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO.ProductDescriptionDTO;
using ServiceContracts.DTO.ProductImageDTO;
using ServiceContracts.DTO.ProductPropertyDTO;
using ServiceContracts.DTO.ProductsDTO;

namespace SabalanMedical.Controllers
{
    [Route("[Controller]")]
    [TypeFilter(typeof(TokenAutherizastionFilter))]
    [TypeFilter(typeof(ExecutedLogActionFilter))]
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _environement;
        private readonly IProductService _productService;
        private readonly IProductTypeService _productTypeService;
        private readonly IProductImageService _productImageService;
        private readonly IProductDescService _productDescService;
        private readonly IProductPropertyService _productPropertyService;
        private readonly ILogger<ProductsController> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public ProductsController(IProductService productService,
            IProductTypeService productTypeService,
            IProductImageService productImageService,
            IProductDescService productDescService,
            IProductPropertyService productPropertyService,
            IWebHostEnvironment environment,
            ILogger<ProductsController> logger, IDiagnosticContext diagnosticContext)
        {
            _productService = productService;
            _productTypeService = productTypeService;
            _productImageService = productImageService;
            _productDescService = productDescService;
            _productPropertyService = productPropertyService;
            _environement = environment;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        #region Products
        [Route("[action]")]
        [TypeFilter(typeof(TypeListActionFilter))]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index action was executed!");
            List<ProductResponse> productResponses = await _productService.GetAllProducts();
            return View(productResponses);
        }

        [Route("[action]")]
        [HttpGet]
        [TypeFilter(typeof(TypeListActionFilter))]
        public async Task<IActionResult> AddProduct()
        {
            _logger.LogInformation("AddProduct Action get method executed");
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CheckNullRequestValidationActionFilter), Order = 1)]
        [TypeFilter(typeof(CheckValidationActionFilter), Order = 2)]
        public async Task<IActionResult> AddProduct(ProductAddRequest request)
        {
            _logger.LogInformation("AddProduct action Post method executed");
            await _productService.AddProduct(request);
            _diagnosticContext.Set("Addrequest", request);
            return RedirectToAction("Index", "Products");
        }

        [Route("[action]/{Id}")]
        [HttpGet]
        [TypeFilter(typeof(GuidValidateActionFilter),Order =1)]
        [TypeFilter(typeof(TypeListActionFilter),Order =3)]
        [TypeFilter(typeof(CheckGetProductByIdActionFilter),Order =2)]
        public async Task<IActionResult> EditProduct(Guid Id)
        {
            ProductResponse? response = await _productService.GetProductById(Id);
            return View(response.ToProductUpdateRequest());
        }

        [Route("[action]/{Id}")]//Id: method get and post method has to have the same route
        [HttpPost]
        [TypeFilter(typeof(CheckNullRequestValidationActionFilter))]
        [TypeFilter(typeof(CheckValidationActionFilter))]
        public async Task<IActionResult> EditProduct(ProductUpdateRequest request)
        {
            if (_productService.GetProductById(request.Id) == null)
            {
                _logger.LogError("requset is null");
                return RedirectToAction("index", "Products");
            }
            await _productService.UpdateProduct(request);
            _logger.LogInformation("Redirect to Index method");
            return RedirectToAction("index", "Products");
        }

        [Route("[action]/{Id}")]
        [HttpGet]
        [TypeFilter(typeof(GuidValidateActionFilter))]
        [TypeFilter(typeof(CheckGetProductByIdActionFilter))]
        public async Task<IActionResult> DeleteProduct(Guid Id)
        {
            ProductResponse? response = await _productService.GetProductById(Id);
/*            if (response == null)
            {
                _logger.LogError($"No product was found for id={Id}");
                return RedirectToAction("Index", "Products");
            }*/
            return View(response);
        }

        [Route("[action]/{Id}")]
        [HttpPost]
        [TypeFilter(typeof(CheckNullRequestValidationActionFilter))]
        public async Task<IActionResult> DeleteProduct(ProductResponse request)
        {
            _logger.LogInformation("DeleteProduct action post method executed");
            ProductResponse? response = await _productService.GetProductById(request.Id);
            if (response == null)
            {
                _logger.LogError($"no product was found for id={request.Id} ");
                return RedirectToAction("Index", "Products");
            }
            await _productService.DeleteProduct(request.Id);
            _logger.LogDebug($"{response.ProductNameEn} was deleted");
            return RedirectToAction("Index", "Products");
        }

        [Route("{action}")]
        [HttpPost]
        public async Task<ActionResult> GetFilteredProducts(Guid typeId, string searchBy, string searchKey)
        {
            _logger.LogDebug($"typeId:{typeId}--seachBy={searchBy}--searchKey={searchKey}");
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
        #endregion

        #region Description
        [Route("[action]/{Id}")]
        [HttpGet]
        [TypeFilter(typeof(GuidValidateActionFilter))]
        public async Task<IActionResult> ProductDescriptions(Guid Id)
        {
            ProductResponse? product = await _productService.GetProductById(Id);
            if (product is null)
            {
                _logger.LogError($"No product was found for id {Id}");
                throw new ArgumentException(nameof(Id));
            }
            return View(product);
        }


        [Route("[action]/{Id}")]//Id=ProductId
        [HttpGet]
        [TypeFilter(typeof(GuidValidateActionFilter))]
        public async Task<IActionResult> AddDescription(Guid Id)
        {
            ProductResponse? Product = await _productService.GetProductById(Id);
            if (Product == null)
            {
                _logger.LogError($"No product was found for productId={Id} ");
                return RedirectToAction("Index");
            }
            ViewBag.Product = Product;
            return View();
        }

        [Route("[action]/{Id}")]//Id=ProductId
        [HttpPost]
        [TypeFilter(typeof(CheckNullRequestValidationActionFilter))]
        public async Task<IActionResult> AddDescription(ProductDescAddRequest request)
        {
            /*            if (request == null)
                        {
                            _logger.LogError("request is null");
                            return RedirectToAction("Index");//redirectToAction Error
                        }*/
            if (!ModelState.IsValid)
            {
                _logger.LogError("validation for request failed");
                ProductResponse? Product = await _productService.GetProductById(request.ProductID);
                ViewBag.Product = Product;
                return View();
            }
            await _productDescService.AddProductDesc(request);
            return RedirectToAction("ProductDescriptions", new { Id = request.ProductID });
        }


        [Route("[action]/Id")]//Id=DescriptionId
        [HttpGet]
        [TypeFilter(typeof(GuidValidateActionFilter))]
        public async Task<IActionResult> DeleteDescription(Guid ID)
        {
            ProductDescResponse? desc = await _productDescService.GetProductDescByDescID(ID);
            if (desc == null)
            {
                _logger.LogError($"No desc was found for descId={ID}");
                return RedirectToAction("Index");
            }
            return View(desc);
        }

        [Route("[action]/Id")]//Id=DescriptionId
        [HttpPost]
        [TypeFilter(typeof(CheckNullRequestValidationActionFilter))]
        public IActionResult DeleteDescription(ProductDescResponse request)
        {
            if (_productDescService.GetProductDescByDescID(request.Id) == null)
            {
                _logger.LogError("desc is null or not fond in database");
                return RedirectToAction("Index");
            }
            _productDescService.DeleteProductDesc(request.Id);
            _logger.LogDebug($"description with id={request.Id} was deleted");
            return RedirectToAction("ProductDescriptions", new { Id = request.ProductID });
        }

        [Route("[action]/Id")]//Id=DescriptionId
        [HttpGet]
        [TypeFilter(typeof(GuidValidateActionFilter))]
        public async Task<IActionResult> EditDescription(Guid Id)
        {
            ProductDescResponse? desc = await _productDescService.GetProductDescByDescID(Id);
            if (desc == null)
            {
                _logger.LogError("Id is null");
                return RedirectToAction("Index");
            }
            return View(desc.ToProductDescUpdateRequest());
        }

        [Route("[action]/Id")]//Id=DescriptionId
        [HttpPost]
        public async Task<IActionResult> EditDescription(ProductDescResponse requset)
        {
            ProductDescResponse response = await _productDescService.GetProductDescByDescID(requset.Id);
            if (response is null)
            {
                _logger.LogError($"No request for id={requset.Id} was found");
                return RedirectToAction("index");
            }
            response.ProductID = requset.ProductID;
            response.Description = requset.Description;
            response.DescTitle = requset.DescTitle;
            await _productDescService.UpdateProductDesc(requset.ToProductDescUpdateRequest());
            _logger.LogDebug($"request with id={requset.Id} was updated");
            return RedirectToAction("ProductDescriptions", new { Id = requset.ProductID });
        }
        #endregion

        #region Images
        [Route("[action]/{Id}")]//Id=ProductId
        [HttpGet]
        [TypeFilter(typeof(GuidValidateActionFilter))]
        public async Task<IActionResult> ProductImages(Guid Id)
        {
            ProductResponse? product = await _productService.GetProductById(Id);
            if (product == null)
            {
                _logger.LogError($"No product was found for id={Id}");
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [Route("[action]/Id")]//Id=ImageId
        [HttpGet]
        [TypeFilter(typeof(GuidValidateActionFilter))]
        public async Task<IActionResult> DeleteImage(Guid Id)
        {
            ProductImageResponse? Image = await _productImageService.GetProductImageByImageID(Id);
            if (Image == null)
            {
                _logger.LogError($"No Image was found for id={Id}");
                return RedirectToAction("Index");
            }
            await _productImageService.DeleteProductImage(Id);
            _logger.LogDebug($"Image for id={Id} was deleted from DB and ready to be deleted from server");
            string fileName = Image.ImageUrl;
            string path = Path.Combine(_environement.WebRootPath, $"images/products/{fileName}");
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();

            }
            return RedirectToAction("ProductImages", new { Id = Image.ProductID });
        }

        [Route("[action]/Id")]//Id=ProductId
        [HttpPost]
        [TypeFilter(typeof(GuidValidateActionFilter))]
        public async Task<IActionResult> AddImage(ProductImageAddRequest? image, IFormFile? imageFile)
        {
            if (image == null || imageFile == null || imageFile.FileName == null || imageFile.Length < 10)
            {
                _logger.LogError("File is not allowed");
                return RedirectToAction("index");
            }
            string[] allowedImageTypes = { "image/jpeg", "image/png", "image/gif" }; // List of allowed image types
            if (!allowedImageTypes.Contains(imageFile.ContentType))
            {
                _logger.LogError("File is not an image");
                return RedirectToAction("index");
            }
            ProductResponse? productResponse = await _productService.GetProductById(image.ProductID);
            if (productResponse == null)
            {
                _logger.LogError($"No product was found for id={image.ProductID}");
                return RedirectToAction("index");
            }

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
                ProductID = image.ProductID,
                ImageUrl = fileName
            };
            _productImageService.AddProductImage(request);

            return RedirectToAction("ProductImages", new { Id = productResponse.Id });
        }
        #endregion

        #region Properties
        [Route("[action]/{ID}")]//Id=ProductId
        [HttpGet]
        [TypeFilter(typeof(GuidValidateActionFilter))]
        public async Task<IActionResult> ProductProperties(Guid Id)
        {
            var productResponses = await _productService.GetProductById(Id);
            if (productResponses == null)
            {
                _logger.LogError($"No Product was found for Id={Id}");
                return RedirectToAction("Index");
            }
            return View(productResponses);
        }

        [Route("[action]/{ID}")]///Id=ProductId
        [HttpGet]
        [TypeFilter(typeof(GuidValidateActionFilter))]
        public async Task<IActionResult> AddProperty(Guid Id)
        {
            var product = await _productService.GetProductById(Id);
            if (product == null)
            {
                _logger.LogError($"No product was found for id={Id}");
                return RedirectToAction("index");
            }
            ViewBag.Product = product;
            return View();
        }

        [Route("[action]/{ID}")]//Id=ProductId
        [HttpPost]
        [TypeFilter(typeof(CheckNullRequestValidationActionFilter))]
        public async Task<IActionResult> AddProperty(ProductPropertyAddRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("validation was failed");
                return View();
            }

            await _productPropertyService.AddProductProperty(request);
            _logger.LogDebug($"requset {request.ToString()} was added to DB");
            return RedirectToAction("ProductProperties", new { Id = request.ProductID });
        }

        [Route("[action]/{Id}")]//Id=PropertyId
        [HttpGet]
        [TypeFilter(typeof(GuidValidateActionFilter))]
        public async Task<IActionResult> EditProperty(Guid Id)
        {
            var request = await _productPropertyService.GetProductPropertyByPropertyID(Id);
            if (request == null)
            {
                _logger.LogError($"No request for id ={Id} was found");
                return RedirectToAction("index");
            }
            return View(request.ToProductPropertyUpdateRequest());
        }

        [Route("[action]/{Id}")]//Id=PropertyId
        [HttpPost]
        [TypeFilter(typeof(CheckNullRequestValidationActionFilter))]
        public async Task<IActionResult> EditProperty(ProductPropertyUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Validation was failed");
                return View();
            }
            var property = await _productPropertyService.GetProductPropertyByPropertyID(request.Id);
            if (property == null)
            {
                _logger.LogError($"No request for id={request.Id} was found ");
                return RedirectToAction("index");
            }
            property.ProductID = request.ProductID;
            property.PropertyDetail = request.PropertyDetail;
            property.PropertyTitle = request.PropertyTitle;
            await _productPropertyService.UpdateProductProperty(request);
            _logger.LogDebug($"Property with id={request.Id} was updated");
            return RedirectToAction("ProductProperties", new { Id = request.ProductID });
        }

        [Route("[action]/{Id}")]//Id=PropertyId
        [HttpGet]
        [TypeFilter(typeof(GuidValidateActionFilter))]
        public async Task<IActionResult> DeleteProperty(Guid Id)
        {
            ProductPropertyResponse property = await _productPropertyService.GetProductPropertyByPropertyID(Id);
            if (property == null)
            {
                _logger.LogError($"No request with id={Id} was found");
                return RedirectToAction("index");
            }
            return View(property);
        }

        [Route("[action]/{Id}")]//Id=PropertyId
        [HttpPost]
        public async Task<IActionResult> DeleteProperty(ProductPropertyResponse request)
        {
            var propertyResponse = await _productPropertyService.GetProductPropertyByPropertyID(request.Id);
            if (propertyResponse == null)
            {
                _logger.LogError($"No request with id={request.Id} was found");
                return RedirectToAction("index");
            }
            await _productPropertyService.DeleteProductProperty(propertyResponse.Id);
            _logger.LogDebug($"Property with id={request.Id} was deleted");
            return RedirectToAction("ProductProperties", new { Id = request.ProductID });
        }
        #endregion

    }
}
