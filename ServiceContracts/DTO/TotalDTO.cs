using ServiceContracts.DTO.ProductDescriptionDTO;
using ServiceContracts.DTO.ProductImageDTO;
using ServiceContracts.DTO.ProductPropertyDTO;
using ServiceContracts.DTO.ProductsDTO;
using System;


namespace ServiceContracts.DTO
{
    public class TotalDTO
    {
        public ProductResponse? ProductResponses { get; set; }
        public List<ProductImageResponse>? ProductImageResponses { get; set; }
        public List<ProductDescResponse>? ProductDescResponses { get; set; }
        public List<ProductPropertyResponse>? ProductPropertyResponses { get; set; }
    }
}
