using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Entities
{
    public class SabalanDbContext : DbContext
    {
        public SabalanDbContext(DbContextOptions options):base(options)
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
    }
}
