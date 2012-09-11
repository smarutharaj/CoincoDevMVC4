using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coinco.SMS.Website.Models;
using Telerik.Web.Mvc;
namespace Coinco.SMS.Website.Controllers
{
    public class CheckInController : Controller
    {
        //
        // GET: /CheckIn/

        public ActionResult CheckIn()
        {
            Customer customer = new Customer();
            customer.CustomerList = new SelectList(customer.GetCustomers(User.Identity.Name.ToString().Split('\\')[1]), "CustomerAccount", "CustomerName");
            ViewData["CustomerList"] = customer.CustomerList;
            TempData.Keep();
            return View();
        }

        [GridAction]
        public ActionResult _SelectionClientSide_ServiceOrders(string siteId)
        {
            if (siteId != null)
            {
                TempData["SiteId"] = siteId;
            }
            return View(new GridModel<ServiceOrderLine>
            {
                Data = GetSerialNumbersHistory(TempData["SiteId"].ToString())

            });
        }

        private List<ServiceOrderLine> GetSerialNumbersHistory(string siteId)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            List<ServiceOrderLine> serviceOrderLine = (new ServiceOrderLine()).GetSerialNumbersHistory(siteId, "-1", "", userName);
            return serviceOrderLine;
        }
    }
}
