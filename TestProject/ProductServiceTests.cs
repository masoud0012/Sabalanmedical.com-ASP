using Entities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using ServiceContracts;
using ServiceContracts.DTO.ProductsDTO;
using ServiceContracts.DTO.ProductTypeDTO;
using ServiceContracts.Enums;
using Services;
using System;
using Xunit.Abstractions;

namespace TestProject
{
    public class ProductServiceTests
    {
        private readonly IProductService _productService;
        private readonly ITestOutputHelper _testHelper;
        private readonly IProductTypeService _productTypeService;
        private readonly SabalanDbContext _sabalanDbContext;

        public ProductServiceTests(ITestOutputHelper testOutputHelper
            ,IProductTypeService productTypeService
            ,IProductService productService,
            SabalanDbContext sabalanDbContext)
        {
            _sabalanDbContext = sabalanDbContext;
            _productService = productService;
            _testHelper = testOutputHelper;
            _productTypeService = productTypeService;
        }
        #region AddProduct
        [Fact]
        public void AddProduct_ProductIsNull()
        {
            //arrangement
            ProductAddRequest? request = null;
            //act

            //assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _productService.AddProduct(request);
            });
        }
        [Fact]
        public void AddProduct_ProductNameIsNull()
        {
            //arrangement
            ProductAddRequest request = new ProductAddRequest()
            {
                ProductNameEn = null,
                ProductNameFr = null
            };
            //act

            //assert
            Assert.Throws<ArgumentException>(() =>
            {
                _productService.AddProduct(request);
            });
        }

        [Fact]
        public void AddProduct_ProperProduct()
        {
            ProductAddRequest request = new ProductAddRequest()
            {
                TypeId = Guid.NewGuid(),
                ProductNameEn = "Reload",
                ProductNameFr = "کارتریج",
            };
            //act
            ProductResponse productResponse = _productService.AddProduct(request);
            //assert
            Assert.True(productResponse.ProductId != Guid.Empty);
            Assert.Contains(productResponse, _productService.GetAllProducts());
        }
        #endregion

        #region GetProductID
        [Fact]
        public void GetPersonById_NullID()
        {
            //arrangement
            Guid? ProductId = null;
            //act
            ProductResponse? response = _productService.GetProductById(ProductId);
            //asser
            Assert.Null(response);
        }
        [Fact]
        public void GetPersonByID_properID()
        {
            //arrangment
            ProductAddRequest request = new ProductAddRequest()
            {
                ProductNameEn = "Reload",
                ProductNameFr = "کارتریج",
                TypeId = Guid.NewGuid()
            };
            ProductResponse productResponse_FromAdd = _productService.AddProduct(request);
            //act
            ProductResponse? productResponse_FromGetById = _productService.GetProductById(productResponse_FromAdd.ProductId);
            //assert
            Assert.Equal(productResponse_FromAdd, productResponse_FromGetById);
        }
        #endregion

        #region GetAllProducts
        [Fact]
        public void GetAllProducts_EmptyList()
        {
            Assert.Empty(_productService.GetAllProducts());
        }
        [Fact]
        public void GetAllProducts_SomeProdcuts()
        {
            //arrangement
            List<ProductTypeAddRequest> productTypeAddRequests = new List<ProductTypeAddRequest>()
            {
                new ProductTypeAddRequest(){TypeNameEN="ENT",TypeNameFr="محصولات بهداشتی"},
                new ProductTypeAddRequest(){TypeNameEN="Surgery",TypeNameFr="محصولات عمل"},
            };
            ProductTypeResponse TypeResponse1 = _productTypeService.AddProductType(productTypeAddRequests[0]);
            ProductTypeResponse TypeResponse2 = _productTypeService.AddProductType(productTypeAddRequests[1]);

            List<ProductAddRequest> requests = new List<ProductAddRequest>()
            {
                new ProductAddRequest(){ProductNameEn="Reload1",ProductNameFr="کارتریج1",TypeId=TypeResponse1.TypeId},
                new ProductAddRequest(){ProductNameEn="Reload2",ProductNameFr="کارتریج2",TypeId=TypeResponse2.TypeId},
                new ProductAddRequest(){ProductNameEn="Reload3",ProductNameFr="3کارتریج",TypeId=TypeResponse1.TypeId}
            };
            List<ProductResponse> responses_fromAdd = requests.Select(temp => _productService.AddProduct(temp)).ToList();

            _testHelper.WriteLine("all products frpm responses_fromAdd Method");
            foreach (ProductResponse item in responses_fromAdd)
            {
                _testHelper.WriteLine(item.ToString() + "\n");
            }

            //act
            List<ProductResponse> responses_fromGelAll = _productService.GetAllProducts();
            _testHelper.WriteLine("all products frpm GetAllProducts Method");
            foreach (ProductResponse item in responses_fromGelAll)
            {
                _testHelper.WriteLine(item.ToString() + "\n");
            }
            //assert
            Assert.Equivalent(responses_fromAdd, responses_fromGelAll);
        }
        #endregion

        #region GetFilteredProducts
        [Fact]
        public void GetFilteredProducts_SearchKeyIsEmpty()
        {
            //arrangement
            List<ProductTypeAddRequest> productTypeAddRequests = new List<ProductTypeAddRequest>()
            {
                new ProductTypeAddRequest(){TypeNameEN="ENT",TypeNameFr="محصولات بهداشتی"},
                new ProductTypeAddRequest(){TypeNameEN="Surgery",TypeNameFr="محصولات عمل"},
            };
            ProductTypeResponse TypeResponse1 = _productTypeService.AddProductType(productTypeAddRequests[0]);
            ProductTypeResponse TypeResponse2 = _productTypeService.AddProductType(productTypeAddRequests[1]);

            List<ProductAddRequest> requests = new List<ProductAddRequest>()
            {
                new ProductAddRequest(){ProductNameEn="Reload1",ProductNameFr="کارتریج1",TypeId=TypeResponse1.TypeId},
                new ProductAddRequest(){ProductNameEn="Reload2",ProductNameFr="کارتریج2",TypeId=TypeResponse2.TypeId},
                new ProductAddRequest(){ProductNameEn="Reload3",ProductNameFr="3کارتریج",TypeId=TypeResponse1.TypeId}
            };
            List<ProductResponse> responses_fromAdd = requests.Select(temp => _productService.AddProduct(temp)).ToList();
            _testHelper.WriteLine("all products frpm responses_fromAdd Method");

            //act
            List<ProductResponse>? responses_fromFilteredProducts =
                _productService.GetFilteredProduct(nameof(ProductResponse.ProductNameEn), "");
            _testHelper.WriteLine("all products frpm GetAllProducts Method");
            if (responses_fromFilteredProducts is not null)
            {
                foreach (ProductResponse item in responses_fromFilteredProducts)
                {
                    _testHelper.WriteLine(item.ToString() + "\n");
                }
            }
            //assert
            Assert.Equivalent(responses_fromAdd, responses_fromFilteredProducts);
        }

        [Fact]
        public void GetFilteredProducts_SearchByProductName()
        {
            //arrangement
            List<ProductTypeAddRequest> productTypeAddRequests = new List<ProductTypeAddRequest>()
            {
                new ProductTypeAddRequest(){TypeNameEN="ENT",TypeNameFr="محصولات بهداشتی"},
                new ProductTypeAddRequest(){TypeNameEN="Surgery",TypeNameFr="محصولات عمل"},
            };
            ProductTypeResponse TypeResponse1 = _productTypeService.AddProductType(productTypeAddRequests[0]);
            ProductTypeResponse TypeResponse2 = _productTypeService.AddProductType(productTypeAddRequests[1]);

            List<ProductAddRequest> requests = new List<ProductAddRequest>()
            {
                new ProductAddRequest(){ProductNameEn="Splint",ProductNameFr="اسپیلنت",TypeId=TypeResponse1.TypeId},
                new ProductAddRequest(){ProductNameEn="ASplint",ProductNameFr="اسپیلنت",TypeId=TypeResponse1.TypeId},
                new ProductAddRequest(){ProductNameEn="HMspE FIlter",ProductNameFr="فیلتر",TypeId=TypeResponse2.TypeId},
                new ProductAddRequest(){ProductNameEn="Reload",ProductNameFr="3کارتریج",TypeId=TypeResponse1.TypeId}
            };
            List<ProductTypeResponse> typeresponse = _productTypeService.GettAllProductTypes();
            List<ProductResponse> responses_fromAdd = requests.Select(temp => _productService.AddProduct(temp)).ToList();

            //act
            List<ProductResponse>? responses_fromFilteredProducts =
                _productService.GetFilteredProduct(nameof(ProductResponse.TypeNameEN), "sp");
            _testHelper.WriteLine("all products frpm GetAllProducts Method");
            if (responses_fromFilteredProducts is not null)
            {
                foreach (ProductResponse item in responses_fromFilteredProducts)
                {
                    _testHelper.WriteLine(item.ToString() + "\n");
                }
            }
            //assert
            foreach (ProductResponse item in responses_fromAdd)
            {
                if (item.ProductNameEn != null)
                {
                    if (item.ProductNameEn.Contains("sp", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(item, responses_fromFilteredProducts);
                    }
                }
            }
        }
        #endregion

        #region GetSortedProducts
        //sort desc products based on productNameEN
        [Fact]
        public void GetSortedProducts()
        {
            //arrangement
            List<ProductTypeAddRequest> productTypeAddRequests = new List<ProductTypeAddRequest>()
            {
                new ProductTypeAddRequest(){TypeNameEN="ENT",TypeNameFr="محصولات بهداشتی"},
                new ProductTypeAddRequest(){TypeNameEN="Surgery",TypeNameFr="محصولات عمل"},
            };
            ProductTypeResponse TypeResponse1 = _productTypeService.AddProductType(productTypeAddRequests[0]);
            ProductTypeResponse TypeResponse2 = _productTypeService.AddProductType(productTypeAddRequests[1]);

            List<ProductAddRequest> requests = new List<ProductAddRequest>()
            {
                new ProductAddRequest(){ProductNameEn="Splint",ProductNameFr="اسپیلنت",TypeId=TypeResponse1.TypeId},
                new ProductAddRequest(){ProductNameEn="ASplint",ProductNameFr="بهداشتی",TypeId=TypeResponse1.TypeId},
                new ProductAddRequest(){ProductNameEn="HMspE FIlter",ProductNameFr="فیلتر",TypeId=TypeResponse2.TypeId},
                new ProductAddRequest(){ProductNameEn="Reload",ProductNameFr="کارتریج",TypeId=TypeResponse1.TypeId}
            };

            List<ProductResponse> responses_fromAdd = requests.Select(temp => _productService.AddProduct(temp)).ToList();

            //act
            List<ProductResponse>? responses_fromSorted =
                _productService.GetSortedProducts(responses_fromAdd, nameof(ProductResponse.ProductNameFr), SortOrderOptions.Asc);
            _testHelper.WriteLine("all products frpm Sorted Method");
            if (responses_fromSorted is not null)
            {
                foreach (ProductResponse item in responses_fromSorted)
                {
                    _testHelper.WriteLine(item.ToString() + "\n");
                }
            }
            responses_fromAdd = responses_fromAdd.OrderBy(t => t.ProductNameFr).ToList();
            //assert
            for (int i = 0; i < responses_fromSorted.Count; i++)
            {
                Assert.Equal(responses_fromSorted[i], responses_fromAdd[i]);
            }
        }
        #endregion

        #region UpdateProduct
        [Fact]
        public void UpdateProduct_RequestIsNull()
        {
            ProductUpdateRequest? request = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                _productService.UpdateProduct(request);
            });
        }
        [Fact]
        public void ProductUpdate_InvalidProductID()
        {
            //Arrangement
            ProductUpdateRequest productUpdateRequest = new ProductUpdateRequest()
            {
                ProductID = Guid.NewGuid(),
                ProductNameEn = "ENT",
                ProductNameFr = "محصولات",
                TypeId = Guid.NewGuid()
            };
            //act
            Assert.Throws<ArgumentException>(() =>
            {
                _productService.UpdateProduct(productUpdateRequest);
            });
            //assert
        }
        [Fact]
        public void ProductUpdate_ProductNameIsNull()
        {
            ProductTypeAddRequest productTypeAddRequest = new ProductTypeAddRequest()
            {
                TypeNameEN = "ENT",
                TypeNameFr = "محصولات"
            };
            ProductTypeResponse typeResponse = _productTypeService.AddProductType(productTypeAddRequest);
            ProductAddRequest productAddRequest = new ProductAddRequest()
            {
                ProductNameEn = "ENT",
                ProductNameFr = "محصول",
                TypeId = typeResponse.TypeId
            };

            ProductResponse productResponse = _productService.AddProduct(productAddRequest);
            ProductUpdateRequest productUpdateRequest = productResponse.ToProductUpdateRequest();
            productUpdateRequest.ProductNameEn = null;
            //act
            Assert.Throws<ArgumentException>(() =>
            {
                _productService.UpdateProduct(productUpdateRequest);
            });
        }


        [Fact]
        public void UpdateProduct_RequestIsProper()
        {
            ProductTypeAddRequest typeRequest = new ProductTypeAddRequest() { TypeNameEN = "ENT", TypeNameFr = "محصولات" };

            ProductTypeResponse typeResponse = _productTypeService.AddProductType(typeRequest);

            ProductAddRequest productRequest = new ProductAddRequest() { TypeId = typeResponse.TypeId, ProductNameEn = "Splint", ProductNameFr = "اسپیلنت" };

            ProductResponse productResponse = _productService.AddProduct(productRequest);
            _testHelper.WriteLine("productResponse");
            _testHelper.WriteLine(productResponse.ToString());

            ProductUpdateRequest productUpdateRequest = productResponse.ToProductUpdateRequest();
            productUpdateRequest.ProductNameEn = "BVF";
            productUpdateRequest.ProductNameFr = "بیهوشی تنفسی";

            _testHelper.WriteLine("productUpdateRequest");
            _testHelper.WriteLine(productUpdateRequest.ProductID + "--" + productUpdateRequest.ProductNameEn);

            ProductResponse productResponse_FromUpdateMethod = _productService.UpdateProduct(productUpdateRequest);
            ProductResponse? productResponse_fromGetById = _productService.GetProductById(productResponse_FromUpdateMethod.ProductId);
            _testHelper.WriteLine("productUpdateRequest");
            _testHelper.WriteLine(productUpdateRequest.ToString());

            _testHelper.WriteLine("productResponse_FromUpdateMethod");
            _testHelper.WriteLine(productResponse_FromUpdateMethod.ToString());

            _testHelper.WriteLine("productResponse_fromGetById");
            _testHelper.WriteLine(productResponse_fromGetById.ToString());
            //assert
            Assert.Equal(productResponse_FromUpdateMethod.ProductNameEn, productUpdateRequest.ProductNameEn);
            Assert.Equal(productResponse_FromUpdateMethod, productResponse_fromGetById);

        }

        #endregion
        #region DeletProduct
        [Fact]
        public void DeleteProduct_ProductIdIsNull()
        {
            Guid? productID = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                _productService.DeleteProduct(productID);
            });
        }
        [Fact]
        public void DeleteProduct_InvalidProductId()
        {
            bool isDeleted = _productService.DeleteProduct(Guid.NewGuid());
            //assert
            Assert.False(isDeleted);
        }
        [Fact]
        public void DeleteProduct_ProperProductId()
        {
            //arrangement
            ProductTypeAddRequest typeRequest = new ProductTypeAddRequest()
            {
                TypeNameEN = "ENT",
                TypeNameFr = "محصولات"
            };
            ProductTypeResponse typeResponse = _productTypeService.AddProductType(typeRequest);
            ProductAddRequest productAddRequest = new ProductAddRequest()
            {
                ProductNameEn = "Ent",
                ProductNameFr = "محصولات",
                TypeId = typeResponse.TypeId
            };

            ProductResponse response = _productService.AddProduct(productAddRequest);
            //arrangement
            bool result = _productService.DeleteProduct(response.ProductId);
            //assert
            Assert.True(result);
        }
        #endregion
    }
}
