using Entities;
using IRepository;
using IRepository2;
using Microsoft.EntityFrameworkCore;
using RepositoryServices;
using RepositoryServices2;
using ServiceContracts;
using ServiceContracts.DTO.ProductsDTO;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IProductTypeService, ProductTypesService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IProductImageService, ProductImageService>();
builder.Services.AddTransient<IProductDescService, ProductDescService>();
builder.Services.AddTransient<IProductPropertyService, ProductPropertyService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddTransient<IProductImgRepository, ProductImageRepository>();
builder.Services.AddTransient<IProductPropertyRepository, ProductPropertyRepository>();
builder.Services.AddTransient<IProductDescRepository, ProductDescriptionRepository>();

builder.Services.AddDbContext<SabalanDbContext>(options => options.UseSqlServer(
               builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
//configure Rotativa-Convert to pdf- the directory in which wkhtmltox.exe is available
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();
