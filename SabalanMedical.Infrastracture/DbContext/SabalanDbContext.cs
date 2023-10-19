using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SabalanMedical.Core.Domain.Entities.Tracking;

namespace Entities
{
    public class SabalanDbContext : DbContext
    {
        public SabalanDbContext(DbContextOptions options) : base(options)
        {

        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<ProductDesc> ProductDescs { get; set; }
        public virtual DbSet<ProductProperty> ProductProperties { get; set; }
        public virtual DbSet<ProductImg> ProductImgs { get; set; }
        #region Tracking Dbset
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Process> Processes { get; set; }
        public virtual DbSet<ProcessDetail> ProcessDetails { get; set; }
        public virtual DbSet<ProcessName> ProcessNames { get; set; }
        public virtual DbSet<ProductCat> ProductCats { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<ProductType>().ToTable("ProductTypes");
            modelBuilder.Entity<ProductDesc>().ToTable("ProductDescs");
            modelBuilder.Entity<ProductProperty>().ToTable("ProductProperties");
            modelBuilder.Entity<ProductImg>().ToTable("ProductImgs");
            #region Tracking tables 
            modelBuilder.Entity<Device>().ToTable("Devices");
            modelBuilder.Entity<Material>().ToTable("Materials");
            modelBuilder.Entity<Person>().ToTable("Persons");
            modelBuilder.Entity<Process>().ToTable("Processes");
            modelBuilder.Entity<ProcessDetail>().ToTable("ProcessDetails");
            modelBuilder.Entity<ProcessName>().ToTable("ProcessNames");
            modelBuilder.Entity<ProductCat>().ToTable("ProductCats");
            #endregion
            #region seed Tracking Data
            List<ProductCat>? productCats = new List<ProductCat>()
            {
                new ProductCat(){Id=Guid.NewGuid(),Category="RawMaterial"},
                new ProductCat(){Id=Guid.NewGuid(),Category="FinalProduct"},
                new ProductCat(){Id=Guid.NewGuid(),Category="Semi Final Prodct"}
            };
            if (productCats != null)
            {
                foreach (ProductCat item in productCats)
                {
                    modelBuilder.Entity<ProductCat>().HasData(item);
                }
            }

            List<ProcessName>? processNames = new List<ProcessName>()
            {
                new ProcessName(){Id=Guid.NewGuid(),Name="برش پارچه"},
                new ProcessName(){Id=Guid.NewGuid(),Name="تزریق پلاستیک"},
                new ProcessName(){Id=Guid.NewGuid(),Name="جوش التراسونیک"},
                new ProcessName(){Id=Guid.NewGuid(),Name="بسته بندی"},
                new ProcessName(){Id=Guid.NewGuid(),Name="خرید"}

            };
            if (processNames != null)
            {
                foreach (ProcessName item in processNames)
                {
                    modelBuilder.Entity<ProcessName>().HasData(item);
                }
            }

            List<Device>? devices = new List<Device>()
            {

                new Device(){Id=Guid.NewGuid(),Name="دستگاه جوش التراسونیک",Description=" "},
                new Device(){Id=Guid.NewGuid(),Name="دستگاه برش التراسونیک پارچه",Description=" "},
                new Device(){Id=Guid.NewGuid(),Name="دستگاه تزریق شماره 1",Description=" "},
                new Device(){Id=Guid.NewGuid(),Name="دستگاه تزریق شماره 2",Description=" "},
                new Device(){Id=Guid.NewGuid(),Name="دستگاه تزریق شماره 3",Description=" "},
                new Device(){Id=Guid.NewGuid(),Name="دستگاه تزریق شماره 4",Description=" "},
                new Device(){Id=Guid.NewGuid(),Name="دستگاه تزریق شماره عباسی",Description=" "},
                new Device(){Id=Guid.NewGuid(),Name="دستگاه سیلر بسته بندی",Description=" "}
            };
            if (devices != null)
            {
                foreach (Device item in devices)
                {
                    modelBuilder.Entity<Device>().HasData(item);
                }
            }

            List<Person>? people = new List<Person>()
            {
                new Person(){Id=Guid.NewGuid(),Name="عباسی"},
                new Person(){Id=Guid.NewGuid(),Name="پژمان"},
                new Person(){Id=Guid.NewGuid(),Name="مهندس عبدالله پور"},
                new Person(){Id=Guid.NewGuid(),Name="مسعود"},
                new Person(){Id=Guid.NewGuid(),Name="رضا"},
                new Person(){Id=Guid.NewGuid(),Name="اپراتور 1 عباسی"},
                new Person(){Id=Guid.NewGuid(),Name="اپراتور 2 عبداللهی"},
            };
            if (people != null)
            {
                foreach (Person item in people)
                {
                    modelBuilder.Entity<Person>().HasData(item);
                }
            }
            #endregion
            #region Seed Data
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
            #endregion
        }


        #region ProductStored Procedures
        public async Task<List<Product>> sp_GetAllProducts()
        {
            List<Product> products = await Products.FromSqlRaw("EXECUTE GetAllProducts").ToListAsync();
            return products.OrderBy(t => t.TypeId).ThenBy(t => t.ProductNameFr).ToList();
        }
        public int sp_AddProduct(Product product)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductId",product.Id),
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
                new SqlParameter("@ProductId",product.Id),
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
        public async Task<List<ProductType>> sp_GetAllProductTypes()
        {
            List<ProductType> allTypes = await ProductTypes.FromSqlRaw
                ("EXECUTE GetAllProductTypes").ToListAsync();
            return allTypes;
        }

        public List<ProductImg> sp_GetAllProductImages()
        {
            return ProductImgs.FromSqlRaw("EXECUTE GetAllProductImages").ToList();
        }

    }
}
