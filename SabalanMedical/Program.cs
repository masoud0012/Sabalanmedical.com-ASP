using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IProductTypeService,ProductTypesService>();
builder.Services.AddTransient<IProductService,ProductService>();
builder.Services.AddTransient<IProductImageService,ProductImageService>();
builder.Services.AddTransient<IProductDescService,ProductDescService>();
builder.Services.AddTransient<IProductPropertyService,ProductPropertyService>();
builder.Services.AddDbContext<SabalanDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();
