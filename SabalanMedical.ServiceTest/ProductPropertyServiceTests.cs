using AutoFixture;
using Entities;
using FluentAssertions;
using IRepository;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO.ProductPropertyDTO;
using ServiceContracts.DTO.ProductTypeDTO;
using Services;
using Xunit.Abstractions;

namespace SabalanMedical.ServiceTest
{

    public class ProductPropertyServiceTests
    {
        private readonly IFixture _fixture;
        private readonly IProductPropertyService _productPropertyService;
        private readonly Mock<IProductPropertyRepository> _PropertyRepositoryMock;
        private readonly ITestOutputHelper _outputHelperMock;
        public ProductPropertyServiceTests(ITestOutputHelper testOutputHelper)
        {
            _outputHelperMock = testOutputHelper;
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _PropertyRepositoryMock = new Mock<IProductPropertyRepository>();
            Mock<IUnitOfWork> unitOfWork = new Mock<IUnitOfWork>();
            _productPropertyService = new ProductPropertyService(_PropertyRepositoryMock.Object, unitOfWork.Object);
        }
        #region AddProperty
        [Fact]
        public async Task AddProductProperty_RequestIsNull_ThroeNullException()
        {
            //Arrangment
            ProductPropertyAddRequest? request = null;
            //Act
            Func<Task> action = async () =>
            {
                await _productPropertyService.AddProductProperty(request);
            };
            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public async Task AddProductProperty_PropertyIsNull_ThrowArgumentExc()
        {
            //Arrangement
            ProductPropertyAddRequest request = _fixture.Build<ProductPropertyAddRequest>()
                .With(t => t.PropertyDetail, null as string).Create();
            ProductProperty productProperty = request.ToProductProperty();
            //Act
            _PropertyRepositoryMock.Setup(t => t.Add(It.IsAny<ProductProperty>())).ReturnsAsync(productProperty);
            Func<Task> action = async () => { await _productPropertyService.AddProductProperty(request); };
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddProductProperty_ProperProperty_shouldReturnPropertyResponse()
        {
            //Arrangement
            ProductPropertyAddRequest request = _fixture.Create<ProductPropertyAddRequest>();
            ProductProperty property = request.ToProductProperty();
            var expectedProperty = property.ToProductPropertyResponse();
            //Act
            _PropertyRepositoryMock.Setup(t => t.Add(It.IsAny<ProductProperty>())).ReturnsAsync(property);
            var propertyResponse = await _productPropertyService.AddProductProperty(request);
            propertyResponse.Id = expectedProperty.Id;
            _outputHelperMock.WriteLine("Expected:");
            _outputHelperMock.WriteLine(expectedProperty.ToString());
            _outputHelperMock.WriteLine("Actual:");
            _outputHelperMock.WriteLine(propertyResponse.ToString());

            //Assert
            propertyResponse.Should().Be(expectedProperty);
        }
        #endregion
        #region DeleteProperty
        [Fact]
        public async Task DeleteProductProperty_IdIsNull_ThrowNullException()
        {
            //Arrangement
            Guid id = Guid.Empty;
            //Act
            Func<Task> action = async () =>
            {
                await _productPropertyService.DeleteProductProperty(id);
            };
            //Assert
            await action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
        [Fact]
        public async Task DeleteProductProperty_IdNotFound_ThrowArgException()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            //Act
            _PropertyRepositoryMock.Setup(t => t.GetById(It.IsAny<Guid>())).ReturnsAsync(null as ProductProperty);
            bool action = await _productPropertyService.DeleteProductProperty(id);
            //Assert

            action.Should().BeFalse();
        }
        [Fact]
        public async Task DeleteProductProperty_ProperId_ShouldReturnTrue()
        {
            //Arrange
            ProductProperty productProperty = _fixture.Create<ProductProperty>();
            //Act
            _PropertyRepositoryMock.Setup(t => t.GetById(It.IsAny<Guid>())).ReturnsAsync(productProperty);
            bool response = await _productPropertyService.DeleteProductProperty(productProperty.Id);
            //Assert
            response.Should().BeTrue();
        }
        #endregion
        #region AllProperties
        [Fact]
        public async Task GetAllProductProperty_EmptyList_SholdReturnEmpty()
        {
            //Arrangement
            List<ProductProperty> productProperties = new List<ProductProperty>();
            //Act
            _PropertyRepositoryMock.Setup(t => t.GetAllAsync(0, 100)).ReturnsAsync(productProperties.AsQueryable());
            List<ProductPropertyResponse> response = await _productPropertyService.GetAllProductProperty();
            //Asset
            response.Should().BeEmpty();
        }
        [Fact]
        public async Task GetAllProductProperty_SomePropertyObjects()
        {
            //Arrangement
            List<ProductProperty> productProperties = new List<ProductProperty>()
            {
                _fixture.Create<ProductProperty>(),
                _fixture.Create<ProductProperty>(),
                _fixture.Create<ProductProperty>(),
            };
            List<ProductPropertyResponse> expected = productProperties.Select(t => t.ToProductPropertyResponse()).ToList();
            //Act
            _PropertyRepositoryMock.Setup(t => t.GetAllAsync(0, 50)).ReturnsAsync(productProperties.AsQueryable());
            List<ProductPropertyResponse> response = await _productPropertyService.GetAllProductProperty();
            //Asset
            response.Should().BeEquivalentTo(expected);
        }
        #endregion
        #region GetProductPropertiesByProductID
        [Fact]
        public async Task GetProductPropertiesByProductID_IdIsNull()
        {
            //Arrangement
            Guid Id = Guid.Empty;

            //Act
            Func<Task> action = async () => { await _productPropertyService.GetProductPropertiesByProductID(Id); };
            //Assert
            action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public async Task GetProductPropertiesByProductID_ProperId_ShoudReturnListOfProperties()
        {
            //Arrangement
            List<ProductProperty> properties = _fixture.Create<List<ProductProperty>>();
            List<ProductPropertyResponse> propertyResponses = properties.Select(t => t.ToProductPropertyResponse()).ToList();
            _PropertyRepositoryMock.Setup(t => t.GetByProductID(It.IsAny<Guid>())).ReturnsAsync(properties.AsQueryable());
            //Act
            List<ProductPropertyResponse>? actual = await _productPropertyService.GetProductPropertiesByProductID(Guid.NewGuid());
            //Assert
            actual.Should().BeEquivalentTo(propertyResponses);
        }
        #endregion
        #region GetProductPropertyByPropertyID
        [Fact]
        public async Task GetProductPropertyByPropertyID_IdisNull_throwExc()
        {
            //Arrangement
            Guid Id = Guid.Empty;
            //Act
            Func<Task> action = async () => await _productPropertyService.GetProductPropertyByPropertyID(Id);
            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public async Task GetProductPropertyByPropertyID_IdNotFound_throwExc()
        {
            //Arrangement
            Guid Id = new Guid();
            _PropertyRepositoryMock.Setup(t => t.GetById(It.IsAny<Guid>())).ReturnsAsync(null as ProductProperty);
            //Act
            Func<Task> action = async () => { await _productPropertyService.GetProductPropertyByPropertyID(Id); };
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetProductPropertyByPropertyID_propperID()
        {
            //Arrangement
            ProductProperty property = _fixture.Create<ProductProperty>();
            ProductPropertyResponse expected = property.ToProductPropertyResponse();
            _PropertyRepositoryMock.Setup(t => t.GetById(It.IsAny<Guid>())).ReturnsAsync(property);
            //Act
            ProductPropertyResponse actual= await _productPropertyService.GetProductPropertyByPropertyID(property.Id);
            actual.Id = property.Id;
            //Assert
            actual.Should().Be(expected);
        }
        #endregion
        #region UpdateProductProperty
        
        #endregion
    }
}
