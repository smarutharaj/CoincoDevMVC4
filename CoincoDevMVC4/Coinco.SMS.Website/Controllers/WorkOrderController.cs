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

            FailureCode failureCodeObject = new FailureCode();
            IEnumerable<FailureCode> failureCodeCollection = failureCodeObject.GetFailureCode(User.Identity.Name.ToString().Split('\\')[1]);
            failureCodeObject.FailureCodeList = new SelectList(failureCodeCollection, "FailureCodeNo", "FailureDescription", null);
            ViewData["FailureCodeList"] = failureCodeObject.FailureCodeList;

            PartDetails partDetails = new PartDetails();
            partDetails.PartDetailsList = new SelectList(partDetails.GetItemNumbers(User.Identity.Name.ToString().Split('\\')[1]), "ItemNumber", "ProductName", null);
            ViewData["PartNumberList"] = partDetails.PartDetailsList;

            LineProperty LinePropertyObject = new LineProperty();
            IEnumerable<LineProperty> LinePropertyCollection = LinePropertyObject.GetLineProperty(User.Identity.Name.ToString().Split('\\')[1]);
            LinePropertyObject.LinePropertyList = new SelectList(LinePropertyCollection, "LinePropertyCode", "LinePropertyDescription", null);
            ViewData["LinePropertyList"] = LinePropertyObject.LinePropertyList;


            SerivceOrderPartLine serivceOrderPartLineObject = new SerivceOrderPartLine();
            IEnumerable<SerivceOrderPartLine> serviceOrderPartLineCollection = null;
            serviceOrderPartLineCollection = serivceOrderPartLineObject.GetSerialNumberByServiceOrder(TempData["ServiceOrderId"].ToString(), User.Identity.Name.ToString().Split('\\')[1]);
            serivceOrderPartLineObject.ServiceOrderPartLineList = new SelectList(serviceOrderPartLineCollection, "ServiceObjectRelation", "SerialNumber", serviceOrderPartLineCollection.First<SerivceOrderPartLine>().ServiceObjectRelation);


            ViewData["SORelationList"] = serivceOrderPartLineObject.ServiceOrderPartLineList;
            ViewData["SerialNumberList"] = serivceOrderPartLineObject.ServiceOrderPartLineList;
            ViewData["ServiceOrderPartLines"] = GetServiceOrderPartLinesByServiceOrderID(TempData["ServiceOrderId"].ToString());


            ServiceTechnician serviceTechnician = new ServiceTechnician();
            serviceTechnician.ServiceTechnicianList = new SelectList(serviceTechnician.GetTechnicians(User.Identity.Name.ToString().Split('\\')[1]), "ServiceTechnicianNo", "ServiceTechnicianName", null);
            ViewData["ServiceTechnicianList"] = serviceTechnician.ServiceTechnicianList;

            Site site = new Site();
            string userName = null;
            IEnumerable<Site> siteCollection = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            siteCollection = site.GetSitesListByUsername(userName);
            site.SiteList = new SelectList(siteCollection, "SiteId", "SiteName", null);

            ViewData["siteList"] = site.SiteList;

            TempData.Keep();
            ViewData["TranasactionTypes"] = TransactionType.GetTransactionTypes();
            return View();
        }

        [GridAction]
        public ActionResult _SelectionClientSide_serviceOrderPartLines(string serviceOrderId)
        {
            Session["SID"] = serviceOrderId;
            TempData["ServiceOrderId"] = serviceOrderId;
            TempData.Keep();
            return View(new GridModel<SerivceOrderPartLine>
            {
                Data = GetServiceOrderPartLinesByServiceOrderID(serviceOrderId)
            });
        }

        [HttpPost]
        public ActionResult GetServiceOrderLineBySerialNumberOrderProcess(string serialNumber)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            ViewData["ServiceOrderLineinProcess"] = GetServiceOrderLineBySerialNumber(serialNumber);
            TempData.Keep();
            return View();
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



    }
}