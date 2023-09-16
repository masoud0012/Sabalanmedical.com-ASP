using ServiceContracts;
using Services;
using Entities;
using ServiceContracts.DTO.ProductTypeDTO;
using Microsoft.EntityFrameworkCore;
using AutoFixture;
using Moq;
using RepositoryServices;
using FluentAssertions;
using IRepository;
using IRepository;

namespace TestProject
{
    public class ProductTypeServiceTest
    {
        private readonly IProductTypeService _productTypeSerivice;
        private readonly IFixture _fixture;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly Mock<IProductTypeRepository> _repositoryMoq;
        
        public ProductTypeServiceTest()
        {
            _repositoryMoq = new Mock<IProductTypeRepository>();
            _productTypeRepository = _repositoryMoq.Object;
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            Mock<IUnitOfWork> unit=new Mock<IUnitOfWork>(); 
            _productTypeSerivice = new ProductTypesService(_productTypeRepository, unit.Object);

        }
        #region Add ProductType Tests
        //ProductType is null.Throw ArgumentNullException
        [Fact]
        public async Task AddProductType_ProductTypeIsNull()
        {
            //Arrange
            ProductTypeAddRequest? request = null;
            //Act
            Func<Task> action = async () =>
            {
                await _productTypeSerivice.AddProductType(request);

            };
            //Assertion

            action.Should().ThrowAsync<ArgumentNullException>();
        }
        //ProductType Name is null, Throw ArumentException
        [Fact]
        public async Task AddProdutType_Name_IsNull()
        {
            //Arrange
            ProductTypeAddRequest request = new ProductTypeAddRequest()
            {
                TypeNameEN = null,
                TypeNameFr = null
            };
            ProductType type = request.ToProductType();
            //Act
            _repositoryMoq.Setup(t => t.Add(It.IsAny<ProductType>())).ReturnsAsync(type);
            Func<Task> action = async () =>
            {
                await _productTypeSerivice.AddProductType(request);
            };
            //Assert

            action.Should().ThrowAsync<ArgumentException>();
        }
        //ProducyType Name Duplicate, Throw ArgumentException
        [Fact]
        public async void AddProductType_TypeIsDuplicated()
        {
            //Arrangment
            ProductTypeAddRequest request1 = _fixture.Build<ProductTypeAddRequest>().With(t => t.TypeNameEN, "test").Create();
            ProductTypeAddRequest request2 = _fixture.Build<ProductTypeAddRequest>().With(t => t.TypeNameEN, "test").Create();
            ProductType productType1 = request1.ToProductType();
            ProductType productType2 = request2.ToProductType();

            _repositoryMoq.Setup(t => t.Add(It.IsAny<ProductType>())).ReturnsAsync(productType1);
            _repositoryMoq.Setup(t => t.GetProductTypeByName(It.IsAny<String>())).ReturnsAsync(null as ProductType);

            ProductTypeResponse productTypeResponse = await _productTypeSerivice.AddProductType(request1);

            //act
            Func<Task> action = async () =>
            {
                _repositoryMoq.Setup(t => t.Add(It.IsAny<ProductType>())).ReturnsAsync(productType1);
                _repositoryMoq.Setup(t => t.GetProductTypeByName(It.IsAny<string>())).ReturnsAsync(productType1);
                await _productTypeSerivice.AddProductType(request2);
            };
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }
        //Proper ProductType,add the product type
        [Fact]
        public async Task AddProductType_ProperObject()
        {
            //Arrangment
            ProductTypeAddRequest request = _fixture.Create<ProductTypeAddRequest>();
            ProductType productType = request.ToProductType();
            ProductTypeResponse expexted_type = productType.ToProductTypeResponse();
            //Act
            _repositoryMoq.Setup(t => t.Add(It.IsAny<ProductType>())).ReturnsAsync(productType);
            ProductTypeResponse response = await _productTypeSerivice.AddProductType(request);
            expexted_type.TypeId = response.TypeId;
            //Assert
            response.TypeId.Should().NotBe(Guid.Empty);
            response.Should().Be(expexted_type);
        }
        #endregion
        #region Get All Product Types tests
        [Fact]
        public async Task GetAllProductTypes_EmptyList()
        {
            //arrangement
            List<ProductType> productTypes = new List<ProductType>();
            _repositoryMoq.Setup(t => t.GetAllAsync(0,100)).ReturnsAsync(productTypes.AsQueryable());
            //act
            List<ProductTypeResponse>? productTypeList = await _productTypeSerivice.GetAllProductTypes();
            //assert
            productTypeList.Should().BeNullOrEmpty();
        }
        [Fact]
        public async Task GetAllProductTypes_PropperList()
        {
            //arrangement
            List<ProductType> productTypes = new List<ProductType>()
            {
                  _fixture.Build<ProductType>().With(t=>t.Products,null as List<Product>).Create(),
                  _fixture.Build<ProductType>().With(t=>t.Products,null as List<Product>).Create(),
                  _fixture.Build<ProductType>().With(t=>t.Products,null as List<Product>).Create()

            };
            List<ProductTypeResponse> productTypeResponses = productTypes.Select(t => t.ToProductTypeResponse()).ToList();
            _repositoryMoq.Setup(t => t.GetAllAsync(0,50)).ReturnsAsync(productTypes.AsQueryable());
            //act
            List<ProductTypeResponse>? productTypeList_fromGetMethod = await _productTypeSerivice.GetAllProductTypes();
            //assert
            productTypeList_fromGetMethod.Should().BeEquivalentTo(productTypeResponses);
        }
        #endregion
        #region GetProductTypeByID tests
        [Fact]
        public async Task GetProductTypeByID_IDIsNull()
        {
            //Arrangment
            Guid? ProductTypId = null;
            _repositoryMoq.Setup(t => t.GetById(It.IsAny<Guid>())).ReturnsAsync(null as ProductType);
            //Act
            Func<Task> action = async () => await _productTypeSerivice.GetProductTypeByID(ProductTypId);
            //Assert
            action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public async void GetProductTypeByID_ValidID()
        {
            //arrange
            ProductType productType = _fixture.Build<ProductType>().With(t => t.Products, null as List<Product>).Create();
            ProductTypeResponse productTypeResponse = productType.ToProductTypeResponse();
            _repositoryMoq.Setup(t => t.GetById(It.IsAny<Guid>())).ReturnsAsync(productType);
            //act
            ProductTypeResponse? response = await _productTypeSerivice.GetProductTypeByID(productType.Id);
            //assert
            response.Should().Be(productTypeResponse);
        }
        #endregion
    }
}