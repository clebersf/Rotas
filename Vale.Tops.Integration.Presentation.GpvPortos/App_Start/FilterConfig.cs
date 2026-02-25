using System.Web;
using System.Web.Mvc;

namespace Vale.Tops.Integration.Presentation.GpvPortos
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
