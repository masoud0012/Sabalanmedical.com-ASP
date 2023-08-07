using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.ProductDescriptionDTO;
using ServiceContracts.DTO.ProductImageDTO;
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
        private readonly List<ProductTypeResponse>? _productTypes;
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
            _productTypes = _productTypeService.GettAllProductTypes();
            _environement = environment;
        }
        #region Products
        [Route("[action]")]
        public IActionResult Index()
        {
            ViewBag.products = _productService.GetAllProducts();
            ViewBag.Types = _productTypes;
            return View();
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddProduct()
        {
            ViewBag.TypeList = _productTypes?.Select(t => new SelectListItem() { Text = t.TypeNameEn, Value = t.TypeId.ToString() });
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult AddProduct(ProductAddRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Types = _productTypes;
                ViewBag.Errors = ModelState.Values.SelectMany(t => t.Errors).Select(t => t.ErrorMessage).ToList();
                return View();
            }
            //request.ProductUrl = request.ProductNameEn;
            _productService.AddProduct(request);
            return RedirectToAction("Index", "Products");
        }
        [Route("[action]/{ProductId}")]
        [HttpGet]
        public IActionResult EditProduct(Guid ProductId)
        {
            ProductResponse? response = _productService.GetProductById(ProductId);
            if (response is null)
            {
                return RedirectToAction("index", "Products");
            }
            ViewBag.TypeList = _productTypes?.Select(t => new SelectListItem() { Text = t.TypeNameEn, Value = t.TypeId.ToString() });
            return View(response.ToProductUpdateRequest());
        }
        [Route("[action]/{ProductId}")]
        [HttpPost]
        public IActionResult EditProduct(ProductUpdateRequest request)
        {
            if (request is null || _productService.GetProductById(request.ProductID) == null)
            {
                return RedirectToAction("index", "Products");
            }
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(request);
                return RedirectToAction("index", "Products");
            }
            else
            {
                ViewBag.Errors = ModelState.Values.SelectMany(t => t.Errors).Select(t => t.ErrorMessage).ToList();
                ViewBag.TypeList = _productTypeService.GettAllProductTypes()?.Select(t =>
                new SelectListItem() { Text = t.TypeNameEn, Value = t.TypeId.ToString() });
                return RedirectToAction("index", "Products");
            }
        }

        [Route("[action]/{ProductId}")]
        [HttpGet]
        public IActionResult DeleteProduct(Guid? ProductId)
        {
            if (ProductId == null)
            {
                return RedirectToAction("Index", "Products");
            }
            ProductResponse? response = _productService.GetProductById(ProductId);
            if (response == null)
            {
                return RedirectToAction("Index", "Products");
            }
            List<ProductImageResponse>? Images = _productImageService.GetProductImagesByProductID(ProductId);
            if (Images != null && Images.Count != 0)
            {
                ViewBag.Image = Images[0].ImageUrl;
            }
            return View(response);
        }

        [Route("[action]/{ProductId}")]
        [HttpPost]
        public IActionResult DeleteProduct(ProductResponse? product)
        {
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            ProductResponse? response = _productService.GetProductById(product.ProductId);
            if (response == null)
            {
                return RedirectToAction("Index", "Products");
            }

            _productService.DeleteProduct(product.ProductId);

            return RedirectToAction("Index", "Products");
        }

        #endregion

        #region Description
        [Route("[action]/{productID}")]
        [HttpGet]
        public IActionResult ProductDescriptions(Guid productID)
        {
            List<ProductDescResponse> descs = _productDescService.GetProductDescByProductID(productID);
            TotalDTO dto = new TotalDTO()
            {
                ProductResponses = _productService.GetProductById(productID),
                ProductDescResponses = descs
            };
            return View(dto);
        }


        [Route("[action]/{ProductId}")]
        [HttpGet]
        public IActionResult AddDescription(Guid ProductId)
        {
            ProductResponse? Product = _productService.GetProductById(ProductId);
            if (Product == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Product = Product;
            return View();
        }

        [Route("[action]/{ProductId}")]
        [HttpPost]
        public IActionResult AddDescription(ProductDescAddRequest Desc)
        {
            if (!ModelState.IsValid)
            {
                ProductResponse? Product = _productService.GetProductById(Desc.ProductID);
                ViewBag.Product = Product;
                return View();
            }
            _productDescService.AddProductDesc(Desc);
            return RedirectToAction("ProductDescriptions", new { productID=Desc.ProductID});
        }



        [Route("[action]/DescriptionId")]
        [HttpGet]
        public IActionResult DeleteDescription(Guid DescriptionId)
        {
            ProductDescResponse? desc=_productDescService.GetProductDescByDescID(DescriptionId);
            if (desc==null)
            {
                return RedirectToAction("Index");
            }
            return View(desc);
        }

        [Route("[action]/DescriptionId")]
        [HttpPost]
        public IActionResult DeleteDescription(ProductDescResponse Description)
        {
            if (Description == null || _productDescService.GetProductDescByDescID(Description.DesctiptionID)==null)
            {
                return RedirectToAction("Index");
            }
            _productDescService.DeleteProductDesc(Description.DesctiptionID);
            return RedirectToAction("ProductDescriptions", new { productID = Description.ProductID });
        }

        [Route("[action]/DescriptionId")]
        [HttpGet]
        public IActionResult EditDescription(Guid DescriptionId)
        {
            ProductDescResponse? desc = _productDescService.GetProductDescByDescID(DescriptionId);
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
            if (Desc==null)
            {
                return RedirectToAction("Index");
            }
            _productDescService.UpdateProductDesc(Desc.ToProductDescUpdateRequest());
            return RedirectToAction("ProductDescriptions", new {productID= Desc.ProductID});
        }
        #endregion

        #region Images
        [Route("[action]/{ProductId}")]
        public IActionResult ProductImages(Guid? ProductId)
        {
            if (ProductId==null || _productService.GetProductById(ProductId)==null)
            {
                return RedirectToAction("Index");
            }
            List<ProductImageResponse>? images=_productImageService.GetProductImagesByProductID(ProductId);
            TotalDTO dto = new TotalDTO()
            {
                ProductResponses = _productService.GetProductById(ProductId),
                ProductImageResponses = images
            };
            return View(dto);
        }

        [Route("[action]/ImageId")]
        [HttpGet]
        public IActionResult DeleteImage(Guid? ImageId)
        {
            ProductImageResponse? Image=_productImageService.GetProductImageByImageID(ImageId);
            if (Image==null)
            {
                return RedirectToAction("Index");
            }
            _productImageService.DeleteProductImage(ImageId);
            string? fileName = Image.ImageUrl;
            if(fileName==null)
            {
                return RedirectToAction("Index"); 
            }
            string path = Path.Combine(_environement.WebRootPath, $"images/products/{fileName}");
            FileInfo file= new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
            return RedirectToAction("ProductImages", new { ProductId = Image.ProductID });
        }

        [Route("[action]/ProductId")]
        [HttpPost]
        public async Task<IActionResult> AddImage(ProductImageAddRequest Image,IFormFile imageFile)
        {
            ProductResponse? productResponse = _productService.GetProductById(Image.ProductID);
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
                string fileName=productResponse.ProductUrl+"-"+random.Next()+ "."+extension;
                string path = Path.Combine(_environement.WebRootPath,$"images/products/{fileName}");
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
            return RedirectToAction("ProductImages", new {ProductId=productResponse.ProductId});
        }
        #endregion
    }
}
