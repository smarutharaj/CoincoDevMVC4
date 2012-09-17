﻿using System;
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
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            List<ServiceOrderLine> serviceOrderLine = (new ServiceOrderLine()).GetServiceOrdersLinesByServiceOrder(serviceOrderId, userName);
            return serviceOrderLine;
        }

        //
        // GET: /ServiceOrderProcess/
        public ActionResult ServiceOrderProcess()
        {
            string userName = "";

            SerivceOrderPartLine serivceOrderPartLineObject = new SerivceOrderPartLine();
            ServiceOrderLine serivceOrderLineObject = new ServiceOrderLine();
           
            List<ServiceOrderLine> serviceOrderLineList = new List<ServiceOrderLine>();
            serivceOrderLineObject.ServiceOrderLineList = serviceOrderLineList;
            serivceOrderPartLineObject.ServiceOrderLine = serivceOrderLineObject;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            FailureCode failureCodeObject = new FailureCode();
            IEnumerable<FailureCode> failureCodeCollection = failureCodeObject.GetFailureCode(User.Identity.Name.ToString().Split('\\')[1]);
            failureCodeObject.FailureCodeList = new SelectList(failureCodeCollection, "FailureCodeNo", "FailureDescription", null);
            ViewData["FailureCodeList"] = failureCodeObject.FailureCodeList;

            PartDetails partDetails = new PartDetails();
            partDetails.PartDetailsList = new SelectList(partDetails.GetItemNumbers(User.Identity.Name.ToString().Split('\\')[1]), "ItemNumber", "ItemNumber", null);
            ViewData["PartNumberList"] = partDetails.PartDetailsList;

            LineProperty LinePropertyObject = new LineProperty();
            IEnumerable<LineProperty> LinePropertyCollection = LinePropertyObject.GetLineProperty(User.Identity.Name.ToString().Split('\\')[1]);
            LinePropertyObject.LinePropertyList = new SelectList(LinePropertyCollection, "LinePropertyCode", "LinePropertyDescription", null);
            ViewData["LinePropertyList"] = LinePropertyObject.LinePropertyList;

            if (Session["SID"] != null)
            {
                
                IEnumerable<SerivceOrderPartLine> serviceOrderPartLineCollection = null;
                serviceOrderPartLineCollection = serivceOrderPartLineObject.GetSerialNumberByServiceOrder(TempData["ServiceOrderId"].ToString(), User.Identity.Name.ToString().Split('\\')[1]);
                serivceOrderPartLineObject.ServiceOrderPartLineList = new SelectList(serviceOrderPartLineCollection, "SerialNumber", "SerialNumber", null);
                
                ViewData["SORelationList"] = serivceOrderPartLineObject.ServiceOrderPartLineList;
                ViewData["WorkSerialNumberList"] = serivceOrderPartLineObject.ServiceOrderPartLineList;
                ViewData["ServiceOrderPartLines"] = GetServiceOrderPartLinesByServiceOrderID(TempData["ServiceOrderId"].ToString());

            }
            //else
            //{
            //    IEnumerable<SerivceOrderPartLine> serviceOrderPartLineCollection = null;
            //    List<SerivceOrderPartLine> emptyList = new List<SerivceOrderPartLine>();
            //    ViewData["SORelationList"] = serviceOrderPartLineCollection;
            //    ViewData["WorkSerialNumberList"] = serviceOrderPartLineCollection;
            //    ViewData["ServiceOrderPartLines"] = emptyList;
            //}


            ServiceTechnician serviceTechnician = new ServiceTechnician();
            serviceTechnician.ServiceTechnicianList = new SelectList(serviceTechnician.GetTechnicians(User.Identity.Name.ToString().Split('\\')[1]), "ServiceTechnicianNo", "ServiceTechnicianName", null);
            ViewData["ServiceTechnicianList"] = serviceTechnician.ServiceTechnicianList;

            Site site = new Site();
            IEnumerable<Site> siteCollection = null;
            siteCollection = site.GetSites(userName);
            site.SiteList = new SelectList(siteCollection, "SiteId", "SiteName", siteCollection.First<Site>().SiteID);
            ViewData["siteList"] = site.SiteList;
            ViewData["TranasactionTypes"] = TransactionType.GetTransactionTypes();

            

            TempData.Keep();
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
            return View();
        }

      
        [HttpGet]
        public ActionResult GetServiceOrderLineBySerialNumberOrderProcess(string serialNumber)
        {
            string userName = null;
            ServiceOrderLine serviceOrderLineObject = new ServiceOrderLine();
            List<ServiceOrderLine> serviceOrderLineList = new List<ServiceOrderLine>();

            try
            {
                userName = User.Identity.Name.ToString().Split('\\')[1];
                serviceOrderLineList = serviceOrderLineObject.GetServiceOrderLinesDetailsBySerialNumber(serialNumber, "",userName);
                serviceOrderLineObject.PartNumber = serviceOrderLineList[0].PartNumber;
                serviceOrderLineObject.Warranty = serviceOrderLineList[0].Warranty;
                serviceOrderLineObject.RepairType = serviceOrderLineList[0].RepairType;
                serviceOrderLineObject.LineProperty = serviceOrderLineList[0].LineProperty;
                ViewData["ServiceOrderLineinProcess"] = serviceOrderLineList;
                TempData.Keep();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View("PartDetailsView", serviceOrderLineObject);
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

        [HttpPost]
        public JsonResult _GetDropDownListSpecialtyCode(int? transactionTypeDropDownList)
        {

            return _GetSpecialtyCode(transactionTypeDropDownList.Value);
        }

        private JsonResult _GetSpecialtyCode(int TransactionTypeID)
        {

            List<SpecialtyCode> specialtyCodeList = new List<SpecialtyCode> { };

            string userName = "";
            userName = User.Identity.Name.ToString().Split('\\')[1];
            string _transactionTypeId = TransactionTypeID.ToString();
            specialtyCodeList = (new SpecialtyCode()).GetSpecialCodes(userName, _transactionTypeId);
            return Json(new SelectList(specialtyCodeList, "SpecialityCodeNo", "SpecialityDescription"), JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult _GetDropDownWareHouse(string partNumberDropDownList, string siteComboBox)
        {
            return _GetWareHouses(partNumberDropDownList, siteComboBox);

        }


        private JsonResult _GetWareHouses(string itemNumber, string site)
        {
            string userName = User.Identity.Name.ToString().Split('\\')[1];
            WareHouse wareHouseObject = new WareHouse();
            return Json(new SelectList(wareHouseObject.GetWareHouses(itemNumber, site, userName), "WareHouseCode", "WareHouseCode"), JsonRequestBehavior.AllowGet);

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
            return Json(new SelectList(locationObject.GetLocations(itemNumber, site, warehouse, userName), "LocationId", "LocationId"), JsonRequestBehavior.AllowGet);

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
            try
            {

                userName = User.Identity.Name.ToString().Split('\\')[1];
                if (serviceOrderNo == null)
                {
                    serviceOrderNo = Session["SID"].ToString();
                }
                SerivceOrderPartLine serviceOrderPartLine = new SerivceOrderPartLine();
                isSuccess = serviceOrderPartLine.CreateServiceOrderItemLines(serviceOrderNo, transactionType, technicinanNo, quantity, specialityCode, failureCode, lineProperty, serviceOrderRelation, description, serviceComments, itemNumber, site, wareHouse, transSerialCodeNo, colorId = "", sizeId = "", configId = "", locationId, userName);
                if (isSuccess)
                {

                }
                ViewData["ServiceOrderPartLines"] = GetServiceOrderPartLinesByServiceOrderID(TempData["ServiceOrderId"].ToString());

                TempData.Keep();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View("ServiceOrderPartLinesView");
        }

        [HttpPost]
        public ActionResult UpdateServiceOrderPartLines(string uniqueId, string serviceOrderNo, string transactionType, string technicinanNo, string quantity, string specialityCode, string failureCode, string lineProperty, string serviceOrderRelation, string description, string serviceComments, string itemNumber, string site, string wareHouse, string transSerialCodeNo, string colorId, string sizeId, string configId, string locationId)
        {
            string userName = null;

            bool isSuccess = false;
            try
            {

                userName = User.Identity.Name.ToString().Split('\\')[1];
                if (serviceOrderNo == null)
                {
                    serviceOrderNo = Session["SID"].ToString();
                }
                SerivceOrderPartLine serviceOrderPartLine = new SerivceOrderPartLine();
                isSuccess = serviceOrderPartLine.UpdateServiceOrderPartLines(uniqueId, serviceOrderNo, transactionType, technicinanNo, quantity, specialityCode, failureCode, lineProperty, serviceOrderRelation, description, serviceComments, itemNumber, site, wareHouse, transSerialCodeNo, colorId = "", sizeId = "", configId = "", locationId, userName);
                if (isSuccess)
                {

                }
                ViewData["ServiceOrderPartLines"] = GetServiceOrderPartLinesByServiceOrderID(TempData["ServiceOrderId"].ToString());

                TempData.Keep();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View("ServiceOrderPartLinesView");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _DeleteServiceOrderPartLines(string uniqueId)
        {
            string userName = null;

            bool isSuccess = false;
            userName = User.Identity.Name.ToString().Split('\\')[1];

            try
            {
                SerivceOrderPartLine serviceOrderPartLine = new SerivceOrderPartLine();
                isSuccess = serviceOrderPartLine.DeleteServiceOrderPartItemLines(uniqueId, userName);
                if (isSuccess)
                {
                    ViewData["ServiceOrderPartLines"] = GetServiceOrderPartLinesByServiceOrderID(TempData["ServiceOrderId"].ToString());
                    TempData.Keep();
                }
               
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return View("ServiceOrderPartLinesView");

        }

    }
}