using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coinco.SMS.Website.Models;
using Telerik.Web.Mvc;

namespace Coinco.SMS.Website.Controllers
{
    public class RepairLinesController : Controller
    {
        //
        // GET: /RepairLines/

        public ActionResult RepairLines()
        {
            TempData["ServiceOrderId"] = Session["SID"];
            TempData["RepairSiteId"]=Session["SiteID"];
            RepairType repairTypeObj = new RepairType();
            try
            { 
                string userName = User.Identity.Name.ToString().Split('\\')[1];
                
                ServiceOrderLine serviceOrderLineobj = new ServiceOrderLine();
                repairTypeObj.ServiceOrderLine = serviceOrderLineobj;

                SerivceOrderPartLine serviceOrderPartLineObj = new SerivceOrderPartLine();
                 IEnumerable<SerivceOrderPartLine> serviceOrderPartLineCollection = null;
                serviceOrderPartLineCollection = serviceOrderPartLineObj.GetSerialNumberByServiceOrder(TempData["ServiceOrderId"].ToString(), userName);
                serviceOrderPartLineObj.ServiceOrderPartLineList = new SelectList(serviceOrderPartLineCollection, "SerialNumber", "SerialNumber", null);
                ViewData["SerialNumberList"] = serviceOrderPartLineObj.ServiceOrderPartLineList;



                repairTypeObj.ConditionList = new SelectList(repairTypeObj.GetCondtions(userName), "ConditionId", "ConditionName", null);
                ViewData["Condition"] = repairTypeObj.ConditionList;

                repairTypeObj.SysmptomAreaList = new SelectList(repairTypeObj.GetSymptomArea(userName), "SymptomAreaId", "SymptomAreaName", null);
                ViewData["SymptomArea"] = repairTypeObj.SysmptomAreaList;

                repairTypeObj.DiagnosisAreaList = new SelectList(repairTypeObj.GetDiagnosisArea(userName), "DiagonsisAreaId", "DiagonsisAreaName", null);
                ViewData["DiagnosisArea"] = repairTypeObj.DiagnosisAreaList;

                repairTypeObj.ResolutionList = new SelectList(repairTypeObj.GetResolution(userName), "ResolutionId", "ResolutionName", null);
                ViewData["Resolution"] = repairTypeObj.ResolutionList;

                repairTypeObj.RepairStageList = new SelectList(repairTypeObj.GetRepairStages(userName), "RepairStageId", "RepairStageName", null);
                ViewData["RepairStage"] = repairTypeObj.RepairStageList;

                //ServiceTechnician serviceTechnician = new ServiceTechnician();
                //serviceTechnician.ServiceTechnicianList = new SelectList(serviceTechnician.GetTechnicians(userName), "ServiceTechnicianNo", "ServiceTechnicianName", null);
                //ViewData["ServiceTechnicianList"] = serviceTechnician.ServiceTechnicianList;

                ServiceOrder ServiceOrder = new ServiceOrder();
                ServiceOrder.ServiceOrderList = GetServiceOrderDetailsByServiceOrder(TempData["RepairSiteId"].ToString(), TempData["ServiceOrderId"].ToString());
                repairTypeObj.ServiceOrders = ServiceOrder;
                ViewData["ServiceOrderDetailsinRepairLines"] = repairTypeObj.ServiceOrders.ServiceOrderList;


                ServiceTechnician serviceTechnician = new ServiceTechnician();
                IEnumerable<ServiceTechnician> serviceTechnicianCollection = null;
                serviceTechnicianCollection = serviceTechnician.GetTechnicians(userName);

                if (!String.IsNullOrEmpty(TempData["Technician-No"].ToString()))
                {
                    serviceTechnician.ServiceTechnicianList = new SelectList(serviceTechnicianCollection, "ServiceTechnicianNo", "ServiceTechnicianName", serviceTechnicianCollection.First<ServiceTechnician>().ServiceTechnicianNo = TempData["Technician-No"].ToString());
                }
                else
                {
                    serviceTechnician.ServiceTechnicianList = new SelectList(serviceTechnicianCollection, "ServiceTechnicianNo", "ServiceTechnicianName", null);
                }
                ViewData["ServiceTechnicianList"] = serviceTechnician.ServiceTechnicianList;

                //List<RepairType> RepairLineList = (new RepairType()).GetRepairLineDetails(TempData["ServiceOrderId"].ToString(), userName);
                //repairTypeObj.RepairLineList = new SelectList(repairTypeObj.GetRepairLineDetails(TempData["ServiceOrderId"].ToString(), userName));
                //ViewData["RepairLinesList"] = GetRepairLinesDetails(TempData["ServiceOrderId"].ToString());
 
                TempData.Keep();    
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(repairTypeObj);
        }

        [HttpGet]
        public ActionResult grid_RepairLinesDetails()
        {
            TempData["ServiceOrderId"] = TempData["ServiceOrderId"].ToString();
            ViewData["RepairLinesList"] = GetRepairLinesDetails(TempData["ServiceOrderId"].ToString());
            TempData.Keep();
            return View("RepairLineDetails");
        }
            
        [GridAction]
        public ActionResult _Selection_RepairLines()
        {
            TempData["ServiceOrderId"] = Session["SID"];
            string serviceOrderId = Session["SID"].ToString();
            TempData.Keep();
            return View(new GridModel<RepairType>
            {
                Data = GetRepairLinesDetails(serviceOrderId)
            });
        }


        private List<RepairType> GetRepairLinesDetails(string serviceOrderId)
        {
            string userName = null;
            userName = User.Identity.Name.ToString().Split('\\')[1];
            List<RepairType> RepairLineList = (new RepairType()).GetRepairLineDetails(TempData["ServiceOrderId"].ToString(), userName);
            return RepairLineList;
        }

 

        [HttpGet]
        public ActionResult getPartNumber(string serialNumber)
        {
            string userName = null;
            ServiceOrderLine serviceOrderLineObject = new ServiceOrderLine();
            List<ServiceOrderLine> serviceOrderLineList = new List<ServiceOrderLine>();

            try
            {
                userName = User.Identity.Name.ToString().Split('\\')[1];
                if (!String.IsNullOrEmpty(serialNumber))
                {
                    serviceOrderLineList = serviceOrderLineObject.GetServiceOrderLinesDetailsBySerialNumber(serialNumber, "", userName);
                    if (serviceOrderLineList.Count > 0)
                    {
                        serviceOrderLineObject.PartNumber = serviceOrderLineList[0].PartNumber;
                        ViewData["RepairLineProcess"] = serviceOrderLineList;
                    }

                }
                else
                {
                    serviceOrderLineObject.ServiceOrderLineList = serviceOrderLineList;
                    ViewData["RepairLineProcess"] = serviceOrderLineObject;
                }
                TempData.Keep();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View("PartNumber", serviceOrderLineObject);
        }

        private List<ServiceOrder> GetServiceOrderDetailsByServiceOrder(string siteId, string serviceOrder)
        {
            string userName = null;

            ServiceOrder serviceOrderObject = new ServiceOrder();
            List<ServiceOrder> serviceOrderList = new List<ServiceOrder>();
            string process = "-1";

            try
            {
                userName = User.Identity.Name.ToString().Split('\\')[1];
                if (!String.IsNullOrEmpty(serviceOrder))
                {
                    serviceOrderList = serviceOrderObject.GetServiceOrders(siteId, process, serviceOrder, userName);
                    if (serviceOrderList.Count > 0)
                    {
                        serviceOrderObject.CustomerPO = serviceOrderList[0].CustomerPO;
                        serviceOrderObject.Customer = new Customer("", serviceOrderList[0].Customer.CustomerName);
                        serviceOrderObject.ServiceTechnician = new ServiceTechnician(serviceOrderList[0].ServiceTechnician.ServiceTechnicianName, serviceOrderList[0].ServiceTechnician.ServiceTechnicianNo);
                        serviceOrderObject.ServiceResponsible = new ServiceTechnician(serviceOrderList[0].ServiceResponsible.ServiceTechnicianName, "");

                        serviceOrderObject.WOBillingAddress = new Address(serviceOrderList[0].WOBillingAddress.AddresswithDesc);
                        serviceOrderObject.WOShippingAddress = new Address(serviceOrderList[0].WOShippingAddress.AddresswithDesc);
                        ViewData["ServiceOrderDetailsinRepairLines"] = serviceOrderList;
                        ViewData["Technician-No"] = serviceOrderList[0].ServiceTechnician.ServiceTechnicianNo.ToString();
                        TempData["Technician-No"] = serviceOrderList[0].ServiceTechnician.ServiceTechnicianNo.ToString();
                    }

                }
                else
                {
                    serviceOrderObject.ServiceOrderList = serviceOrderList;
                    ViewData["ServiceOrderDetailsinRepairLines"] = serviceOrderObject;
                }
                TempData.Keep();
            }
            catch (Exception ex)
            {
                TempData.Keep();
                throw ex;
            }
            return serviceOrderList;
        }

        /* Binding Symptom Code...*/
       [HttpGet]
        public JsonResult _GetDropDownSymptomCode(string symptomAreaList) 
        {
            return _GetSymptomCodeval(symptomAreaList); 

        }

        private JsonResult _GetSymptomCodeval(string SymptomAreaId) 
        {
            string userName = User.Identity.Name.ToString().Split('\\')[1];
            RepairType repairTypeObj = new RepairType();
            return Json(new SelectList(repairTypeObj.GetSymptomCode(SymptomAreaId, userName), "SymptomCodeId", "SymptomCodeName"), JsonRequestBehavior.AllowGet); 

        }

        /* Binding Diagnosis Code...*/
        [HttpGet]
        public JsonResult _GetDropDownDiagnosisCode(string diagnosisAreaList)
        {
            return _GetDiagnosisCodeval(diagnosisAreaList);

        }

        private JsonResult _GetDiagnosisCodeval(string DiagonsisAreaId)
        {
            string userName = User.Identity.Name.ToString().Split('\\')[1];
            RepairType repairTypeObj = new RepairType();
            return Json(new SelectList(repairTypeObj.GetDiagnosisCode(DiagonsisAreaId, userName), "DiagonsisCodeId", "DiagonsisCodeName"), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult CreateRepairLineItems(string serviceOrderNo, string serialNumberList, string conditionId, string symptomAreaId, string symptomCodeId, string diagnosisAreaId, string diagnosisCodeId, string resolutionId, string repairStageId, string technicianNo, string description, string serviceComments)
        {
            string userName = null;
            bool isSuccess = false;
            try
            {

                userName = User.Identity.Name.ToString().Split('\\')[1];
                RepairType repairType = new RepairType();
                isSuccess = repairType.CreateRepairLineItems(serviceOrderNo, serialNumberList, conditionId, symptomAreaId, symptomCodeId, diagnosisAreaId, diagnosisCodeId, resolutionId, repairStageId, technicianNo, description, serviceComments, userName);

                if (isSuccess)
                {
                    TempData["ServiceOrderId"] = serviceOrderNo;
                    Session["SID"] = serviceOrderNo;
                  
                }
                ViewData["RepairLinesList"] = GetRepairLinesDetails(TempData["ServiceOrderId"].ToString());
                TempData["RepairLinesList"] = ViewData["RepairLinesList"];
                TempData.Keep();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View("RepairLineDetails");
        }

        [HttpPost]
        public ActionResult UpdateRepairLineItems(string uniqueId, string serviceOrderNo, string serialNumberList, string serviceOrderRelation, string conditionId, string symptomAreaId, string symptomCodeId, string diagnosisAreaId, string diagnosisCodeId, string resolutionId, string repairStageId, string technicianNo, string description, string serviceComments)
        {
            string userName = null;
            bool isSuccess = false;
            try
            {
                userName = User.Identity.Name.ToString().Split('\\')[1];
                RepairType repairType = new RepairType();
                isSuccess = repairType.UpdateRepairLineItems(uniqueId, serviceOrderNo, serialNumberList, conditionId, symptomAreaId, symptomCodeId, diagnosisAreaId, diagnosisCodeId, resolutionId, repairStageId, technicianNo, description, serviceComments, userName);

                if (isSuccess)
                {
                    TempData["ServiceOrderId"] = serviceOrderNo;
                    Session["SID"] = serviceOrderNo;

                }
                ViewData["RepairLinesList"] = GetRepairLinesDetails(TempData["ServiceOrderId"].ToString());
                TempData.Keep();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View("RepairLineDetails");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult DeleteRepairLine(string uniqueID)
        {
            string userName = null;
            bool isSuccess = false;
            try
            {

                userName = User.Identity.Name.ToString().Split('\\')[1];
                RepairType repairType = new RepairType();
                isSuccess = repairType.DeleteRepairLineItems(uniqueID, userName);
                TempData["ServiceOrderId"] = Session["SID"].ToString();
                ViewData["RepairLinesList"] = GetRepairLinesDetails(TempData["ServiceOrderId"].ToString());
                TempData.Keep();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(new GridModel<RepairType>
                {
                    Data = ViewData["RepairLinesList"] as List<RepairType>
                });
        }
    }
}
