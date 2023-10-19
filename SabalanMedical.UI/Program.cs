using SabalanMedical.UI.StartupExtensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices(builder.Configuration);
//Config Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration)//Read Configuration setting from built in IConfiguration
    .ReadFrom.Services(service);//Read out current app's Services and make them available to serilog
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
