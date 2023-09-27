using AutoFixture;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO.ProductsDTO;
using SabalanMedical.Controllers;
using ServiceContracts.DTO.ProductTypeDTO;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace TestProject
{
    public class ProductsControllerTests
    {
        private readonly IFixture _fixture;

        private readonly IProductService _productService;
        private readonly IProductTypeService _productTypeService;
        private readonly IProductImageService _productImageService;
        private readonly IProductDescService _productDescService;
        private readonly IProductPropertyService _productPropertyService;

        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<IProductTypeService> _productTypeServiceMock;
        private readonly Mock<IProductImageService> _productImageServiceMock;
        private readonly Mock<IProductDescService> _productDescServiceMock;
        private readonly Mock<IProductPropertyService> _productPropertyServiceMock;
        public ProductsControllerTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _productServiceMock = new Mock<IProductService>();
            _productTypeServiceMock = new Mock<IProductTypeService>();
            _productImageServiceMock = new Mock<IProductImageService>();
            _productDescServiceMock = new Mock<IProductDescService>();
            _productPropertyServiceMock = new Mock<IProductPropertyService>();

            _productService = _productServiceMock.Object;
            _productTypeService = _productTypeServiceMock.Object;
            _productImageService = _productImageServiceMock.Object;
            _productDescService = _productDescServiceMock.Object;
            _productPropertyService = _productPropertyServiceMock.Object;
        }
        [Fact]
        public async Task Index_ShouldRetunIndexViewWithProductList()
        {
            //Arrange
            List<ProductResponse> productResponses = _fixture.Create<List<ProductResponse>>();
            List<ProductTypeResponse> productTypeResponses = _fixture.Create<List<ProductTypeResponse>>();
            ProductsController productsController = new ProductsController(_productService, _productTypeService,
                _productImageService, _productDescService, _productPropertyService, null);
            _productServiceMock.Setup(t => t.GetAllProducts()).ReturnsAsync(productResponses);
            _productTypeServiceMock.Setup(t => t.GetAllProductTypes()).ReturnsAsync(productTypeResponses);

            //Act
            IActionResult result = await productsController.Index();
            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<List<ProductResponse>>();
            viewResult.ViewData.Model.Should().Be(productResponses);
        }
        [Fact]
        public async Task AddProduct_RequestIsInValid_shouldReturnView()
        {
            //Arrange
            List<ProductTypeResponse> productTypeResponses = _fixture.Create<List<ProductTypeResponse>>();
            ProductsController productsController = new ProductsController(_productService, _productTypeService,
             _productImageService, _productDescService, _productPropertyService, null);

            _productTypeServiceMock.Setup(t => t.GetAllProductTypes()).ReturnsAsync(productTypeResponses);
            ProductResponse productResponse = _fixture.Create<ProductResponse>();
            _productServiceMock.Setup(t => t.AddProduct(It.IsAny<ProductAddRequest>())).ReturnsAsync(productResponse);

            //Act
            productsController.ModelState.AddModelError("ProdctNameEN", "Product Name can not be null");
            IActionResult result = await productsController.AddProduct(_fixture.Create<ProductAddRequest>());
            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async Task AddProduct_RequestIsValid_shouldReturnRedirectAction()
        {
            //Arrange
            List<ProductTypeResponse> productTypeResponses = _fixture.Create<List<ProductTypeResponse>>();
            ProductsController productsController = new ProductsController(_productService, _productTypeService,
                _productImageService, _productDescService, _productPropertyService, null);
            _productTypeServiceMock.Setup(t => t.GetAllProductTypes()).ReturnsAsync(productTypeResponses);
            ProductResponse productResponse = _fixture.Create<ProductResponse>();
            _productServiceMock.Setup(t => t.AddProduct(It.IsAny<ProductAddRequest>())).ReturnsAsync(productResponse);
            //Act
            IActionResult result = await productsController.AddProduct(_fixture.Create<ProductAddRequest>());
            //Assert
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task EditProduct_nullRequest_RedirectToAction()
        {
            //Arrange
            List<ProductTypeResponse> productTypeResponses = _fixture.Create<List<ProductTypeResponse>>();
            ProductsController productsController = new ProductsController(_productService, _productTypeService,
                _productImageService, _productDescService, _productPropertyService, null);
            _productServiceMock.Setup(t => t.GetProductById(It.IsAny<Guid>())).ReturnsAsync(null as ProductResponse);
            _productTypeServiceMock.Setup(t => t.GetAllProductTypes()).ReturnsAsync(productTypeResponses);

            //Act
            IActionResult result = await productsController.EditProduct(Guid.NewGuid());

            //Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        public async Task EditProduct_ProperAddRequest_RedirectToAction()
        {
            //Arrange
            ProductResponse productResponses = _fixture.Create<ProductResponse>();
            List<ProductTypeResponse> productTypeResponses = _fixture.Create<List<ProductTypeResponse>>();
            ProductsController productsController = new ProductsController(_productService, _productTypeService,
                _productImageService, _productDescService, _productPropertyService, null);
            _productServiceMock.Setup(t => t.GetProductById(It.IsAny<Guid>())).ReturnsAsync(productResponses);
            _productTypeServiceMock.Setup(t => t.GetAllProductTypes()).ReturnsAsync(productTypeResponses);

            //Act
            IActionResult result = await productsController.EditProduct(Guid.NewGuid());

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<ProductUpdateRequest>();
        }
    }

}


