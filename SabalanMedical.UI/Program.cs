using Entities;
using IRepository;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using RepositoryServices;
using SabalanMedical.UI.Filters.ActionFilters;
using Serilog;
using ServiceContracts;
using Services;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(option =>
{
    //var logger=builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();//GetService gets the service if availabe else null
    var logger=builder.Services.BuildServiceProvider().GetRequiredService<ILogger<GlobalActionFilter>>();//GetRequiredService gets the service if available else throw exceptions
    option.Filters.Add(new GlobalActionFilter(logger, "Key-From-Global", "ObjectValue-From-Global"));
});
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
//Configure Databse
builder.Services.AddDbContext<SabalanDbContext>(options => options.UseSqlServer(
               builder.Configuration.GetConnectionString("DefaultConnection")));

//Config Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration)//Read Configuration setting from built in IConfiguration
    .ReadFrom.Services(service);//Read out current app's Services and make them available to serilog
});

//Config Logging
builder.Services.AddHttpLogging(option =>
{
    option.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders | HttpLoggingFields.ResponsePropertiesAndHeaders;
});


var app = builder.Build();
app.UseSerilogRequestLogging();//Used for IDiagnostic end loging


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpLogging();//for logging ILoger
//configure Rotativa-Convert to pdf- the directory in which wkhtmltox.exe is available
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();
