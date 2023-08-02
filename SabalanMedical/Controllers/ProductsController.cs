using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.ProductDescriptionDTO;
using ServiceContracts.DTO.ProductImageDTO;
using ServiceContracts.DTO.ProductsDTO;
using ServiceContracts.DTO.ProductTypeDTO;

namespace SabalanMedical.Controllers
{
    [Route("[Controller]")]
    public class ProductsController : Controller
    {
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
            IProductPropertyService productPropertyService)
        {
            _productService = productService;
            _productTypeService = productTypeService;
            _productImageService = productImageService;
            _productDescService = productDescService;
            _productPropertyService = productPropertyService;
            _productTypes = _productTypeService.GettAllProductTypes();
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
            return View(desc);
        }

        [Route("[action]/DescriptionId")]
        [HttpGet]
        public IActionResult EditDescription(ProductDescResponse Desc)
        {
            if (Desc==null)
            {
                return RedirectToAction("Index");
            }
            _productDescService.UpdateProductDesc(Desc.ToProductDescUpdateRequest());
            return RedirectToAction("ProductDescriptions", new {ProductId=Desc.ProductID});
        }

        #endregion
    }
}
