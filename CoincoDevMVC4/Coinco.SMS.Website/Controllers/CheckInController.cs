using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coinco.SMS.Website.Models;
using Telerik.Web.Mvc.UI;
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
            
           
            TempData.Keep();
            ViewData["ServiceOrder"] = serviceOrder;
            return View(serviceOrder);
        }

        [HttpPost]
        public ActionResult SelectionServerSide(string addressId)
        {
            TempData["AddressId"] = addressId;
            return View();
        }

        [GridAction]
        public ActionResult _SelectionClientSide_ServiceOrderLines(string siteId)
        {
            List <ServiceOrderLine> serviceOrderLineList= new List<ServiceOrderLine>();
            if (siteId != null)
            {
                TempData["SiteId"] = siteId;
            }
            return View(new GridModel<ServiceOrderLine>
            {
                Data = serviceOrderLineList

            });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _UpdateServiceOrderLine(string serialNumber)
       
        {
            List<ServiceOrderLine> serviceOrderLineExistingList = TempData["ServiceOrderLine"] as List<ServiceOrderLine>;
            ServiceOrderLine serviceOrderLineExisting = (from item in serviceOrderLineExistingList
                                                         where item.SerialNumber == serialNumber
                                                         select item).First();

            TryUpdateModel(serviceOrderLineExisting);
            TempData["ServiceOrderLine"] = serviceOrderLineExistingList;

            return View(new GridModel(serviceOrderLineExistingList));

        }
        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _DeleteServiceOrderLine(string serialNumber)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            if (TempData["ServiceOrderLine"] != null)
            {
                
                List<ServiceOrderLine> serviceOrderLineExistingList = TempData["ServiceOrderLine"] as List<ServiceOrderLine>;
                ServiceOrderLine serviceOrderLine = (from item in serviceOrderLineExistingList
                                                     where item.SerialNumber == serialNumber
                                                     select item).FirstOrDefault();
                if (serviceOrderLine != null)
                {
                    //Delete the record
                    serviceOrderLineExistingList.Remove(serviceOrderLine);
                }
             
                ViewData["ServiceOrderLine"] = serviceOrderLineExistingList;
            }
            TempData["ServiceOrderLine"] = ViewData["ServiceOrderLine"];
            TempData.Keep();
            return View(new GridModel<ServiceOrderLine>
            {
                Data = TempData["ServiceOrderLine"] as List<ServiceOrderLine>

            });
        }
        [GridAction]
        public ActionResult _GetCustomerAddresses(string customerAccount)
        {
            List<Address> addressList = new List<Address>();
            return View(new GridModel<Address>
            {

                Data = addressList

            });
        }


        [HttpGet]
        public ActionResult GetOtherDetails(string customerAccount)
        {
            WOClassification woClassification = new WOClassification();
            if (customerAccount == null)
            {
                List<WOClassification> woClassificationCollection = new List<WOClassification>();

                // IEnumerable<WOClassification> woClassificationCollection=woClassification.GetWOClassification(User.Identity.Name.ToString().Split('\\')[1]);
                woClassification.WOClassificationList = new SelectList(woClassificationCollection.AsEnumerable<WOClassification>(), "WOClassificationCode", "WOClassificationName", null);

            
                ViewData["WOClassificationList"] = woClassification.WOClassificationList;
                ServiceTechnician serviceTechnician = new ServiceTechnician();
                List<ServiceTechnician> serviceTechnicianCollection = new List<ServiceTechnician>();
                // IEnumerable<ServiceTechnician> serviceTechnicianCollection = serviceTechnician.GetTechnicians(User.Identity.Name.ToString().Split('\\')[1]);
                serviceTechnician.ServiceTechnicianList = new SelectList(serviceTechnicianCollection.AsEnumerable<ServiceTechnician>(), "ServiceTechnicianNo", "ServiceTechnicianName", null);

                ViewData["ServiceTechnicianList"] = serviceTechnician.ServiceTechnicianList;
                ViewData["ServiceResponsibleList"] = serviceTechnician.ServiceTechnicianList;

                PartDetails partDetails = new PartDetails();
                List<PartDetails> partDetailsCollection = new List<PartDetails>();
                //IEnumerable<PartDetails> partDetailsCollection = partDetails.GetItemNumbers(User.Identity.Name.ToString().Split('\\')[1]);

                partDetails.PartDetailsList = new SelectList(partDetailsCollection.AsEnumerable<PartDetails>(), "ItemNumber", "ProductName", null);
                ViewData["PartNumberList"] = partDetails.PartDetailsList;
            }
            else
            {
                // IEnumerable<WOClassification> woClassificationCollection = new IEnumerable<WOClassification;

                IEnumerable<WOClassification> woClassificationCollection = woClassification.GetWOClassification(User.Identity.Name.ToString().Split('\\')[1]);
                woClassification.WOClassificationList = new SelectList(woClassificationCollection, "WOClassificationCode", "WOClassificationName", null);

                ViewData["WOClassificationList"] = woClassification.WOClassificationList;
                ServiceTechnician serviceTechnician = new ServiceTechnician();
                //IEnumerable<ServiceTechnician> serviceTechnicianCollection = null;
                IEnumerable<ServiceTechnician> serviceTechnicianCollection = serviceTechnician.GetTechnicians(User.Identity.Name.ToString().Split('\\')[1]);
                serviceTechnician.ServiceTechnicianList = new SelectList(serviceTechnicianCollection, "ServiceTechnicianNo", "ServiceTechnicianName", null);

                ViewData["ServiceTechnicianList"] = serviceTechnician.ServiceTechnicianList;
                ViewData["ServiceResponsibleList"] = serviceTechnician.ServiceTechnicianList;

                PartDetails partDetails = new PartDetails();
                //IEnumerable<PartDetails> partDetailsCollection = null;
                IEnumerable<PartDetails> partDetailsCollection = partDetails.GetItemNumbers(User.Identity.Name.ToString().Split('\\')[1]);

                partDetails.PartDetailsList = new SelectList(partDetailsCollection, "ItemNumber", "ProductName", null);
                ViewData["PartNumberList"] = partDetails.PartDetailsList;
            }
            TempData.Keep();
            return View("OtherDetails");
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
            TempData["CustomerAccount"] = customerAccount;
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
        public ActionResult CreateServiceOrder( string customerAccount, string customerPo, string technicinanNo, string responsibleNo, string woClassification, string customerComments)
        {
            string userName = null;
            string newSerivceOrder = null;
            bool isSuccess = false;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            ServiceOrder serviceOrder = new ServiceOrder();
            ServiceOrderLine serviceOrderLine = new ServiceOrderLine();
            isSuccess = serviceOrder.CreateServiceOrder(TempData["SiteId"].ToString(), customerAccount, TempData["AddressId"] == null ? null : TempData["AddressId"].ToString(), customerPo, technicinanNo, responsibleNo, woClassification, customerComments, out newSerivceOrder, userName);
            if (isSuccess)
            {
                isSuccess = serviceOrderLine.CreateServiceOrderLinesItem(newSerivceOrder, (List<ServiceOrderLine>)TempData["ServiceOrderLine"], userName);
            }
            else
            {

            }
            if (isSuccess)
            {
                List<ServiceOrderLine> emptyList = new List<ServiceOrderLine>();
                ViewData["ServiceOrderLine"] = emptyList;
            }


            return View("ServiceOrderLine");
        }

        [HttpGet]
        public ActionResult GetServiceOrderLinesHistory(string partNumber, string serialNumber)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            List<ServiceOrderLine> serviceOrderList = new List<ServiceOrderLine>();
            ViewData["ServiceOrderLine"] = serviceOrderList;

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
