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
            ServiceOrder serviceOrder = new ServiceOrder();
            Customer customer = new Customer();

            IEnumerable<Customer> customerCollection = customer.GetCustomers(User.Identity.Name.ToString().Split('\\')[1]);
            customer.CustomerList = new SelectList(customerCollection, "CustomerAccount", "CustomerName", null);
            serviceOrder.Customer = customer;
            ViewData["CustomerList"] = customer.CustomerList;
            WOClassification woClassification = new WOClassification();
            IEnumerable<WOClassification> woClassificationCollection=woClassification.GetWOClassification(User.Identity.Name.ToString().Split('\\')[1]);
             woClassification.WOClassificationList = new SelectList(woClassificationCollection, "WOClassificationCode", "WOClassificationName", null);
             serviceOrder.WOClassification = woClassification;
            ViewData["WOClassificationList"] = woClassification.WOClassificationList;
            ServiceTechnician serviceTechnician = new ServiceTechnician();
            serviceTechnician.ServiceTechnicianList = new SelectList(serviceTechnician.GetTechnicians(User.Identity.Name.ToString().Split('\\')[1]), "ServiceTechnicianNo", "ServiceTechnicianName", null);
            serviceOrder.ServiceTechnician = serviceTechnician;
            ViewData["ServiceTechnicianList"] = serviceTechnician.ServiceTechnicianList;
            ViewData["ServiceResponsibleList"] = serviceTechnician.ServiceTechnicianList;
            serviceOrder.ServiceResponsible = serviceTechnician;
            PartDetails partDetails = new PartDetails();
            partDetails.PartDetailsList = new SelectList(partDetails.GetItemNumbers(User.Identity.Name.ToString().Split('\\')[1]), "ItemNumber", "ProductName", null);
            ViewData["PartNumberList"] = partDetails.PartDetailsList;
           
            TempData.Keep();
            ViewData["ServiceOrder"] = serviceOrder;
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
                Data = GetServiceOrderLinesBySerialNumberPartNumber("","","")

            });
        }

        [GridAction]
        public ActionResult _GetCustomerAddresses(string customerAccount)
        {
            return PartialView(new GridModel<Address>
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
            return View("Address");
        }

        [HttpPost]
        public ActionResult GetServiceOrderLinesHistoryBySerialNumberPartNumber(ServiceOrder model,string serialNumber, string partNumber)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            ViewData["ServiceOrderLine"] = GetServiceOrderLinesBySerialNumberPartNumber(partNumber, serialNumber, "");
            TempData.Keep();
            return View("ServiceOrderLine");
        }

        [HttpGet]
        public ActionResult GetServiceOrderLinesHistory(string partNumber, string serialNumber)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            ViewData["ServiceOrderLine"] = GetServiceOrderLinesBySerialNumberPartNumber(partNumber,serialNumber,"");

            TempData.Keep();
            return View("ServiceOrderLine", ViewData["ServiceOrderLine"]);
        }

        private List<ServiceOrderLine> GetServiceOrderLinesBySerialNumberPartNumber(string partNumber,string serialNumber,string customerAccount)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            List<ServiceOrderLine> serviceOrderLine = (new ServiceOrderLine()).GetServiceOrderLinesBySerialNumberPartNumber(serialNumber, partNumber, customerAccount, userName);
            return serviceOrderLine;
        }
    }
}
