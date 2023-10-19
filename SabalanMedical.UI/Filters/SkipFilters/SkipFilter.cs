using Microsoft.AspNetCore.Mvc.Filters;

namespace SabalanMedical.UI.Filters.SkipFilters
{
    public class SkipFilter:Attribute,IFilterMetadata
    {
        int x=50;
    }
}
