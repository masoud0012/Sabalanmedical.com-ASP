using ServiceContracts;
using Services;
using Entities;
using ServiceContracts.DTO.ProductTypeDTO;

namespace TestProject
{
    public class ProductTypeServiceTest
    {
        private readonly IProductTypeService _productTypeSerivice;

        public ProductTypeServiceTest(IProductTypeService productTypeService)
        {
            _productTypeSerivice = productTypeService;
        }
        #region Add ProductType
        //ProductType is null.Throw ArgumentNullException
        [Fact]
        public void AddProductType_IsNull()
        {
            //Arrange
            ProductTypeAddRequest? request = null;
            //Act

            //Assertion
            Assert.Throws<ArgumentNullException>(() =>
            {
                _productTypeSerivice.AddProductType(request);
            });
        }
        //ProductType Name is null, Throw ArumentException
        [Fact]
        public void AddProdutType_NameIsNull()
        {
            //Arrange
            ProductTypeAddRequest request = new ProductTypeAddRequest()
            {
                TypeNameEN = null,
                TypeNameFr = null
            };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _productTypeSerivice.AddProductType(request);
            });
        }
        //ProducyType Name Duplicate, Throw ArgumentException
        [Fact]
        public void AddProductType_TypeIsDuplicated()
        {
            //Arrangment
            ProductTypeAddRequest request1 = new ProductTypeAddRequest()
            {
                TypeNameEN = "ENT1",
                TypeNameFr = "بیهوشی تنفسی"
            };
            ProductTypeAddRequest request2 = new ProductTypeAddRequest()
            {
                TypeNameEN = "ENT",
                TypeNameFr = "بیهوشی تنفسی"
            };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _productTypeSerivice.AddProductType(request1);
                _productTypeSerivice.AddProductType(request2);
            });
        }
        //Proper ProductType,add the product type
        [Fact]
        public void AddProductType_ProperObject()
        {
            //Arrangment
            ProductTypeAddRequest request = new ProductTypeAddRequest()
            {
                TypeNameEN = "ENT",
                TypeNameFr = "محصولات بیهوشی تنفسی"
            };
            //Act
            ProductTypeResponse response = _productTypeSerivice.AddProductType(request);
            List<ProductTypeResponse>? allProductTypes = _productTypeSerivice.GettAllProductTypes();
            //Assert
            Assert.Contains(response, allProductTypes);
            Assert.True(response.TypeId != Guid.Empty);
        }
        #endregion
        #region Get All Product Types
        [Fact]
        public void GetAllProductTypes_EmptyList()
        {
            //arrangement
            //act
            List<ProductTypeResponse>? productTypeList = _productTypeSerivice.GettAllProductTypes();
            //assert
            Assert.Empty(productTypeList);
        }
        [Fact]
        public void GetAllProductTypes_PropperList()
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
                ProductTypeFromAddMethod.Add(_productTypeSerivice.AddProductType(request));
            }
            //act
            List<ProductTypeResponse>? productTypeList_fromGetMethod = _productTypeSerivice.GettAllProductTypes();
            //assert
            foreach (ProductTypeResponse item in ProductTypeFromAddMethod)
            {
                Assert.Contains(item, productTypeList_fromGetMethod);
            }
        }
        #endregion
        #region GetProductTypeByID
        [Fact]
        public void GetProductTypeByID_IDIsNull()
        {
            //Arrangment
            Guid? ProductTypId = null;
            //Act
            ProductTypeResponse? response = _productTypeSerivice.GetProductTypeByID(ProductTypId);
            //Assert
            Assert.Null(response);
        }
        [Fact]
        public void GetProductTypeByID_ValidID()
        {
            //arrange
            ProductTypeAddRequest request = new ProductTypeAddRequest()
            {
                TypeNameEN = "ENT",
                TypeNameFr = "بیهوشی"
            };
            ProductTypeResponse response = _productTypeSerivice.AddProductType(request);
            //act
            ProductTypeResponse? response2 = _productTypeSerivice.GetProductTypeByID(response.TypeId);
            //assert
            Assert.Equal(response, response2);
        }
        #endregion
    }
}