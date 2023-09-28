using AutoFixture;
using Castle.Core.Logging;
using Entities;
using FluentAssertions;
using IRepository;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO.ProductsDTO;
using ServiceContracts.Enums;
using Services;
using System.Linq.Expressions;
using Xunit.Abstractions;

namespace TestProject
{
    public class ProductServiceTests
    {
        private readonly IProductService _productService;
        private readonly ITestOutputHelper _testHelper;
        private readonly IProductTypeService _productTypeService;
        private readonly Mock<IProductTypeRepository> _produtTypeRepositoryMoq;
        private readonly Mock<IProductRepository> _productRepositoryMoq;
        private readonly IFixture _fixture;

        public ProductServiceTests(ITestOutputHelper testOutputHelper)
        {

            _testHelper = testOutputHelper;

            _productRepositoryMoq = new Mock<IProductRepository>();
            _produtTypeRepositoryMoq = new Mock<IProductTypeRepository>();

            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            Mock<IUnitOfWork> unit = new Mock<IUnitOfWork>();
            _productService = new ProductService(_productRepositoryMoq.Object, unit.Object, new Mock<ILogger<ProductService>>().Object);
            _productTypeService = new ProductTypesService(_produtTypeRepositoryMoq.Object, unit.Object, new Mock<ILogger<ProductTypesService>>().Object);
        }
        #region AddProduct
        [Fact]
        public void AddProduct_ProductIsNull()
        {
            //Arrange
            ProductAddRequest? request = null;
            //act
            Func<Task> action = async () =>
            {
                await _productService.AddProduct(request);
            };
            //assert
            action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public async void AddProduct_ProductNameIsNull()
        {
            //Arrange
            ProductAddRequest request = _fixture.Build<ProductAddRequest>().With(t => t.ProductNameEn, null as string)
                .With(t => t.ProductNameFr, null as string).Create();
            //act
            _productRepositoryMoq.Setup(t => t.Add(It.IsAny<Product>())).ReturnsAsync(request.ToProduct());

            Func<Task> action = async () =>
            {
                await _productService.AddProduct(request);
            };
            //assert
            action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void AddProduct_ProperProduct()
        {
            //Arrange
            ProductAddRequest request = _fixture.Create<ProductAddRequest>();
            Product product = request.ToProduct();

            //act
            _productRepositoryMoq.Setup(t => t.Add(It.IsAny<Product>())).ReturnsAsync(product);
            ProductResponse productResponse = await _productService.AddProduct(request);
            product.Id = productResponse.Id;

            //assert
            productResponse.Id.Should().NotBe(Guid.Empty);
            productResponse.Should().Be(product.ToProductResponse());
        }
        #endregion

        #region GetProductID
        [Fact]
        public async void GetProductById_NullID()
        {
            //Arrange
            Guid? ProductId = null;
            //act
            ProductResponse? response = await _productService.GetProductById(ProductId);
            //asser
            response.Should().Be(null);
        }
        [Fact]
        public async void GetPProductByID_properID()
        {
            //arrangment
            ProductAddRequest request = _fixture.Create<ProductAddRequest>();
            Product product = request.ToProduct();
            _productRepositoryMoq.Setup(t => t.GetById(It.IsAny<Guid>())).ReturnsAsync(product);
            //act
            ProductResponse? response = await _productService.GetProductById(product.Id);
            //assert
            response.Should().Be(product.ToProductResponse());
        }
        #endregion

        #region GetAllProducts
        [Fact]
        public async void GetAllProducts_EmptyList()
        {
            //Arrange
            List<Product> products = new List<Product>();
            _productRepositoryMoq.Setup(t => t.GetAllAsync(0, 50)).ReturnsAsync(products.AsQueryable());

            //Act
            List<ProductResponse> responses = await _productService.GetAllProducts();

            //Assert
            responses.Should().BeEmpty();
        }
        [Fact]
        public async Task GetAllProducts_SomeProdcuts()
        {
            //Arrange
            List<Product> products = new List<Product>()
            {
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
                _fixture.Create<Product>()
            };
            List<ProductResponse> productResponses = products.Select(t => t.ToProductResponse()).ToList();
            _productRepositoryMoq.Setup(t => t.GetAllAsync(0, 50)).ReturnsAsync(products.AsQueryable());

            //Act
            List<ProductResponse> responses_fromGelAll = await _productService.GetAllProducts();
            _testHelper.WriteLine("all products frpm GetAllProducts Method");
            foreach (ProductResponse item in responses_fromGelAll)
            {
                _testHelper.WriteLine(item.ToString() + "\n");
            }
            //Assert
            responses_fromGelAll.Should().BeEquivalentTo(productResponses);
        }
        #endregion

        #region GetFilteredProducts
        [Fact]
        public async Task GetFilteredProducts_SearchKeyIsEmpty()
        {
            //Arrange
            List<Product> products = new List<Product>()
            {
             _fixture.Create<Product>(),
             _fixture.Create<Product>(),
             _fixture.Create<Product>(),
            };
            List<ProductResponse> productResponses = products.Select(t => t.ToProductResponse()).ToList();
            _productRepositoryMoq.Setup(t => t.GetFilteredProduct(It.IsAny<Expression<Func<Product, bool>>>(), 0, 50))
                .ReturnsAsync(products);
            _testHelper.WriteLine("Expected products ");
            foreach (Product item in products)
            {
                _testHelper.WriteLine(item.ToString());
            }

            //Act
            List<ProductResponse>? responses_fromFilteredProducts =
                await _productService.GetFilteredProduct(nameof(ProductResponse.ProductNameEn), "");

            _testHelper.WriteLine("all products frpm GetAllProducts Method");
            if (responses_fromFilteredProducts is not null)
            {
                foreach (ProductResponse item in responses_fromFilteredProducts)
                {
                    _testHelper.WriteLine(item.ToString() + "\n");
                }
            }
            //assert
            Assert.Equivalent(productResponses, responses_fromFilteredProducts);
        }

        [Fact]
        public async Task GetFilteredProducts_SearchByProductName()
        {
            //Arrange
            List<Product> products = new List<Product>()
            {
               _fixture.Build<Product>().With(t=>t.ProductNameEn,"Reload Stapler1").Create(),
               _fixture.Build<Product>().With(t=>t.ProductNameEn,"Reload Stapler2").Create(),
               _fixture.Build<Product>().With(t=>t.ProductNameEn,"Reload Stapler3").Create(),
               _fixture.Create<Product>(),
               _fixture.Create<Product>()
            };

            List<ProductResponse> productResponses = products.Select(t => t.ToProductResponse()).ToList();

            _testHelper.WriteLine("Expected Products");
            foreach (var item in productResponses)
            {
                _testHelper.WriteLine(item.ToString() + ",\n");
            }
            _productRepositoryMoq.Setup(t => t.GetFilteredProduct(It.IsAny<Expression<Func<Product, bool>>>(), 0, 50))
                .ReturnsAsync(products);
            //Act
            List<ProductResponse>? responses_fromFilteredProducts =
               await _productService.GetFilteredProduct(nameof(ProductResponse.ProductNameEn), "Re");

            _testHelper.WriteLine("Actual products from filter method");
            if (responses_fromFilteredProducts is not null)
            {
                foreach (ProductResponse item in responses_fromFilteredProducts)
                {
                    _testHelper.WriteLine(item.ToString() + ",\n");
                }
            }
            //assert
            responses_fromFilteredProducts.Should().BeEquivalentTo(responses_fromFilteredProducts);
        }
        #endregion

        #region GetSortedProducts
        //sort desc products based on productNameEN
        [Fact]
        public async Task GetSortedProducts()
        {
            //Arrange
            List<Product> products = new List<Product>()
            {
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
                _fixture.Create<Product>(),
            };
            List<ProductResponse> productResponses = products.Select(t => t.ToProductResponse()).ToList();
            _testHelper.WriteLine("Expected Products");
            foreach (ProductResponse item in productResponses)
            {
                _testHelper.WriteLine(item.ToString());
            }

            //Act
            List<ProductResponse>? responses_fromSorted =
             await _productService.GetSortedProducts(productResponses, nameof(ProductResponse.ProductNameEn), SortOrderOptions.Asc);
            _testHelper.WriteLine("all products from Sorted Method");
            if (responses_fromSorted is not null)
            {
                foreach (ProductResponse item in responses_fromSorted)
                {
                    _testHelper.WriteLine(item.ToString());
                }
            }
            //Assert
            responses_fromSorted.Should().BeInAscendingOrder(t => t.ProductNameEn);
        }
        #endregion

        #region UpdateProduct
        [Fact]
        public async Task UpdateProduct_RequestIsNull()
        {
            //Arrangement
            ProductUpdateRequest? request = null;
            //Act
            Func<Task> action = async () =>
             {
                 await _productService.UpdateProduct(request);
             };
            //Assert
            action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public async Task ProductUpdate_InvalidProductID()
        {
            //Arrange
            Product product = _fixture.Build<Product>().With(t => t.Id, Guid.Empty).Create();
            _productRepositoryMoq.Setup(t => t.Update(It.IsAny<Product>())).ReturnsAsync(product);
            ProductResponse productResponse = product.ToProductResponse();

            //Act

            Func<Task> action = async () =>
             {
                 await _productService.UpdateProduct(productResponse.ToProductUpdateRequest());
             };
            //Assert
            action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task ProductUpdate_ProductNameIsNull()
        {
            //Arrange
            Product product = _fixture.Build<Product>().With(t => t.ProductNameEn, null as string).Create();
            ProductResponse productResponse = product.ToProductResponse();
            _productRepositoryMoq.Setup(t => t.Update(It.IsAny<Product>())).ReturnsAsync(product);
            //Act
            Func<Task> action = async () =>
            {
                await _productService.UpdateProduct(productResponse.ToProductUpdateRequest());
            };
            //Assert
            action.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public async Task UpdateProduct_RequestIsProper()
        {
            //Arrange
            Product product = _fixture.Create<Product>();
            ProductResponse productResponse = product.ToProductResponse();
            _productRepositoryMoq.Setup(t => t.Update(It.IsAny<Product>())).ReturnsAsync(product);
            _productRepositoryMoq.Setup(t => t.GetById(It.IsAny<Guid>())).ReturnsAsync(product);
            //Act

            ProductResponse productResponse_from_update_methos = await _productService.UpdateProduct(productResponse.ToProductUpdateRequest());

            //assert
            productResponse_from_update_methos.Should().Be(productResponse);

        }

        #endregion

        #region DeletProduct
        [Fact]
        public async Task DeleteProduct_ProductIdIsNull()
        {
            //Arrangemet
            Guid? productID = null;
            //Act
            var action = async () =>
            {
                await _productService.DeleteProduct(productID);
            };
            //Assest
            action.Should().ThrowAsync<ArgumentException>();


        }
        [Fact]
        public async Task DeleteProduct_InvalidProductId()
        {
            //Arrange
            Guid productId = Guid.NewGuid();
            _productRepositoryMoq.Setup(t => t.GetById(It.IsAny<Guid>())).ReturnsAsync(null as Product);
            //Act
            bool isDeleted = await _productService.DeleteProduct(productId);
            //assert
            isDeleted.Should().BeFalse();
        }
        [Fact]
        public async Task DeleteProduct_ProperProductId()
        {
            //Arrange
            Product product = _fixture.Create<Product>();
            ProductResponse productResponse = product.ToProductResponse();
            _productRepositoryMoq.Setup(t => t.GetById(It.IsAny<Guid>())).ReturnsAsync(product);
            _productRepositoryMoq.Setup(t => t.Delete(It.IsAny<Product>())).ReturnsAsync(true);
            //Arrange
            bool result = await _productService.DeleteProduct(productResponse.Id);
            //assert
            result.Should().BeTrue();
        }
        #endregion
    }
}
