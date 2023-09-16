using Entities;
using IRepository;
using Microsoft.EntityFrameworkCore;
using RepositoryServices;
using ServiceContracts;
using Services;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IProductTypeService, ProductTypesService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<IProductDescService, ProductDescService>();
builder.Services.AddScoped<IProductPropertyService, ProductPropertyService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddScoped<IProductImgRepository, ProductImageRepository>();
builder.Services.AddScoped<IProductPropertyRepository, ProductPropertyRepository>();
builder.Services.AddScoped<IProductDescRepository, ProductDescriptionRepository>();

builder.Services.AddDbContext<SabalanDbContext>(options => options.UseSqlServer(
               builder.Configuration.GetConnectionString("DefaultConnection")));
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
