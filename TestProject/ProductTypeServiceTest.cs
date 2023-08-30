using ServiceContracts;
using Services;
using Entities;
using ServiceContracts.DTO.ProductTypeDTO;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;

namespace TestProject
{
    public class ProductTypeServiceTest
    {
        private readonly IProductTypeService _productTypeSerivice;
        public ProductTypeServiceTest()
        {
         
            List<ProductType> productTypes = new List<ProductType>() { };
            DbContextMock<SabalanDbContext> dbContextMock=new DbContextMock<SabalanDbContext> (new DbContextOptionsBuilder<SabalanDbContext>().Options);
            SabalanDbContext sabalanDbContext=dbContextMock.Object;
            dbContextMock.CreateDbSetMock(t=>t.ProductTypes,productTypes);
            _productTypeSerivice = new ProductTypesService(sabalanDbContext);
        }
        #region Add ProductType
        //ProductType is null.Throw ArgumentNullException
        [Fact]
        public async Task AddProductType_IsNull()
        {
            //Arrange
            ProductTypeAddRequest? request = null;
            //Act

            //Assertion
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
               await _productTypeSerivice.AddProductType(request);
            });
        }
        //ProductType Name is null, Throw ArumentException
        [Fact]
        public async Task AddProdutType_NameIsNull()
        {
            //Arrange
            ProductTypeAddRequest request = new ProductTypeAddRequest()
            {
                TypeNameEN = null,
                TypeNameFr = null
            };
            //Assert
           await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
              await  _productTypeSerivice.AddProductType(request);
            });
        }
        //ProducyType Name Duplicate, Throw ArgumentException
        [Fact]
        public void AddProductType_TypeIsDuplicated()
        {
            //Arrangment
            ProductTypeAddRequest request1 = new ProductTypeAddRequest()
            {
                TypeNameEN = "ENT",
                TypeNameFr = "بیهوشی تنفسی"
            };
            ProductTypeAddRequest request2 = new ProductTypeAddRequest()
            {
                TypeNameEN = "ENT",
                TypeNameFr = "بیهوشی تنفسی"
            };
            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
               await _productTypeSerivice.AddProductType(request1);
               await _productTypeSerivice.AddProductType(request2);
            });
        }
        //Proper ProductType,add the product type
        [Fact]
        public async Task AddProductType_ProperObject()
        {
            //Arrangment
            ProductTypeAddRequest request = new ProductTypeAddRequest()
            {
                TypeNameEN = "ENT",
                TypeNameFr = "محصولات بیهوشی تنفسی"
            };
            //Act
            ProductTypeResponse response =await _productTypeSerivice.AddProductType(request);

            //Assert
            Assert.Equal(response.TypeNameEn, request.TypeNameEN);
            Assert.True(response.TypeId != Guid.Empty);
        }
        #endregion
        #region Get All Product Types
        [Fact]
        public async Task GetAllProductTypes_EmptyList()
        {
            //arrangement
            //act
            var productTypeList =await _productTypeSerivice.GetAllProductTypes();
            //assert
            Assert.Empty(productTypeList);
        }
        [Fact]
        public async Task GetAllProductTypes_PropperList()
        {
            //arrangement
            List<ProductTypeAddRequest> requests = new List<ProductTypeAddRequest>()
            {
                new ProductTypeAddRequest(){TypeNameEN="ENT", TypeNameFr="بهراشتی"},
                new ProductTypeAddRequest(){TypeNameEN="Laparascopy",TypeNameFr="محصولات لاپاراسگوپی"},
                new ProductTypeAddRequest(){TypeNameEN="surgery",TypeNameFr="محصولات اتاق غمل"}
            };
            List<ProductTypeResponse> ProductTypeFromAddMethod = new List<ProductTypeResponse>();
            foreach (ProductTypeAddRequest request in requests)
            {
               ProductTypeFromAddMethod.Add(await _productTypeSerivice.AddProductType(request));
            }
            //act
            List<ProductTypeResponse>? productTypeList_fromGetMethod =await _productTypeSerivice.GetAllProductTypes();
            //assert
            foreach (ProductTypeResponse item in ProductTypeFromAddMethod)
            {
                Assert.Contains(item, productTypeList_fromGetMethod);
            }
        }
        #endregion
        #region GetProductTypeByID
        [Fact]
        public async Task GetProductTypeByID_IDIsNull()
        {
            //Arrangment
            Guid? ProductTypId = null;
            //Act
            ProductTypeResponse? response =await _productTypeSerivice.GetProductTypeByID(ProductTypId);
            //Assert
            Assert.Null(response);
        }
        [Fact]
        public async void GetProductTypeByID_ValidID()
        {
            //arrange
            ProductTypeAddRequest request = new ProductTypeAddRequest()
            {
                TypeNameEN = "ENT",
                TypeNameFr = "بیهوشی"
            };
            ProductTypeResponse response =await _productTypeSerivice.AddProductType(request);
            //act
            ProductTypeResponse? response2 =await _productTypeSerivice.GetProductTypeByID(response.TypeId);
            //assert
            Assert.Equal(response, response2);
        }
        #endregion
    }
}