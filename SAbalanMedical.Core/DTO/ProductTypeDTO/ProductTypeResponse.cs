﻿using Entities;
using System;

namespace ServiceContracts.DTO.ProductTypeDTO
{
    /// <summary>
    /// DTO class that is used as return type for most ProductTypeService Methodes
    /// </summary>
    public class ProductTypeResponse
    {
        public Guid Id { get; set; }
        public string? TypeNameEn { get; set; }
        public string? TypeNameFr { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(ProductTypeResponse))
            {
                return false;
            }
            ProductTypeResponse productType = (ProductTypeResponse)obj;
            return productType.Id == Id && productType.TypeNameFr == TypeNameFr && productType.TypeNameEn == TypeNameEn;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"{TypeNameEn}, {TypeNameFr}";
        }
    }
    public static class ProductTypeExtension
    {
        public static ProductTypeResponse ToProductTypeResponse(this ProductType productType)
        {
            return new ProductTypeResponse()
            {
                Id = productType.Id,
                TypeNameEn = productType.TypeNameEN,
                TypeNameFr = productType.TypeNameFr,
            };
        }
    }
}
