using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using StructureMap;
using Coinco.SMS.AXWrapper;

namespace Coinco.SMS.Website
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            StructureMapConfiguration();
        }

        private void StructureMapConfiguration()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For<IAXHelper>().Use<AXHelper2012>();
            }
            );

        }
        protected void Session_Start()
        {
            //if (Context.Session != null)
            //{
            //    if (Context.Session.IsNewSession)
            //    {
            //        string sCookieHeader = Request.Headers["Cookie"];
            //        if ((null != sCookieHeader) && (sCookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
            //        {
            //            // intercept current route
            //            HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
            //            RouteData routeData = RouteTable.Routes.GetRouteData(currentContext);


            //            var domainRoute = routeData.Route;


            //            // substitute route values
            //            routeData.Values["controller"] = "WorkOrder";
            //            routeData.Values["action"] = "ServiceOrderWithHistory";


            //            // Clear the error on server.
            //            Server.ClearError();
            //            Response.Clear();


            //            // Call target Controller and pass the routeData.
            //            IController myController = new Controllers.WorkOrderController();
            //            myController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));


            //            Response.Flush();
            //            Response.End();
            //        }
            //    }
            //}
        }
        protected void Session_End(object sender, EventArgs e)
        {
            
        }

    }
}