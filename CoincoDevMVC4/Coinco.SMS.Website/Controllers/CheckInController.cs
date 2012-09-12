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
            IEnumerable<Customer> customerCollection = customer.GetCustomers(User.Identity.Name.ToString().Split('\\')[1]);
            customer.CustomerList = new SelectList(customerCollection, "CustomerAccount", "CustomerName", null);
            ViewData["CustomerList"] = customer.CustomerList;
            WOClassification woClassification = new WOClassification();
            IEnumerable<WOClassification> woClassificationCollection=woClassification.GetWOClassification(User.Identity.Name.ToString().Split('\\')[1]);
             woClassification.WOClassificationList = new SelectList(woClassificationCollection, "WOClassificationCode", "WOClassificationName", null);
            ViewData["WOClassificationList"] = woClassification.WOClassificationList;
            ServiceTechnician serviceTechnician = new ServiceTechnician();
            serviceTechnician.ServiceTechnicianList = new SelectList(serviceTechnician.GetTechnicians(User.Identity.Name.ToString().Split('\\')[1]), "ServiceTechnicianNo", "ServiceTechnicianName", null);
            ViewData["ServiceTechnicianList"] = serviceTechnician.ServiceTechnicianList;
            ViewData["ServiceResponsibleList"] = serviceTechnician.ServiceTechnicianList;

            PartDetails partDetails = new PartDetails();
            partDetails.PartDetailsList = new SelectList(partDetails.GetItemNumbers(User.Identity.Name.ToString().Split('\\')[1]), "ItemNumber", "ProductName", null);
            ViewData["PartNumberList"] = partDetails.PartDetailsList;
            TempData.Keep();
            return View();
        }

        [GridAction]
        public ActionResult _SelectionClientSide_ServiceOrderLines(string siteId)
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

        [GridAction]
        public ActionResult _GetCustomerAddresses(string customerAccount)
        {
            return View(new GridModel<Address>
            {
                Data =(new Address()).GetCustomerAddress(customerAccount, User.Identity.Name.ToString().Split('\\')[1])

            });
        }

        [HttpGet]
        public ActionResult GetCustomerAddresses(string customerAccount)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            List<Address> addressList = (new Address()).GetCustomerAddress(customerAccount, userName);
            ViewData["BillingAddress"] = addressList;
            ViewData["ShippingAddress"] = addressList;
          //  TempData["CustomerAccount"] = customerAccount;
            TempData.Keep();
            return View("BillingAddress");
        }

        private List<ServiceOrderLine> GetSerialNumbersHistory(string siteId)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            List<ServiceOrderLine> serviceOrderLine = (new ServiceOrderLine()).GetSerialNumbersHistory("", "", "", userName);
            return serviceOrderLine;
        }
    }
}
