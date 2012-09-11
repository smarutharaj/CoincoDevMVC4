using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coinco.SMS.Website.Models;
using Telerik.Web.Mvc;
using System.Text;

namespace Coinco.SMS.Controllers
{
    public class WorkOrderController : Controller
    {

        // GET: /WorkOrder/
        [HttpGet]
        public ActionResult ServiceOrderWithHistory(string siteId, string process)
        {
            if (siteId == null)
            {
                GetSites();
                if (process == null)
                {
                    process = "-1";
                }
            }
            else
            {
                TempData["SiteId"] = siteId;
                TempData.Keep();
            }
            ViewData["ServiceOrder"] = GetServiceOrders(TempData["SiteId"].ToString(), process);
            ViewData["ServiceOrderLine"] = GetServiceOrderLinesByServiceOrderID("");
            return View();
        }

        [GridAction]
        public ActionResult _SelectionClientSide_ServiceOrders(string siteId, string process)
        {
            if (siteId != null)
            {
                TempData["SiteId"] = siteId;
            }
            return View(new GridModel<ServiceOrder>
            {
                Data = GetServiceOrders(TempData["SiteId"].ToString(), process)
                           
            });
        }

        [GridAction]
        public ActionResult _SelectionClientSide_SerialNumber(string serviceOrderId)
        {
            //serviceOrderId = serviceOrderId ?? "";
            Session["SID"] = serviceOrderId;
            return View(new GridModel<ServiceOrderLine>
            {
                Data = GetServiceOrderLinesByServiceOrderID(serviceOrderId)
            });
        }

        //Get Sites for the authenticated user
        private string GetSites()
        {
            Site site = new Site();
            IEnumerable<Site> siteCollection = null;
            if (site.SiteList == null)
            {
                string userName = null;
                userName = User.Identity.Name.ToString().Split('\\')[1];

                siteCollection = site.GetSitesListByUsername(userName);
                site.SiteList = new SelectList(siteCollection, "SiteId", "SiteName", siteCollection.First<Site>().SiteID);

            }
            TempData["SiteId"] = siteCollection.First<Site>().SiteID;
            TempData["FeaturedSites"] = site.SiteList;
            TempData.Keep();

            return siteCollection.First<Site>().SiteID;
        }

        //Get ServiceOrders by SiteID
        private List<ServiceOrder> GetServiceOrders(string siteId, string process)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            List<ServiceOrder> serviceOrder = (new ServiceOrder()).GetServiceOrders(siteId, process, "", userName);
            return serviceOrder;
        }

        //Get Service Order Lines by Selected Service Order
        private List<ServiceOrderLine> GetServiceOrderLinesByServiceOrderID(string serviceOrderId)
        {
            List<ServiceOrderLine> serviceOrderLine = (new ServiceOrderLine()).GetServiceOrdersLinesByServiceOrder(serviceOrderId, "vvinothkum");
            return serviceOrderLine;
        }

        //
        // GET: /ServiceOrderProcess/
        public ActionResult ServiceOrderProcess()
        {
            TempData.Keep();
            return View();
        }
    }
}
