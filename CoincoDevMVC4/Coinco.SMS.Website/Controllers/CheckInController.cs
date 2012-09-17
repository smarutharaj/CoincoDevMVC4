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
        public ActionResult CheckIn()
        {
            ServiceOrder serviceOrder = new ServiceOrder();
            Customer customer = new Customer();
            WOClassification woClassification = new WOClassification();
            List<WOClassification> woClassificationCollection = new List<WOClassification>();
            ServiceTechnician serviceTechnician = new ServiceTechnician();
            List<ServiceTechnician> serviceTechnicianCollection = new List<ServiceTechnician>();
            PartDetails partDetails = new PartDetails();
            List<PartDetails> partDetailsCollection = new List<PartDetails>();
            List<ServiceOrderLine> serviceOrderLineList = new List<ServiceOrderLine>();
            List<Address> addressList = new List<Address>();
            try
            {
                IEnumerable<Customer> customerCollection = customer.GetCustomers(User.Identity.Name.ToString().Split('\\')[1]);
                customer.CustomerList = new SelectList(customerCollection, "CustomerAccount", "CustomerName", null);
                serviceOrder.Customer = customer;
                ViewData["CustomerList"] = customer.CustomerList;

                woClassification.WOClassificationList = new SelectList(woClassificationCollection.AsEnumerable<WOClassification>(), "WOClassificationCode", "WOClassificationName", null);
                ViewData["WOClassificationList"] = woClassification.WOClassificationList;
                
                serviceTechnician.ServiceTechnicianList = new SelectList(serviceTechnicianCollection.AsEnumerable<ServiceTechnician>(), "ServiceTechnicianNo", "ServiceTechnicianName", null);
                ViewData["ServiceTechnicianList"] = serviceTechnician.ServiceTechnicianList;
                ViewData["ServiceResponsibleList"] = serviceTechnician.ServiceTechnicianList;

                partDetails.PartDetailsList = new SelectList(partDetailsCollection.AsEnumerable<PartDetails>(), "ItemNumber", "ProductName", null);
                ViewData["PartNumberList"] = partDetails.PartDetailsList;
      
                ViewData["ServiceOrderLine"] = serviceOrderLineList;
                TempData["ServiceOrderLine"] = serviceOrderLineList;

                ViewData["BillingAddress"] = addressList;
                ViewData["ShippingAddress"] = addressList;

                ViewData["ServiceOrder"] = serviceOrder;
                TempData.Keep();
            }
            catch(Exception ex)
            {
                throw (ex);
            }

            return View(serviceOrder);
        }

        #region "Service Order Line Grid Actions"

        [GridAction]
        public ActionResult _SelectionClientSide_ServiceOrderLines(string siteId)
        {
            List <ServiceOrderLine> serviceOrderLineList= new List<ServiceOrderLine>();
            try
            {
                if (siteId != null)
                {
                    TempData["SiteId"] = siteId;
                }
                TempData.Keep();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return View(new GridModel<ServiceOrderLine>
            {
                Data = serviceOrderLineList

            });
        }

        [GridAction]
        public ActionResult _SelectionClientSide_ServiceInfoLines(string siteId)
        {
            List<SalesHistory> serviceInfoLineList = new List<SalesHistory>();
            try
            {
                if (siteId != null)
                {
                    TempData["SiteId"] = siteId;
                }
                TempData.Keep();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(new GridModel<SalesHistory>
            {
                Data = serviceInfoLineList

            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _UpdateServiceOrderLine(string serialNumber)
       
        {List<ServiceOrderLine> serviceOrderLineExistingList =new List<ServiceOrderLine>();
            try
            {
                serviceOrderLineExistingList = TempData["ServiceOrderLine"] as List<ServiceOrderLine>;
                ServiceOrderLine serviceOrderLineExisting = (from item in serviceOrderLineExistingList
                                                             where item.SerialNumber == serialNumber
                                                             select item).First();

                TryUpdateModel(serviceOrderLineExisting);
                TempData["ServiceOrderLine"] = serviceOrderLineExistingList;
                TempData.Keep();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View(new GridModel(serviceOrderLineExistingList));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _DeleteServiceOrderLine(string serialNumber)
        {
            string userName = null;
            try
            {         
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(new GridModel<ServiceOrderLine>
            {
                Data = TempData["ServiceOrderLine"] as List<ServiceOrderLine>

            });
        }

        #endregion

        #region "Customer Address Grid Actions"

        [GridAction]
        public ActionResult _GetCustomerAddresses(string customerAccount)
        {
            List<Address> addressList = new List<Address>();
            TempData.Keep();
            return View(new GridModel<Address>
            {
                Data = addressList
            });
        }
        
        #endregion

        #region "Other Details Get Actions"

        [HttpGet]
        public ActionResult GetOtherDetails(string customerAccount)
        {
            WOClassification woClassification = new WOClassification();
            ServiceTechnician serviceTechnician = new ServiceTechnician();
            PartDetails partDetails = new PartDetails();
            try
            {
                if (customerAccount != null)
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
                    if (addressShipping.Count > 1)
                    {
                        addressShipping[0].IsSelected = "checked";
                    }
                    ViewData["BillingAddress"] = addressBilling;
                    ViewData["ShippingAddress"] = addressShipping;
                    TempData["CustomerAccount"] = customerAccount;

                    IEnumerable<WOClassification> woClassificationCollection = woClassification.GetWOClassification(User.Identity.Name.ToString().Split('\\')[1]);
                    woClassification.WOClassificationList = new SelectList(woClassificationCollection, "WOClassificationCode", "WOClassificationName", null);
                    ViewData["WOClassificationList"] = woClassification.WOClassificationList;               

                    IEnumerable<ServiceTechnician> serviceTechnicianCollection = serviceTechnician.GetTechnicians(User.Identity.Name.ToString().Split('\\')[1]);
                    serviceTechnician.ServiceTechnicianList = new SelectList(serviceTechnicianCollection, "ServiceTechnicianNo", "ServiceTechnicianName", null);
                    ViewData["ServiceTechnicianList"] = serviceTechnician.ServiceTechnicianList;
                    ViewData["ServiceResponsibleList"] = serviceTechnician.ServiceTechnicianList;

                    IEnumerable<PartDetails> partDetailsCollection = partDetails.GetItemNumbers(User.Identity.Name.ToString().Split('\\')[1]);
                    partDetails.PartDetailsList = new SelectList(partDetailsCollection, "ItemNumber", "ProductName", null);
                    ViewData["PartNumberList"] = partDetails.PartDetailsList;
                }
                TempData.Keep();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View("OtherDetails");
        }

        #endregion

        #region Sales History Details Get Action


        public JsonResult GetSalesHistoryDetails(string serialNo)
        {
            string userName = null;
            SalesHistory salesHistory = new SalesHistory();
            List<SalesHistory> salesInfoList = new List<SalesHistory>();

            try
            {
                userName = User.Identity.Name.ToString().Split('\\')[1];
                salesInfoList = salesHistory.GetSalesDetails(serialNo, userName);
                salesHistory.SalesSerialNumber = salesInfoList[0].SalesSerialNumber;
                salesHistory.ItemNumber = salesInfoList[0].ItemNumber;
                salesHistory.SalesOrderNumber = salesInfoList[0].SalesOrderNumber;
                salesHistory.CustomerName = salesInfoList[0].CustomerName;
                salesHistory.InvoiceNumber = salesInfoList[0].InvoiceNumber;
                salesHistory.InvoiceDate = salesInfoList[0].InvoiceDate;
                salesHistory.ServiceInfoList = salesHistory.GetServiceDetails(serialNo, userName);
                ViewData["ServiceInformation"] = salesHistory.ServiceInfoList;
                TempData.Keep();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Json(salesHistory);
        }
        #endregion

        #region "Service Order Line Grid Post Actions"

        [HttpPost]
        public ActionResult ClearServiceOrderLines()
        {
            List<ServiceOrderLine> serviceOrderLineEmptyList = new List<ServiceOrderLine>();
            try
            {
                TempData["ServiceOrderLine"] = serviceOrderLineEmptyList;
                ViewData["ServiceOrderLine"] = serviceOrderLineEmptyList;
                TempData.Keep();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View("ServiceOrderLine");
        }
        
        [HttpPost]
        public ActionResult GetServiceOrderLinesHistoryBySerialNumberPartNumber(ServiceOrder model,string serialNumber, string partNumber)
        {
            string userName = null;
            try
            {
                userName = User.Identity.Name.ToString().Split('\\')[1];
                if (TempData["ServiceOrderLine"] == null)
                {
                    ViewData["ServiceOrderLine"] = GetServiceOrderLinesBySerialNumberPartNumber(partNumber, serialNumber, "");
                }
                else
                {
                    List<ServiceOrderLine> serviceOrderLineExistingList = TempData["ServiceOrderLine"] as List<ServiceOrderLine>;
                    List<ServiceOrderLine> serviceOrderLineNewList = GetServiceOrderLinesBySerialNumberPartNumber(partNumber, serialNumber, "");
                    if (serviceOrderLineNewList.Count > 0)
                    {
                        serviceOrderLineExistingList.Add(serviceOrderLineNewList.First());
                    }
                    ViewData["ServiceOrderLine"] = serviceOrderLineExistingList;
                }
                TempData["ServiceOrderLine"] = ViewData["ServiceOrderLine"];
                TempData.Keep();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View("ServiceOrderLine");
        }

        [HttpPost]
        public ActionResult CreateServiceOrder( string customerAccount, string customerPo,string addressId, string technicinanNo, string responsibleNo, string woClassification, string customerComments)
        {
            string userName = null;
            string newSerivceOrder = null;
            bool isSuccess = false;
            ServiceOrder serviceOrder = new ServiceOrder();
            ServiceOrderLine serviceOrderLine = new ServiceOrderLine();
            try
            {
                userName = User.Identity.Name.ToString().Split('\\')[1];

                isSuccess = serviceOrder.CreateServiceOrder(TempData["SiteId"].ToString(), customerAccount, addressId == null ? null : addressId.ToString(), customerPo, technicinanNo, responsibleNo, woClassification, customerComments, out newSerivceOrder, userName);
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
                    TempData["ServiceOrderLine"] = emptyList;
                }
                TempData.Keep();
            }
            catch (Exception ex)
            {
                throw ex;
            }    
            return View("ServiceOrderLine");
        }

        #endregion

        #region "Methods"

        private List<ServiceOrderLine> GetServiceOrderLinesBySerialNumberPartNumber(string partNumber,string serialNumber,string customerAccount)
        {
            List<ServiceOrderLine> serviceOrderLine = new List<ServiceOrderLine>();
            string userName = null;
            try
            {
                userName = User.Identity.Name.ToString().Split('\\')[1];
                serviceOrderLine = (new ServiceOrderLine()).GetServiceOrderLinesBySerialNumberPartNumber(serialNumber, partNumber, customerAccount, userName);
            }
            catch (Exception ex)
            {
                throw ex;
            }   
            return serviceOrderLine;
        }

        #endregion
    }

}
