using Entities;
using ServiceContracts;
using ServiceContracts.DTO.ProductDescriptionDTO;
using Services.Helpers;
using System;

namespace Services
{
    public class ProductDescService : IProductDescService
    {
        private readonly SabalanDbContext _sabalanDbContext;
        public ProductDescService(SabalanDbContext sabalanDbContext)
        {
            _sabalanDbContext = sabalanDbContext;
        }
        public ProductDescResponse? AddProductDesc(ProductDescAddRequest? request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            ValidationHelper.ModelValidation(request);
            ProductDesc productDesc = request.ToProductDesc();
            _sabalanDbContext.ProductDescs.Add(productDesc);
            _sabalanDbContext.SaveChanges();
            return productDesc.ToProductDescResponse();
        }

        public bool DeleteProductDesc(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            ProductDesc? response = _sabalanDbContext.ProductDescs.FirstOrDefault(t => t.DesctiptionID == id);
            if (response is null)
            {
                return false;
            }
            _sabalanDbContext.ProductDescs.Remove(response);
            _sabalanDbContext.SaveChanges();
            return true ;
        }

        public List<ProductDescResponse>? GetAllProductDesc()
        {
            return _sabalanDbContext.ProductDescs.Select(t => t.ToProductDescResponse()).ToList();
        }

        public ProductDescResponse? GetProductDescByDescID(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            ProductDesc? productDesc = _sabalanDbContext.ProductDescs.FirstOrDefault(t => t.DesctiptionID == id);
            if (productDesc == null)
            {
                throw new ArgumentException("No Description was found!");
            }
            return productDesc.ToProductDescResponse();
        }

        public List<ProductDescResponse>? GetProductDescByProductID(Guid? productID)
        {
            List<ProductDesc> productDescResponses = _sabalanDbContext.ProductDescs.Where(t =>
            t.ProductID == productID).ToList();
            return productDescResponses.Select(t=>t.ToProductDescResponse()).ToList();
        }

        public ProductDescResponse? UpdateProductDesc(ProductDescUpdateRequest? updateRequest)
        {
            if (updateRequest == null)
            {
                throw new ArgumentNullException(nameof(updateRequest));
            }
            ProductDesc? response = _sabalanDbContext.ProductDescs.FirstOrDefault(t => t.DesctiptionID == updateRequest.DesctiptionID);
            if (response == null)
            {
                throw new ArgumentException("No description was found");
            }
            response.DesctiptionID = updateRequest.DesctiptionID;
            response.ProductID = updateRequest.ProductID;
            response.DescTitle = updateRequest.DescTitle;
            response.Description = updateRequest.Description;
            _sabalanDbContext.SaveChanges();
            return response.ToProductDescResponse();
        }
    }
}
