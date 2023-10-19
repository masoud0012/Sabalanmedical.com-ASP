using Entities;
using IRepository;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using RepositoryServices;
using SabalanMedical.UI.Filters.ActionFilters;
using Serilog;
using ServiceContracts;
using Services;

namespace SabalanMedical.UI.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddControllersWithViews(option =>
            {
                //var logger=configuration.services.BuildServiceProvider().GetService<ILogger<Program>>();//GetService gets the services if availabe else null
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<GlobalActionFilter>>();//GetRequiredService gets the services if available else throw exceptions
                option.Filters.Add(new GlobalActionFilter(logger, "Key-From-Global", "ObjectValue-From-Global"));
            });
            services.AddScoped<IProductTypeService, ProductTypesService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductImageService, ProductImageService>();
            services.AddScoped<IProductDescService, ProductDescService>();
            services.AddScoped<IProductPropertyService, ProductPropertyService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            services.AddScoped<IProductImgRepository, ProductImageRepository>();
            services.AddScoped<IProductPropertyRepository, ProductPropertyRepository>();
            services.AddScoped<IProductDescRepository, ProductDescriptionRepository>();
            //Configure Databse
            services.AddDbContext<SabalanDbContext>(options => options.UseSqlServer(
                           configuration.GetConnectionString("DefaultConnection")));
            //Config Logging
            services.AddHttpLogging(option =>
            {
                option.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders | HttpLoggingFields.ResponsePropertiesAndHeaders;
            });
            return services;
        }
    }
}
