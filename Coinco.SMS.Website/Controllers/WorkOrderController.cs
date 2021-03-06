﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coinco.SMS.Website.Models;
using Telerik.Web.Mvc;
using System.Text;

namespace Coinco.SMS.Website.Controllers
{
    public class WorkOrderController : Controller
    {

        // GET: /WorkOrder/
        #region "ServiceOrderWith History"
        [HandleError]
        [OutputCache(Duration = 0)] 
        [HttpGet]
        public ActionResult ServiceOrderWithHistory(string siteId, string process)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            try
            {
               
                if (String.IsNullOrEmpty(siteId))
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
                    Session["SiteID"] = siteId;
                    Site site = new Site();
                    IEnumerable<Site> existingSites = (IEnumerable<Site>)TempData["AllSites"];
                    site.SiteList = new SelectList(existingSites, "SiteId", "SiteName", siteId);
                    TempData["FeaturedSites"] = site.SiteList;
                    ViewData["FeaturedSites"] = site.SiteList;
                    TempData.Keep();
                }
                ViewData["ServiceOrder"] = GetServiceOrders(TempData["SiteId"].ToString(), process);
                ViewData["ServiceOrderLine"] = GetServiceOrderLinesByServiceOrderID("");
            }
            catch (Exception ex)
            {
                TempData.Keep();
                ExceptionLog.LogException(ex, userName);
                //return Json("No Sites assigned for this user in AX Dynamics", JsonRequestBehavior.AllowGet);
                //return RedirectToAction("ServiceOrderWithHistory");
            }
            return View("ServiceOrderWithHistory");
        }

        [GridAction]
        public ActionResult _SelectionClientSide_ServiceOrders(string siteId, string process)
        {
            if (siteId != null)
            {
                TempData["SiteId"] = siteId;
                Session["SiteID"] = siteId;
            }
            return View(new GridModel<ServiceOrder>
            {
                Data = GetServiceOrders(TempData["SiteId"].ToString(), process)

            });
        }

        [GridAction]
        public ActionResult _SelectionClientSide_SerialNumber(string serviceOrderId, string serviceOrderStatus)
        {
            //serviceOrderId = serviceOrderId ?? "";
            Session["SID"] = serviceOrderId;
            Session["SOStatus"] = serviceOrderStatus;
            TempData["ServiceOrderId"] = serviceOrderId;
            TempData.Keep();
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
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            try
            {
                if (site.SiteList == null)
                {
                  

                    siteCollection = site.GetSitesListByUsername(userName);
                    if (siteCollection.Count() > 0)
                    {
                        site.SiteList = new SelectList(siteCollection, "SiteId", "SiteName", siteCollection.First<Site>().SiteID);
                        TempData["SiteId"] = siteCollection.First<Site>().SiteID;
                        Session["SiteID"] = TempData["SiteId"];
                        TempData["AllSites"] = siteCollection;
                        TempData["FeaturedSites"] = site.SiteList;
                        ViewData["FeaturedSites"] = site.SiteList;
                        TempData.Keep();
                    }
                    else
                    {
                        throw new Exception("No Sites assigned for this user in AX Dynamics");
                    }

                }
              
            }
            catch (Exception ex)
            {
                TempData.Keep();
                ExceptionLog.LogException(ex, userName);
                throw ex;
                //return Json("Select the service order number in Service Order with history page", JsonRequestBehavior.AllowGet);
            }
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
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            List<ServiceOrderLine> serviceOrderLine = (new ServiceOrderLine()).GetServiceOrdersLinesByServiceOrder(serviceOrderId, userName);
            return serviceOrderLine;
        }

        #endregion


        #region "ServiceOrderProcess"
        // GET: /ServiceOrderProcess/
       
        public ActionResult ServiceOrderProcess()
        {
            string userName = "";
            TempData["ServiceOrderId"] = Session["SID"];
            TempData["WorkOrderSiteId"] = Session["SiteID"];
            userName = User.Identity.Name.ToString().Split('\\')[1];
            SerivceOrderPartLine serivceOrderPartLineObject = new SerivceOrderPartLine();
            ServiceOrderLine serivceOrderLineObject = new ServiceOrderLine();
            List<ServiceOrderLine> serviceOrderLineList = new List<ServiceOrderLine>();
           
            try
            {
                if (HttpContext.Session != null)
                {
                    if (Session["SiteID"] == null)
                    {
                        return RedirectToAction("ServiceOrderWithHistory");
                    }
                }
                if (TempData["ServiceOrderId"] != null)
                {

                    serivceOrderLineObject.ServiceOrderLineList = serviceOrderLineList;
                    serivceOrderPartLineObject.ServiceOrderLine = serivceOrderLineObject;

                    FailureCode failureCodeObject = new FailureCode();
                    IEnumerable<FailureCode> failureCodeCollection = failureCodeObject.GetFailureCode(userName);
                    failureCodeObject.FailureCodeList = new SelectList(failureCodeCollection, "FailureCodeNo", "FailureDescription", null);
                    ViewData["FailureCodeList"] = failureCodeObject.FailureCodeList;

                    PartDetails partDetails = new PartDetails();
                    partDetails.PartDetailsList = new SelectList(partDetails.GetItemNumbers(userName), "ItemNumber", "ItemNumber", null);
                    ViewData["PartNumberList"] = partDetails.PartDetailsList;

                    LineProperty LinePropertyObject = new LineProperty();
                    IEnumerable<LineProperty> LinePropertyCollection = LinePropertyObject.GetLineProperty(userName);
                    LinePropertyObject.LinePropertyList = new SelectList(LinePropertyCollection, "LinePropertyCode", "LinePropertyDescription", null);
                    ViewData["LinePropertyList"] = LinePropertyObject.LinePropertyList;



                    IEnumerable<SerivceOrderPartLine> serviceOrderPartLineCollection = null;
                   
                    serviceOrderPartLineCollection = serivceOrderPartLineObject.GetSerialNumberByServiceOrder(TempData["ServiceOrderId"].ToString(), userName);
                   
                  
                    serivceOrderPartLineObject.ServiceOrderPartLineList = new SelectList(serviceOrderPartLineCollection, "ServiceObjectRelation", "SerialNumber", null);
                   

                    ViewData["SORelationList"] = serivceOrderPartLineObject.ServiceOrderPartLineList;
                    ViewData["WorkSerialNumberList"] = serivceOrderPartLineObject.ServiceOrderPartLineList;
                    ViewData["ServiceOrderPartLines"] = GetServiceOrderPartLinesByServiceOrderID(TempData["ServiceOrderId"].ToString());


                    ServiceOrder ServiceOrder = new ServiceOrder();
                    ServiceOrder.ServiceOrderList = GetServiceOrderDetailsByServiceOrder(TempData["WorkOrderSiteId"].ToString(), TempData["ServiceOrderId"].ToString());
                    serivceOrderPartLineObject.ServiceOrders = ServiceOrder;
                    ViewData["ServiceOrderLineinProcess"] = serivceOrderPartLineObject.ServiceOrders.ServiceOrderList;

                    ServiceTechnician serviceTechnician = new ServiceTechnician();
                    IEnumerable<ServiceTechnician> serviceTechnicianCollection = null;
                    serviceTechnicianCollection=serviceTechnician.GetTechnicians(userName);

                    if (!String.IsNullOrEmpty(TempData["Technician"].ToString()))
                    {
                        serviceTechnician.ServiceTechnicianList = new SelectList(serviceTechnicianCollection, "ServiceTechnicianNo", "ServiceTechnicianName", serviceTechnicianCollection.First<ServiceTechnician>().ServiceTechnicianNo = TempData["Technician"].ToString());
                    }
                    else
                    {
                        serviceTechnician.ServiceTechnicianList = new SelectList(serviceTechnicianCollection, "ServiceTechnicianNo", "ServiceTechnicianName", null);
                    }
                    ViewData["ServiceTechnicianLoadList"] = serviceTechnician.ServiceTechnicianList;

                    Site site = new Site();
                    IEnumerable<Site> siteCollection = null;
                    siteCollection = site.GetSites(userName);
                    if (!String.IsNullOrEmpty(TempData["WorkOrderSiteId"].ToString()))
                    {
                        site.SiteList = new SelectList(siteCollection, "SiteId", "SiteName", siteCollection.First<Site>().SiteID = TempData["WorkOrderSiteId"].ToString());
                    }
                    else
                    {
                        site.SiteList = new SelectList(siteCollection, "SiteId", "SiteName", siteCollection.First<Site>().SiteID);
                    }


                    ViewData["siteList"] = site.SiteList;
                    ViewData["TranasactionTypes"] = TransactionType.GetTransactionTypes();
                    TempData.Keep();
                }
                else
                {
                   return RedirectToAction("ServiceOrderWithHistory");
                   // return View("ServiceOrderWithHistory", serivceOrderPartLineObject);
                    //throw new Exception("Select the service order number in Service Order with history page");
                    
                }
            }
            catch (Exception ex)
            {
                TempData.Keep();
                ExceptionLog.LogException(ex, userName);
               // throw new Exception("Select the service order number in Service Order with history page");
                //return RedirectToAction("ServiceOrderWithHistory");
             //  return Json("Select the service order number in Service Order with history page",JsonRequestBehavior.AllowGet);
                
            }
            
            return View(serivceOrderPartLineObject);

        }

        [GridAction]
        public ActionResult _SelectionClientSide_serviceOrderPartLines(string serviceOrderId)
        {
            Session["SID"] = serviceOrderId;
            TempData["ServiceOrderId"] = serviceOrderId;
            TempData.Keep();
            if (!String.IsNullOrEmpty(serviceOrderId))
            {
                return View(new GridModel<SerivceOrderPartLine>
                {
                    Data = GetServiceOrderPartLinesByServiceOrderID(serviceOrderId)
                });
            }
            return View("ServiceOrderPartLinesView");
        }

        [OutputCache(Duration = 0)] 
        [HttpGet]
        public ActionResult GetServiceOrderLineBySerialNumberOrderProcess(string serialNumber)
        {
            string userName = null;
            ServiceOrderLine serviceOrderLineObject = new ServiceOrderLine();
            List<ServiceOrderLine> serviceOrderLineList = new List<ServiceOrderLine>();
            userName = User.Identity.Name.ToString().Split('\\')[1];
            try
            {
                
                if (!String.IsNullOrEmpty(serialNumber))
                {
                    serviceOrderLineList = serviceOrderLineObject.GetServiceOrderLinesDetailsBySerialNumber(serialNumber, "", userName);
                    if (serviceOrderLineList.Count > 0)
                    {
                        serviceOrderLineObject.PartNumber = serviceOrderLineList[0].PartNumber;
                        serviceOrderLineObject.Warranty = serviceOrderLineList[0].Warranty;
                        serviceOrderLineObject.RepairType = serviceOrderLineList[0].RepairType;
                        serviceOrderLineObject.LineProperty = serviceOrderLineList[0].LineProperty;
                        ViewData["ServiceOrderLineinProcess"] = serviceOrderLineList;
                    }
                    
                }
                else
                {
                    serviceOrderLineObject.ServiceOrderLineList = serviceOrderLineList;
                    ViewData["ServiceOrderLineinProcess"] = serviceOrderLineObject;
                }
                TempData.Keep();
            }
            catch (Exception ex)
            {
                TempData.Keep();
                ExceptionLog.LogException(ex, userName);
            }
            return View("PartDetailsView", serviceOrderLineObject);
        }

        private List<ServiceOrder> GetServiceOrderDetailsByServiceOrder(string siteId, string serviceOrder)
        {
            string userName = null;

            ServiceOrder serviceOrderObject = new ServiceOrder();
            List<ServiceOrder> serviceOrderList = new List<ServiceOrder>();
            string process = "-1";
            userName = User.Identity.Name.ToString().Split('\\')[1];
            try
            {
                
                if (!String.IsNullOrEmpty(serviceOrder))
                {
                    serviceOrderList = serviceOrderObject.GetServiceOrders(siteId, process, serviceOrder, userName);
                    if (serviceOrderList.Count > 0)
                    {
                        serviceOrderObject.CustomerPO = serviceOrderList[0].CustomerPO;
                        serviceOrderObject.Customer = new Customer("",serviceOrderList[0].Customer.CustomerName);
                        serviceOrderObject.ServiceTechnician = new ServiceTechnician(serviceOrderList[0].ServiceTechnician.ServiceTechnicianName, serviceOrderList[0].ServiceTechnician.ServiceTechnicianNo);
                        serviceOrderObject.ServiceResponsible = new ServiceTechnician(serviceOrderList[0].ServiceResponsible.ServiceTechnicianName, "");

                        serviceOrderObject.WOBillingAddress = new Address(serviceOrderList[0].WOBillingAddress.AddresswithDesc);
                        serviceOrderObject.WOShippingAddress = new Address(serviceOrderList[0].WOShippingAddress.AddresswithDesc);
                        ViewData["ServiceOrderDetailsinProcess"] = serviceOrderList;
                        ViewData["Technician"] = serviceOrderList[0].ServiceTechnician.ServiceTechnicianNo.ToString();
                        TempData["Technician"] = serviceOrderList[0].ServiceTechnician.ServiceTechnicianNo.ToString();
                    }

                }
                else
                {
                    serviceOrderObject.ServiceOrderList = serviceOrderList;
                    ViewData["ServiceOrderDetailsinProcess"] = serviceOrderObject;
                }
                TempData.Keep();
            }
            catch (Exception ex)
            {
                TempData.Keep();
                ExceptionLog.LogException(ex, userName);
            }
            return serviceOrderList;
        }

        private List<SerivceOrderPartLine> GetServiceOrderPartLinesByServiceOrderID(string serviceOrderId)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            List<SerivceOrderPartLine> serviceOrderPartLine = (new SerivceOrderPartLine()).GetServiceOrderPartLineByServiceOrder(serviceOrderId, userName);
            return serviceOrderPartLine;
        }

        private List<ServiceOrderLine> GetServiceOrderLineBySerialNumber(string serialNumber)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            List<ServiceOrderLine> serviceOrderLine = (new ServiceOrderLine()).GetServiceOrderLinesDetailsBySerialNumber(serialNumber, "", userName);
            return serviceOrderLine;
        }

        [HttpPost]
        public JsonResult _GetTransactionTypes()
        {
            return Json(new SelectList(TransactionType.GetTransactionTypes(), "TransactionTypeID", "TransactionTypeName"), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult _GetDropDownListSpecialtyCode(int? transactionTypeDropDownList)
        {

            return _GetSpecialtyCode(transactionTypeDropDownList.Value);
        }

        private JsonResult _GetSpecialtyCode(int TransactionTypeID)
        {

            List<SpecialtyCode> specialtyCodeList = new List<SpecialtyCode> { };
            SpecialtyCode specialtyCodeObject = new SpecialtyCode();
           
           
            string userName = "";
            userName = User.Identity.Name.ToString().Split('\\')[1];
            string _transactionTypeId = TransactionTypeID.ToString();
            //specialtyCodeList = (new SpecialtyCode()).GetSpecialCodes(userName, _transactionTypeId);
            IEnumerable<SpecialtyCode> specialtyCodeCollection = null;
            specialtyCodeCollection = specialtyCodeObject.GetSpecialCodes(userName, _transactionTypeId);
            specialtyCodeObject.SpecialityCodeList = new SelectList(specialtyCodeCollection, "SpecialityCodeNo", "SpecialityDescription");
            ViewData["SpecialityCodeList"]=specialtyCodeObject.SpecialityCodeList;
            return Json(ViewData["SpecialityCodeList"], JsonRequestBehavior.AllowGet);

        }


        //[HttpGet]
        //public JsonResult _GetDropDownListTechnicianSpecialtyCode(int? transactionTypeDropDownList, string specialtyCodeDropDownList)
        //{

        //    return _GetTechnicianSpecialtyCode(transactionTypeDropDownList.Value, specialtyCodeDropDownList);
        //}

        //private JsonResult _GetTechnicianSpecialtyCode(int TransactionTypeID, string specialCode)
        //{

        //    List<ServiceTechnician> ServiceTechnicianList = new List<ServiceTechnician> { };

        //    string userName = "";
        //    userName = User.Identity.Name.ToString().Split('\\')[1];
        //    string _transactionTypeId = TransactionTypeID.ToString();
        //    ServiceTechnicianList = (new ServiceTechnician()).GetTechniciansServiceOrderProcess(_transactionTypeId, specialCode, userName);
        //    return Json(new SelectList(ServiceTechnicianList, "ServiceTechnicianNo", "ServiceTechnicianName"), JsonRequestBehavior.AllowGet);

        //}


        [HttpGet]
        public JsonResult _GetDropDownWareHouse(string partNumberDropDownList, string siteComboBox)
        {
            return _GetWareHouses(partNumberDropDownList, siteComboBox);

        }


        private JsonResult _GetWareHouses(string itemNumber, string site)
        {
            string userName = User.Identity.Name.ToString().Split('\\')[1];
            WareHouse wareHouseObject = new WareHouse();
            return Json(new SelectList(wareHouseObject.GetWareHouses(itemNumber, site, userName), "WareHouseCode", "WareHouseandQty"), JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public JsonResult _GetLocationList(string partNumberDropDownList, string siteComboBox, string wareHouseDropDownList)
        {
            return _GetLocations(partNumberDropDownList, siteComboBox, wareHouseDropDownList);

        }


        private JsonResult _GetLocations(string itemNumber, string site, string warehouse)
        {
            string userName = User.Identity.Name.ToString().Split('\\')[1];
            Location locationObject = new Location();
            return Json(new SelectList(locationObject.GetLocations(itemNumber, site, warehouse, userName), "LocationId", "LocationandQty"), JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public JsonResult _GetTransactionSerialNumberList(string partNumberDropDownList, string siteComboBox, string wareHouseDropDownList, string locationDropDownList)
        {
            return _GetTransactionSerialNumber(partNumberDropDownList, siteComboBox, wareHouseDropDownList, locationDropDownList);

        }


        private JsonResult _GetTransactionSerialNumber(string itemNumber, string site, string warehouse, string locationid)
        {
            string userName = User.Identity.Name.ToString().Split('\\')[1];
            SerivceOrderPartLine transactionSerialObject = new SerivceOrderPartLine();
            return Json(new SelectList(transactionSerialObject.GetTransactionSerialNumbers(itemNumber, site, warehouse, locationid, userName), "TransactionSerialNumber", "TransactionSerialNumber"), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateServiceOrderPartLines(string serviceOrderNo, string transactionType, string technicinanNo, string quantity, string specialityCode, string failureCode, string lineProperty, string serviceOrderRelation, string description, string serviceComments, string itemNumber, string site, string wareHouse, string transSerialCodeNo, string colorId, string sizeId, string configId, string locationId)
        {
            string userName = null;

            bool isSuccess = false;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            try
            {

               
                SerivceOrderPartLine serviceOrderPartLine = new SerivceOrderPartLine();
                isSuccess = serviceOrderPartLine.CreateServiceOrderItemLines(serviceOrderNo, transactionType, technicinanNo, quantity, specialityCode, failureCode, lineProperty, serviceOrderRelation, description, serviceComments, itemNumber, site, wareHouse, transSerialCodeNo, colorId = "", sizeId = "", configId = "", locationId, userName);

                if (isSuccess)
                {
                    ViewData["ServiceOrderPartLines"] = GetServiceOrderPartLinesByServiceOrderID(serviceOrderNo);
                }
                else
                {
                    throw new Exception("Service Order Part Lines creation failed");
                }
                TempData.Keep();
            }
            catch (Exception ex)
            {
                TempData.Keep();
                if (!isSuccess)
                {
                    ExceptionLog.LogException(ex, userName);
                    throw ex;
                }
            }
            return View("ServiceOrderPartLinesView");
        }

        [HttpPost]
        public ActionResult UpdateServiceOrderPartLines(string uniqueId, string serviceOrderNo, string transactionType, string technicinanNo, string quantity, string specialityCode, string failureCode, string lineProperty, string serviceOrderRelation, string description, string serviceComments, string itemNumber, string site, string wareHouse, string transSerialCodeNo, string colorId, string sizeId, string configId, string locationId)
        {
            string userName = null;

            bool isSuccess = false;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            try
            {

               
               
                SerivceOrderPartLine serviceOrderPartLine = new SerivceOrderPartLine();
                isSuccess = serviceOrderPartLine.UpdateServiceOrderPartLines(uniqueId, serviceOrderNo, transactionType, technicinanNo, quantity, specialityCode, failureCode, lineProperty, serviceOrderRelation, description, serviceComments, itemNumber, site, wareHouse, transSerialCodeNo, colorId = "", sizeId = "", configId = "", locationId, userName);
                if (isSuccess)
                {
                    ViewData["ServiceOrderPartLines"] = GetServiceOrderPartLinesByServiceOrderID(serviceOrderNo);
                }
                else
                {
                    throw new Exception("Service Order Part Lines updation failed");
                }

                TempData.Keep();
            }
            catch (Exception ex)
            {
                TempData.Keep();

                if (!isSuccess)
                {
                    ExceptionLog.LogException(ex, userName);
                    throw ex;
                }
            }
            return View("ServiceOrderPartLinesView");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _DeleteServiceOrderPartLines(string uniqueId, string serviceOrderId)
        {
            string userName = null;

            bool isSuccess = false;
            userName = User.Identity.Name.ToString().Split('\\')[1];

            try
            {
               
                    SerivceOrderPartLine serviceOrderPartLine = new SerivceOrderPartLine();
                    isSuccess = serviceOrderPartLine.DeleteServiceOrderPartItemLines(uniqueId, userName);
                    //TempData["ServiceOrderId"] = Session["SID"].ToString();
                    if (isSuccess)
                    {

                        ViewData["ServiceOrderPartLines"] = GetServiceOrderPartLinesByServiceOrderID(serviceOrderId);

                    }
                
                TempData.Keep();
            }
            catch (Exception ex)
            {
                TempData.Keep();
                throw ex;
            }
            return View(new GridModel<SerivceOrderPartLine>
            {
                Data = ViewData["ServiceOrderPartLines"] as List<SerivceOrderPartLine>
            });


        }

        #endregion

    }
}