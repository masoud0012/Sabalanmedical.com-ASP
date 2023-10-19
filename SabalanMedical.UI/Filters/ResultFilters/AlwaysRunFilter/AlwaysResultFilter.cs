using Microsoft.AspNetCore.Mvc.Filters;
using SabalanMedical.UI.Filters.SkipFilters;
using System.Runtime.CompilerServices;

namespace SabalanMedical.UI.Filters.ResultFilters.AlwaysRunFilter
{
    public class AlwaysResultFilter : IAlwaysRunResultFilter
    {
        private readonly ILogger<AlwaysResultFilter> _logger;
        public AlwaysResultFilter(ILogger<AlwaysResultFilter> logger)
        {

            _logger = logger;

        }
        public void OnResultExecuted(ResultExecutedContext context)
        {
            
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Filters.OfType<SkipFilter>().Any())
            {
                return;
            }
        }
    }
}
