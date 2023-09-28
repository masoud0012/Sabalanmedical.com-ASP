using Entities;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductService productService,
            IProductTypeService productTypeService,
            IProductImageService productImageService,
            IProductDescService productDescService,
            IProductPropertyService productPropertyService,
            IWebHostEnvironment environment,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _productTypeService = productTypeService;
            _productImageService = productImageService;
            _productDescService = productDescService;
            _productPropertyService = productPropertyService;
            _environement = environment;
            _logger = logger;
        }
        #region Products
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index action was executed!");
            List<ProductResponse> productResponses = await _productService.GetAllProducts();
            List<ProductResponse> distinctTypes = productResponses.DistinctBy(t => t.TypeId).ToList();
            ViewBag.TypeList = distinctTypes?.Select(t => new SelectListItem() { Text = t.productType?.TypeNameEN, Value = t.TypeId.ToString() });
            return View(productResponses);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            _logger.LogInformation("AddProduct Action get method executed");
            var allTypes = await _productTypeService.GetAllProductTypes();

            ViewBag.TypeList = allTypes?.Select(t => new SelectListItem() { Text = t.TypeNameEn, Value = t.Id.ToString() });
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductAddRequest request)
        {
            _logger.LogInformation("AddProduct action Post method executed");
            if (request is null)
            {
                _logger.LogError("requset is null");
                throw new ArgumentNullException(nameof(request));
            }
            _logger.LogDebug($"-{request?.ProductNameEn}- received to be checked to be added");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Validation for the product add reaquest object failed");
                ViewBag.Types = await _productTypeService.GetAllProductTypes();
                ViewBag.Errors = ModelState.Values.SelectMany(t => t.Errors).Select(t => t.ErrorMessage).ToList();
                return View();
            }
            await _productService.AddProduct(request);
            _logger.LogDebug($"the request seccesfully added to databse:{request.ToString()}");
            return RedirectToAction("Index", "Products");
        }

        [Route("[action]/{ProductId}")]
        [HttpGet]
        public async Task<IActionResult> EditProduct(Guid? ProductId)
        {
            _logger.LogInformation("Edit action get method executed!");
            if (ProductId == null || ProductId == Guid.Empty)
            {
                _logger.LogError("Id is null");
                throw new ArgumentNullException(nameof(ProductId));
            }
            ProductResponse? response = await _productService.GetProductById(ProductId);
            if (response is null)
            {
                _logger.LogError($"No product was found for id= {ProductId} to be updated");
                return RedirectToAction("index", "Products");
            }
            var allTypes = await _productTypeService.GetAllProductTypes();
            ViewBag.TypeList = allTypes?.Select(t => new SelectListItem() { Text = t.TypeNameEn, Value = t.Id.ToString() });
            return View(response.ToProductUpdateRequest());
        }

        [Route("[action]/{ProductId}")]
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductUpdateRequest request)
        {
            _logger.LogInformation("EditProduct post action method executed");
            if (request is null || _productService.GetProductById(request.Id) == null)
            {
                _logger.LogError("requset is null");
                return RedirectToAction("index", "Products");
            }
            if (ModelState.IsValid)
            {
                await _productService.UpdateProduct(request);
                _logger.LogInformation("Redirect to Index method");
                return RedirectToAction("index", "Products");
            }
            _logger.LogError("validation for the request is failed");
            return View(request);
        }

        [Route("[action]/{ProductId}")]
        [HttpGet]
        public async Task<IActionResult> DeleteProduct(Guid? ProductId)
        {
            _logger.LogInformation("Delete action get method executed");
            if (ProductId == null)
            {
                _logger.LogError("Id is null");
                return RedirectToAction("Index", "Products");
            }
            ProductResponse? response = await _productService.GetProductById(ProductId);
            if (response == null)
            {
                _logger.LogError($"No product was found for id={ProductId}");
                return RedirectToAction("Index", "Products");
            }
            return View(response);
        }

        [Route("[action]/{ProductId}")]
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(ProductResponse? product)
        {
            _logger.LogInformation("DeleteProduct action post method executed");
            if (product == null)
            {
                _logger.LogError("id is null");
                return RedirectToAction("Index", "Products");
            }
            ProductResponse? response = await _productService.GetProductById(product.Id);
            if (response == null)
            {
                _logger.LogError($"no product was found for id={product.Id} ");
                return RedirectToAction("Index", "Products");
            }

            await _productService.DeleteProduct(product.Id);
            _logger.LogDebug($"{response.ProductNameEn} was deleted");
            return RedirectToAction("Index", "Products");
        }

        [Route("{action}")]
        [HttpPost]
        public async Task<ActionResult> GetFilteredProducts(Guid typeId, string searchBy, string searchKey)
        {
            _logger.LogInformation("GetFilteredProducts action method executed");
            _logger.LogDebug($"typeId:{typeId}--seachBy={searchBy}--searchKey={searchKey}");
            return ViewComponent("ProductTable", new { typeID = typeId, searchBy = searchBy, searchKey = searchKey });
        }

        [Route("[action]")]
        public async Task<IActionResult> ProductToPDF()
        {
            _logger.LogInformation("ProductToPDF action method executed");
            List<ProductResponse> products = await _productService.GetAllProducts();
            return new ViewAsPdf(products);
        }

        [Route("[action]")]
        public async Task<IActionResult> ProductToCSV()
        {
            _logger.LogInformation("ProductToCSV method executed");
            MemoryStream memoryStream = await _productService.ProductToCsv();
            return File(memoryStream, "application/octet-stream", "product.csv");
        }
        #endregion

        #region Description
        [Route("[action]/{productID}")]
        [HttpGet]
        public async Task<IActionResult> ProductDescriptions(Guid? productID)
        {
            _logger.LogInformation("ProductDescriptions action method executed");
            if (productID == null || productID == Guid.Empty)
            {
                _logger.LogError("ProductId is null");
                throw new ArgumentNullException(nameof(productID));
            }
            ProductResponse? product = await _productService.GetProductById(productID);
            if (product is null)
            {
                _logger.LogError($"No product was found for id {productID}");
                throw new ArgumentException(nameof(productID));
            }
            return View(product);
        }


        [Route("[action]/{ProductId}")]
        [HttpGet]
        public async Task<IActionResult> AddDescription(Guid? ProductId)
        {
            _logger.LogInformation("AddDescription get method executed");
            if (ProductId == null || ProductId == Guid.Empty)
            {
                _logger.LogError("ProductId is null");
                throw new ArgumentNullException(nameof(ProductId));
            }
            ProductResponse? Product = await _productService.GetProductById(ProductId);
            if (Product == null)
            {
                _logger.LogError($"No product was found for productId={ProductId} ");
                return RedirectToAction("Index");
            }
            ViewBag.Product = Product;
            return View();
        }

        [Route("[action]/{ProductId}")]
        [HttpPost]
        public async Task<IActionResult> AddDescription(ProductDescAddRequest? Desc)
        {
            _logger.LogInformation("AddDescription action post method executed");
            if (Desc == null)
            {
                _logger.LogError("request is null");
                return RedirectToAction("Index");//redirectToAction Error
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("validation for request failed");
                ProductResponse? Product = await _productService.GetProductById(Desc.ProductID);
                ViewBag.Product = Product;
                return View();
            }
            await _productDescService.AddProductDesc(Desc);
            return RedirectToAction("ProductDescriptions", new { productID = Desc.ProductID });
        }


        [Route("[action]/DescriptionId")]
        [HttpGet]
        public async Task<IActionResult> DeleteDescription(Guid? DescriptionId)
        {
            _logger.LogInformation("DeleteDescription action get method executed");
            if (DescriptionId == null || DescriptionId == Guid.Empty)
            {
                _logger.LogError("DescriptionId is nll");
                return RedirectToAction("index");
            }
            ProductDescResponse? desc = await _productDescService.GetProductDescByDescID(DescriptionId);
            if (desc == null)
            {
                _logger.LogError($"No desc was found for descId={DescriptionId}");
                return RedirectToAction("Index");
            }
            return View(desc);
        }

        [Route("[action]/DescriptionId")]
        [HttpPost]
        public IActionResult DeleteDescription(ProductDescResponse? Desc)
        {
            _logger.LogInformation("DeleteDescription action POST method executed");

            if (Desc == null || _productDescService.GetProductDescByDescID(Desc.Id) == null)
            {
                _logger.LogError("desc is null or not fond in database");
                return RedirectToAction("Index");
            }
            _productDescService.DeleteProductDesc(Desc.Id);
            _logger.LogDebug($"description with id={Desc.Id} was deleted");
            return RedirectToAction("ProductDescriptions", new { productID = Desc.ProductID });
        }

        [Route("[action]/DescriptionId")]
        [HttpGet]
        public async Task<IActionResult> EditDescription(Guid? DescId)
        {
            _logger.LogInformation("EditDescription action GET method executed");
            ProductDescResponse? desc = await _productDescService.GetProductDescByDescID(DescId);
            if (desc == null)
            {
                _logger.LogError("Id is null");
                return RedirectToAction("Index");
            }
            return View(desc.ToProductDescUpdateRequest());
        }

        [Route("[action]/DescriptionId")]
        [HttpPost]
        public async Task<IActionResult> EditDescription(ProductDescResponse Desc)
        {
            _logger.LogInformation("EditDescription action POST method executed");
            if (Desc == null)
            {
                _logger.LogError("Desc is null");
                return RedirectToAction("Index");
            }
            ProductDescResponse response = await _productDescService.GetProductDescByDescID(Desc.Id);
            if (response is null)
            {
                _logger.LogError($"No Desc for id={Desc.Id} was found");
                return RedirectToAction("index");
            }
            response.ProductID = Desc.ProductID;
            response.Description = Desc.Description;
            response.DescTitle = Desc.DescTitle;
            await _productDescService.UpdateProductDesc(Desc.ToProductDescUpdateRequest());
            _logger.LogDebug($"Desc with id={Desc.Id} was updated");
            return RedirectToAction("ProductDescriptions", new { productID = Desc.ProductID });
        }
        #endregion

        #region Images
        [Route("[action]/{ProductId}")]
        public async Task<IActionResult> ProductImages(Guid? ProductId)
        {
            _logger.LogInformation("ProductImages action method executed");
            if (ProductId == null)
            {
                _logger.LogError("id is null");
                return RedirectToAction("Index");
            }
            ProductResponse? product = await _productService.GetProductById(ProductId);
            if (product == null)
            {
                _logger.LogError($"No product was found for id={ProductId}");
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [Route("[action]/ImageId")]
        [HttpGet]
        public async Task<IActionResult> DeleteImage(Guid? ImageId)
        {
            _logger.LogInformation("DeleteImage action GET method executed");
            if (ImageId == null || ImageId == Guid.Empty)
            {
                _logger.LogError("imageID is null");
                return RedirectToAction("index");
            }
            ProductImageResponse? Image = await _productImageService.GetProductImageByImageID(ImageId);
            if (Image == null)
            {
                _logger.LogError($"No Image was found for id={ImageId}");
                return RedirectToAction("Index");
            }
            await _productImageService.DeleteProductImage(ImageId);
            _logger.LogDebug($"Image for id={ImageId} was deleted from DB and ready to be deleted from server");
            string fileName = Image.ImageUrl;
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
        public async Task<IActionResult> AddImage(ProductImageAddRequest? image, IFormFile? imageFile)
        {
            _logger.LogInformation("AddImage action POST method executed");
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

            return RedirectToAction("ProductImages", new { ProductId = productResponse.Id });
        }
        #endregion

        #region Properties
        [Route("[action]/{productID}")]
        [HttpGet]
        public async Task<IActionResult> ProductProperties(Guid productId)
        {
            _logger.LogInformation("ProductProperties action method executed");
            if (productId == Guid.Empty)
            {
                _logger.LogError("Id is null");
                return RedirectToAction("Index");
            }
            var productResponses = await _productService.GetProductById(productId);
            if (productResponses == null)
            {
                _logger.LogError($"No Product was found for Id={productId}");
                return RedirectToAction("Index");
            }
            return View(productResponses);
        }

        [Route("[action]/{productId}")]
        [HttpGet]
        public async Task<IActionResult> AddProperty(Guid? productId)
        {
            _logger.LogInformation("AddProprty action GET method executed");
            if (productId==null||productId == Guid.Empty)
            {
                _logger.LogError("id is null");
                return RedirectToAction("Index");
            }
            var product = await _productService.GetProductById(productId);
            if (product==null)
            {
                _logger.LogError($"No product was found for id={productId}");
                return RedirectToAction("index");
            }
            ViewBag.Product = product;
            return View();
        }

        [Route("[action]/{productId}")]
        [HttpPost]
        public async Task<IActionResult> AddProperty(ProductPropertyAddRequest request)
        {
            _logger.LogInformation("AddProprty action POST method executed");

            if (request == null)
            {
                _logger.LogError("request is null");
                return RedirectToAction("index");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("validation was failed");
                return View();
            }

            await _productPropertyService.AddProductProperty(request);
            _logger.LogDebug($"requset {request.ToString()} was added to DB");
            return RedirectToAction("ProductProperties", new { productId = request.ProductID });
        }

        [Route("[action]/{propertyId}")]
        [HttpGet]
        public async Task<IActionResult> EditProperty(Guid? propertyId)
        {
            _logger.LogInformation("EditProperty action GET method executed");
            if (propertyId == null)
            {
                _logger.LogError("propertyId is null");
                return RedirectToAction("index");
            }
            var request = await _productPropertyService.GetProductPropertyByPropertyID(propertyId);
            if (request==null)
            {
                _logger.LogError($"No property for id ={propertyId} was found");
                return RedirectToAction("index");
            }
            return View(request.ToProductPropertyUpdateRequest());
        }

        [Route("[action]/{propertyId}")]
        [HttpPost]
        public async Task<IActionResult> EditProperty(ProductPropertyUpdateRequest? request)
        {
            _logger.LogInformation("EditProperty action POST method executed");
            if (request == null)
            {
                _logger.LogError("request is null");
                return RedirectToAction("index");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Validation was failed");
                return View();
            }
            var property=await _productPropertyService.GetProductPropertyByPropertyID(request.Id);
            if (property==null)
            {
                _logger.LogError($"No property for id={request.Id} was found ");
                return RedirectToAction("index");
            }
            property.ProductID = request.ProductID;
            property.PropertyDetail = request.PropertyDetail;
            property.PropertyTitle = request.PropertyTitle;
            await _productPropertyService.UpdateProductProperty(request);
            _logger.LogDebug($"Property with id={request.Id} was updated");
            return RedirectToAction("ProductProperties", new { productId = request.ProductID });
        }

        [Route("[action]/{propertyId}")]
        [HttpGet]
        public async Task<IActionResult> DeleteProperty(Guid? propertyId)
        {
            _logger.LogInformation("DeleteProperty action GET method executed");
            if (propertyId == null)
            {
                _logger.LogError("id is null");
                return RedirectToAction("index");
            }
            ProductPropertyResponse property = await _productPropertyService.GetProductPropertyByPropertyID(propertyId);
            if (property == null)
            {
                _logger.LogError($"No property with id={propertyId} was found");
                return RedirectToAction("index");
            }
            return View(property);
        }

        [Route("[action]/{propertyId}")]
        [HttpPost]
        public async Task<IActionResult> DeleteProperty(ProductPropertyResponse property)
        {
            _logger.LogInformation("DeleteProperty action POST method executed");
            if (property == null)
            {
                _logger.LogError("property is null");
                return RedirectToAction("index");
            }
            var propertyResponse = await _productPropertyService.GetProductPropertyByPropertyID(property.Id);
            if (propertyResponse == null)
            {
                _logger.LogError($"No property with id={property.Id} was found");
                return RedirectToAction("index");
            }
            await _productPropertyService.DeleteProductProperty(propertyResponse.Id);
            _logger.LogDebug($"Property with id={property.Id} was deleted");
            return RedirectToAction("ProductProperties", new { productId = property.ProductID });
        }
        #endregion

    }
}
