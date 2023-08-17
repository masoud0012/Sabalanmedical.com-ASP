using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text.Json;

namespace Entities
{
    public class SabalanDbContext : DbContext
    {
        public SabalanDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductDesc> ProductDescs { get; set; }
        public DbSet<ProductProperty> ProductProperties { get; set; }
        public DbSet<ProductImg> ProductImgs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<ProductType>().ToTable("ProductTypes");
            modelBuilder.Entity<ProductDesc>().ToTable("ProductDescs");
            modelBuilder.Entity<ProductProperty>().ToTable("ProductProperties");
            modelBuilder.Entity<ProductImg>().ToTable("ProductImgs");

            string productsText = File.ReadAllText("wwwroot/json/tblproducts.json");
            List<Product>? productList = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(productsText);
            if (productList != null)
            {
                foreach (Product item in productList)
                {
                    modelBuilder.Entity<Product>().HasData(item);
                }
            }

            string typeText = File.ReadAllText("wwwroot/json/tblproducttype.json");
            List<ProductType>? alltypes = System.Text.Json.JsonSerializer.Deserialize<List<ProductType>>(typeText);
            if (alltypes != null)
            {
                foreach (ProductType item in alltypes)
                {
                    modelBuilder.Entity<ProductType>().HasData(item);
                }
            }

            string allImgText = File.ReadAllText("wwwroot/json/tblproductimages.json");
            List<ProductImg>? allImages = System.Text.Json.JsonSerializer.Deserialize<List<ProductImg>>(allImgText);
            if (allImages != null)
            {
                foreach (ProductImg item in allImages)
                {
                    modelBuilder.Entity<ProductImg>().HasData(item);
                }
            }

            string propertiesText = File.ReadAllText("wwwroot/json/tblproductfeatures.json");
            List<ProductProperty>? allProperties = System.Text.Json.JsonSerializer.Deserialize<List<ProductProperty>>(propertiesText);
            if (allProperties != null)
            {
                foreach (ProductProperty item in allProperties)
                {
                    modelBuilder.Entity<ProductProperty>().HasData(item);
                }
            }

            string descText = File.ReadAllText("wwwroot/json/tblproductdescription.json");
            List<ProductDesc>? allDescs = System.Text.Json.JsonSerializer.Deserialize<List<ProductDesc>>(descText);
            if (allDescs != null)
            {
                foreach (ProductDesc item in allDescs)
                {
                    modelBuilder.Entity<ProductDesc>().HasData(item);
                }
            }
        }

        #region ProductStored Procedures
        public List<Product> sp_GetAllProducts()
        {
            return Products.FromSqlRaw("EXECUTE GetAllProducts").ToList().OrderBy(t=>t.TypeId).OrderBy(t=>t.ProductNameEn).ToList();
        }
        public int sp_AddProduct(Product product)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductId",product.ProductID),
                new SqlParameter("@TypeId",product.TypeId),
                new SqlParameter("@ProductNameEn",product.ProductNameEn),
                new SqlParameter("@ProductNameFr",product.ProductNameFr),
                new SqlParameter("@ProductUrl",product.ProductUrl),
                new SqlParameter("@isHotSale",product.isHotSale),
                new SqlParameter("@isManufactured",product.isManufactured),
            };
            return Database.ExecuteSqlRaw("EXECUTE AddProduct @ProductID, @TypeId, @ProductNameEn," +
                " @ProductNameFr, @ProductUrl, @isHotSale, @isManufactured"
                , parameters);
        }
        public int sp_UpdateProduct(Product product)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductId",product.ProductID),
                new SqlParameter("@TypeId",product.TypeId),
                new SqlParameter("@ProductNameEn",product.ProductNameEn),
                new SqlParameter("@ProductNameFr",product.ProductNameFr),
                new SqlParameter("@ProductUrl",product.ProductUrl),
                new SqlParameter("@isHotSale",product.isHotSale),
                new SqlParameter("@isManufactured",product.isManufactured),
            };
            return Database.ExecuteSqlRaw($"EXECUTE UpdateProduct @ProductId,@TypeId,@ProductNameEn," +
                $"@ProductNameFr,@ProductUrl,@isHotSale,@isManufactured", parameters);
        }
        public Product sp_GetProductById(Guid ProductId)
        {
            SqlParameter parameter = new SqlParameter("@ProductId", ProductId);
            List<Product> product = Products.FromSqlRaw("EXECUTE GetProductById @ProductId", parameter).ToList();
            return product.FirstOrDefault();
        }
        #endregion
        public List<ProductType> sp_GetAllProductTypes()
        {
            return ProductTypes.FromSqlRaw("EXECUTE GetAllProductTypes").ToList();
        }

        public List<ProductImg> sp_GetAllProductImages()
        {
            return ProductImgs.FromSqlRaw("EXECUTE GetAllProductImages").ToList();
        }

    }
}
