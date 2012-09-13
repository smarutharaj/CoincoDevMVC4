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
            return View(serviceOrder);
        }

        public ActionResult CheckBoxesServerSide(string[] checkedRecords,string customerAccount)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            checkedRecords = checkedRecords ?? new string[] { };
            ViewData["checkedRecords"] = checkedRecords;

            if (checkedRecords.Any())
            {
                ViewData["checkedOrders"] = (new Address()).GetCustomerAddress(customerAccount,userName).Where(o => checkedRecords.Contains(o.AddressId));
            }
            return View(GetCustomerAddresses(customerAccount));
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
            List<Address> addressShipping = (from item1 in addressList
                                 where item1.IsShipping == "1"
                                 select item1).ToList<Address>();
            List<Address> addressBilling = (from item1 in addressList
                                             where item1.IsBilling == "1"
                                             select item1).ToList<Address>();
            ViewData["BillingAddress"] = addressBilling;
            ViewData["ShippingAddress"] = addressShipping;
          //  TempData["CustomerAccount"] = customerAccount;
            TempData.Keep();
            return View("Address");
        }

        [HttpPost]
        public ActionResult GetServiceOrderLinesHistoryBySerialNumberPartNumber(ServiceOrder model,string serialNumber, string partNumber)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            if (TempData["ServiceOrderLine"] == null)
            {
                ViewData["ServiceOrderLine"] = GetServiceOrderLinesBySerialNumberPartNumber(partNumber, serialNumber, "");
            }
            else
            {
                List<ServiceOrderLine> serviceOrderLineExistingList = TempData["ServiceOrderLine"] as List<ServiceOrderLine>;
                List<ServiceOrderLine> serviceOrderLineNewList = GetServiceOrderLinesBySerialNumberPartNumber(partNumber, serialNumber, "");
                serviceOrderLineExistingList.Add(serviceOrderLineNewList.First());
                ViewData["ServiceOrderLine"] = serviceOrderLineExistingList;
            }
            TempData["ServiceOrderLine"]=ViewData["ServiceOrderLine"];
            TempData.Keep();
            return View("ServiceOrderLine");
        }

        [HttpPost]
        public ActionResult CreateServiceOrder(ServiceOrder model, string customerAccount, string addressId, string customerPo, string technicinanNo, string responsibleNo, string woClassification, string customerComments)
        {
            string userName = null;
            string newSerivceOrder = null;
            bool isSuccess = false;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            ServiceOrder serviceOrder = new ServiceOrder();
            ServiceOrderLine serviceOrderLine = new ServiceOrderLine();
            isSuccess=  serviceOrder.CreateServiceOrder(TempData["SiteId"].ToString(), customerAccount, addressId, customerPo, technicinanNo, responsibleNo, woClassification, customerComments, out newSerivceOrder, userName);
            if (isSuccess)
            {
                isSuccess = serviceOrderLine.CreateServiceOrderLinesItem(newSerivceOrder, (List<ServiceOrderLine>)TempData["ServiceOrderLine"], userName);
            }
            ViewData["ServiceOrderLine"] = TempData["ServiceOrderLine"];
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
