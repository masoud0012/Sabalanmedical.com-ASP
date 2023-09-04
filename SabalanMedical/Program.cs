using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using RepositoryServices;
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
builder.Services.AddTransient<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddDbContext<SabalanDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
   
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
