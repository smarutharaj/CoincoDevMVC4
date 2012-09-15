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
            TempData["ServiceOrderId"] = TempData["ServiceOrderId"].ToString();
            string userName = User.Identity.Name.ToString().Split('\\')[1];

            SerivceOrderPartLine serviceOrderPartLineObj = new SerivceOrderPartLine();
            IEnumerable<SerivceOrderPartLine> serviceOrderPartLineCollection = null;
            serviceOrderPartLineCollection = serviceOrderPartLineObj.GetSerialNumberByServiceOrder(TempData["ServiceOrderId"].ToString(), userName);
            serviceOrderPartLineObj.ServiceOrderPartLineList = new SelectList(serviceOrderPartLineCollection, "SerialNumber", "SerialNumber", null);
            ViewData["SerialNumberList"] = serviceOrderPartLineObj.ServiceOrderPartLineList;

            RepairType repairTypeObj = new RepairType();

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

            ServiceTechnician serviceTechnician = new ServiceTechnician();
            serviceTechnician.ServiceTechnicianList = new SelectList(serviceTechnician.GetTechnicians(userName), "ServiceTechnicianNo", "ServiceTechnicianName", null);
            ViewData["ServiceTechnicianList"] = serviceTechnician.ServiceTechnicianList;

            //List<RepairType> RepairLineList = (new RepairType()).GetRepairLineDetails(TempData["ServiceOrderId"].ToString(), userName);
            //repairTypeObj.RepairLineList = new SelectList(repairTypeObj.GetRepairLineDetails(TempData["ServiceOrderId"].ToString(), userName));
            //ViewData["RepairLinesList"] = GetRepairLinesDetails(TempData["ServiceOrderId"].ToString());
 
            TempData.Keep();

            return View();
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
            string userName = User.Identity.Name.ToString().Split('\\')[1];
            RepairType repairTypeObj = new RepairType();
            repairTypeObj.PartNum = new SelectList(repairTypeObj.GetServiceOrderLinesDetailsBySerialNumber(serialNumber, "", userName), "PartNumber", "PartNumber");
            ViewData["PartNumber"] = repairTypeObj.PartNum;
            TempData["PartNum"] = repairTypeObj.PartNum.First().Text;
            TempData.Keep();
            return View("PartNumber");
        }

       

        /* Binding Symptom Code...*/
        [HttpPost]
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
        [HttpPost]
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
        public ActionResult CreateRepairLineItems(string serviceOrderRelation, string conditionId, string symptomAreaId, string symptomCodeId, string diagonsisAreaId, string diagonsisCodeId, string resolutionId, string repairStageId, string technicianNo, string description, string serviceComments)
        {
            string userName = null;
            bool isSuccess = false;
            try
            {

                userName = User.Identity.Name.ToString().Split('\\')[1];
                RepairType repairType = new RepairType();
                isSuccess = repairType.CreateRepairLineItems(Session["SID"].ToString(), "", conditionId, symptomAreaId, symptomCodeId, diagonsisAreaId, diagonsisCodeId, resolutionId, repairStageId, technicianNo, description, serviceComments, userName);

                if (isSuccess)
                {
                    TempData["ServiceOrderId"] = Session["SID"].ToString();
                  
                }
                ViewData["RepairLinesList"] = GetRepairLinesDetails(TempData["ServiceOrderId"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
                return View("RepairLineDetails");
        }

        ///* Binding PartList...*/

        //[HttpPost]
        //public JsonResult _GetPartNumberList(string serialNumberList)
        //{
        //    return _GetParNumber(serialNumberList);

        //}

        //private JsonResult _GetParNumber(string SerialNumber)
        //{
        //    string userName = User.Identity.Name.ToString().Split('\\')[1];
        //    RepairType repairTypeObj = new RepairType();
        //    return Json(new SelectList(repairTypeObj.GetServiceOrderLinesDetailsBySerialNumber(SerialNumber, "", userName), "PartNumber", "PartNumber"), JsonRequestBehavior.AllowGet);

        //}



    }
}
