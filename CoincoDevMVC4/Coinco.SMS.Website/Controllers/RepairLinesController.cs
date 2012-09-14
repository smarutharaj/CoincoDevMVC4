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
            String userName = User.Identity.Name.ToString().Split('\\')[1];

            SerivceOrderPartLine serviceOrderPartLineObj = new SerivceOrderPartLine();
            IEnumerable<SerivceOrderPartLine> serviceOrderPartLineCollection = null;
            serviceOrderPartLineCollection = serviceOrderPartLineObj.GetSerialNumberByServiceOrder(TempData["ServiceOrderId"].ToString(), userName);
            serviceOrderPartLineObj.ServiceOrderPartLineList = new SelectList(serviceOrderPartLineCollection, "ServiceObjectRelation", "SerialNumber", null);
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
            List<RepairType> SymptomCodeCollection = new List<RepairType>();
            repairTypeObj.SymptomCodeList = new SelectList(SymptomCodeCollection.AsEnumerable<RepairType>(), "SymptomCodeId", "SymptomCodeName", null);
            TempData.Keep();
            return View(repairTypeObj);
            //return View("RepailLines", repairType);
        }

        [HttpGet]
        public ActionResult GetSymptomCode(string symptomArea)
        {
            RepairType repairTypeObject = new RepairType();
            if (symptomArea == null)
            {
                List<RepairType> SymptomCodeCollection = new List<RepairType>();
                repairTypeObject.SymptomCodeList = new SelectList(SymptomCodeCollection.AsEnumerable<RepairType>(), "SymptomCodeId", "SymptomCodeName", null);
                ViewData["SymptomCode"] = repairTypeObject.SymptomCodeList;
            }
            else
            {
                string userName = null;
                userName = User.Identity.Name.ToString().Split('\\')[1];
                repairTypeObject.SymptomCodeList = new SelectList(repairTypeObject.GetSymptomCode(symptomArea, userName), "SymptomCodeId", "SymptomCodeName", null);
                ViewData["SymptomCode"] = repairTypeObject.SymptomCodeList;
            }
            TempData.Keep();
            return View("SymptomsCode");
        }

    }
}
