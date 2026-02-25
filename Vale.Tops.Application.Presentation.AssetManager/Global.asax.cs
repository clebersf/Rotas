using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Vale.Tops.Integration.Infrastructure.AD;

namespace Vale.Tops.Application.Presentation.AssetManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new MyPropertyActionFilter(), 0);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }

    public class MyPropertyActionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            AdInterface ui = new AdInterface();
            filterContext.Controller.ViewBag.Menu_01 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu01"]);
            filterContext.Controller.ViewBag.Menu_02 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu02"]);
            filterContext.Controller.ViewBag.Menu_03 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu03"]);
            filterContext.Controller.ViewBag.Menu_04 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu04"]);
            filterContext.Controller.ViewBag.Menu_05 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu05"]);
            filterContext.Controller.ViewBag.Menu_06 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu06"]);
            filterContext.Controller.ViewBag.Menu_07 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu07"]);
            filterContext.Controller.ViewBag.Menu_08 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu08"]);
            filterContext.Controller.ViewBag.Menu_09 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu09"]);
            filterContext.Controller.ViewBag.Menu_10 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu10"]);
            filterContext.Controller.ViewBag.Menu_11 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu11"]);
            filterContext.Controller.ViewBag.Menu_12 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu12"]);
            filterContext.Controller.ViewBag.Menu_13 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu13"]);
            filterContext.Controller.ViewBag.Menu_14 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu14"]);
            filterContext.Controller.ViewBag.Menu_15 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu15"]);
            filterContext.Controller.ViewBag.Menu_16 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu16"]);
            filterContext.Controller.ViewBag.Menu_17 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu17"]);
            filterContext.Controller.ViewBag.Menu_18 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu18"]);
            filterContext.Controller.ViewBag.Menu_19 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu19"]);
            filterContext.Controller.ViewBag.Menu_20 = true;//WebConfigurationManager.AppSettings["AdServer"], HttpContext.Current.User.Identity.Name, WebConfigurationManager.AppSettings["Menu20"]);
        }
    }
}
