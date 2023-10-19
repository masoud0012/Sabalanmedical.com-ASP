using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SabalanMedical.UI.Filters.ExceptionFilters
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HandleExceptionFilter> _logger;
        private readonly IWebHostEnvironment _env;
        public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger
            , IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }
        public void OnException(ExceptionContext context)
        {
            _logger.LogError("{FilterName}.{FilterMethod}\n" +
                "Exception Type={ExceptionType}:\nException Error={ExceptionName}"
                , nameof(HandleExceptionFilter), nameof(OnException),
                context.Exception.GetType().ToString(),
                context.Exception.Message);
            if (_env.IsDevelopment())
            {
                context.Result = new ContentResult()
                {
                    Content = $"Exception Type={context.Exception.GetType().ToString()}\n" +
                    $"Exception Error={context.Exception.Message}"
                };
            }
            else if (_env.IsProduction())
            {
                _logger.LogInformation(context.HttpContext.Connection.Id);
                context.Result = new StatusCodeResult(StatusCodes.Status400BadRequest);
                
            }
        }
    }
}

