using System.Web;
using System.Web.Mvc;
using Shell.Filters;

namespace Shell
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthenticateMvcUserAttribute());
        }
    }
}
