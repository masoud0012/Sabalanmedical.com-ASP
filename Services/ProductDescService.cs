using Entities;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ProductDescResponse>? AddProductDesc(ProductDescAddRequest? request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            ValidationHelper.ModelValidation(request);
            ProductDesc productDesc = request.ToProductDesc();
            await _sabalanDbContext.ProductDescs.AddAsync(productDesc);
            await _sabalanDbContext.SaveChangesAsync();
            return productDesc.ToProductDescResponse();
        }

        public async Task<bool> DeleteProductDesc(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            ProductDesc? response = await _sabalanDbContext.ProductDescs.FirstOrDefaultAsync(t => t.DesctiptionID == id);
            if (response is null)
            {
                return false;
            }
            _sabalanDbContext.ProductDescs.Remove(response);
            await _sabalanDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductDescResponse>>? GetAllProductDesc()
        {
            return _sabalanDbContext.ProductDescs.Select(t => t.ToProductDescResponse()).ToList();
        }

        public async Task<ProductDescResponse>? GetProductDescByDescID(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            ProductDesc? productDesc = await _sabalanDbContext.ProductDescs.
                FirstOrDefaultAsync(t => t.DesctiptionID == id);
            if (productDesc == null)
            {
                throw new ArgumentException("No Description was found!");
            }
            return productDesc.ToProductDescResponse();
        }

        public async Task<List<ProductDescResponse>>? GetProductDescByProductID(Guid? productID)
        {
            List<ProductDesc> productDescResponses = _sabalanDbContext.ProductDescs.Where(t =>
            t.ProductID == productID).ToList();
            return productDescResponses.Select(t => t.ToProductDescResponse()).ToList();
        }

        public async Task<ProductDescResponse>? UpdateProductDesc(ProductDescUpdateRequest? updateRequest)
        {
            if (updateRequest == null)
            {
                throw new ArgumentNullException(nameof(updateRequest));
            }
            ProductDesc? response = await _sabalanDbContext.ProductDescs.FirstOrDefaultAsync(t => t.DesctiptionID == updateRequest.DesctiptionID);
            if (response == null)
            {
                throw new ArgumentException("No description was found");
            }
            response.DesctiptionID = updateRequest.DesctiptionID;
            response.ProductID = updateRequest.ProductID;
            response.DescTitle = updateRequest.DescTitle;
            response.Description = updateRequest.Description;
            await _sabalanDbContext.SaveChangesAsync();
            return response.ToProductDescResponse();
        }
    }
}
