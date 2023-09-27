﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO.ProductDescriptionDTO
{
    public class ProductDescUpdateRequest
    {
        public Guid DesctiptionID { get; set; }
        public Guid ProductID { get; set; }
        public string? DescTitle { get; set; }
        public string? Description { get; set; }

        public ProductDesc ToProductDesc()
        {
            return new ProductDesc()
            {
                Id =this.DesctiptionID,
                ProductID = this.ProductID,
                DescTitle = this.DescTitle,
                Description = this.Description
            };
        }
    }
}