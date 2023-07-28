using Entities;
using System;
using System.ComponentModel.DataAnnotations;


namespace ServiceContracts.DTO.ProductDescriptionDTO
{
    public class ProductDescAddRequest
    {
        [Required(ErrorMessage = "ProductID can not be blank")]
        public Guid ProductID { get; set; }
        public string? DescTitle { get; set; }
        public string? Description { get; set; }

        public ProductDesc ToProductDesc()
        {
            return new ProductDesc()
            {
                DesctiptionID = Guid.NewGuid(),
                ProductID = ProductID,
                DescTitle = DescTitle,
            };
        }
    }
}
