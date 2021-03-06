﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using Coinco.SMS.AXWrapper;
using StructureMap;

namespace Coinco.SMS.Website.Models
{
    public class RepairType
    {
        public ServiceOrderLine ServiceOrderLine { get; set; }

        public string RepServiceOrder { get; set; }

        public string SerialNumber { get; set; }

        public string PartNumber { get; set; }

        public SelectList PartNum { get; set; }

        public string Description { get; set; }

        public string ConditionId { get; set; }
        public string ConditionName { get; set; }

        public SelectList ConditionList { get; set; }

        public string SymptomAreaId { get; set; }
        public string SymptomAreaName { get; set; }

        public SelectList SysmptomAreaList { get; set; }

        public string SymptomCodeId { get; set; }
        public string SymptomCodeName { get; set; }

        public SelectList SymptomCodeList { get; set; }

        public string DiagonsisAreaId { get; set; }
        public string DiagonsisAreaName { get; set; }

        public SelectList DiagnosisAreaList { get; set; }

        public string DiagonsisCodeId { get; set; }
        public string DiagonsisCodeName { get; set; }

        public SelectList DiagnosisCodeList { get; set; }

        public string ResolutionId { get; set; }
        public string ResolutionName { get; set; }

        public SelectList ResolutionList { get; set; }

        public string RepairStageId { get; set; }
        public string RepairStageName { get; set; }

        public SelectList RepairStageList { get; set; }

        public string ServiceTechnicianName { get; set; }
        public string ServiceTechnicianNo { get; set; }

        public ServiceTechnician Technician { get; set; }

        public string Comments { get; set; }
        public ServiceOrder ServiceOrders { get; set; }

        public string UniqueId { get; set; }
        public string ServiceObjectRelation { get; set; }

        //public List<ServiceOrderProcess> ServiceOrderProcess { get; set; }

        //public List<RepairType> GetRepairServiceOrders(string inventid, string processid, string userName)
        //{

        //    IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
        //    List<RepairType> repairList = new List<RepairType>();
        //    try
        //    {
        //        DataTable resultTable = axHelper.GetRepairServiceOrders(inventid, processid, userName);


        //        foreach (DataRow row in resultTable.Rows)
        //        {
        //            RepairType repairObject = new RepairType();
        //            repairObject.RepServiceOrder = row["ServiceorderId"].ToString();
        //            repairList.Add(repairObject);

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;

        //    }
        //    return repairList;

        //}


        public List<RepairType> GetCondtions(string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<RepairType> repairList = new List<RepairType>();
            try
            {
                DataTable resultTable = axHelper.GetConditionList(userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    RepairType repairObject = new RepairType();
                    repairObject.ConditionId = row["ConditionId"].ToString();
                    repairObject.ConditionName = row["Name"].ToString();
                    repairList.Add(repairObject);

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return repairList;

        }

        public List<RepairType> GetSymptomArea(string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<RepairType> repairList = new List<RepairType>();

            try
            {
                DataTable resultTable = axHelper.GetSymptomAreaList(userName);

                foreach (DataRow row in resultTable.Rows)
                {
                    RepairType repairObject = new RepairType();
                    repairObject.SymptomAreaId = row["SymptomAreaId"].ToString();
                    repairObject.SymptomAreaName = row["Name"].ToString();
                    repairList.Add(repairObject);

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return repairList;

        }

        public IQueryable<RepairType> GetSymptomCode(string symptomArea, string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<RepairType> repairList = new List<RepairType>();
            try
            {
                DataTable resultTable = axHelper.GetSymptomCodeList(symptomArea, userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    RepairType repairObject = new RepairType();
                    repairObject.SymptomCodeId = row["SMASymptomCodeId"].ToString();
                    repairObject.SymptomCodeName = row["Name"].ToString();
                    repairObject.SymptomAreaId = row["SMASymptomAreaId"].ToString();
                    repairList.Add(repairObject);

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return repairList.AsQueryable<RepairType>();

        }


        public List<RepairType> GetDiagnosisArea(string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<RepairType> repairList = new List<RepairType>();
            try
            {
                DataTable resultTable = axHelper.GetDiagnosisAreaList(userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    RepairType repairObject = new RepairType();
                    repairObject.DiagonsisAreaId = row["DiagnosisAreaId"].ToString();
                    repairObject.DiagonsisAreaName = row["Name"].ToString();

                    repairList.Add(repairObject);

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return repairList;

        }

        public IQueryable<RepairType> GetDiagnosisCode(string diagnosisArea, string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<RepairType> repairList = new List<RepairType>();
            try
            {
                DataTable resultTable = axHelper.GetDiagnosisCodeList(diagnosisArea, userName);
                
                foreach (DataRow row in resultTable.Rows)
                {
                    RepairType repairObject = new RepairType();


                    repairObject.DiagonsisCodeId = row["DiagnosisCodeId"].ToString();
                    repairObject.DiagonsisCodeName = row["Name"].ToString();
                    repairObject.DiagonsisAreaId = row["DiagnosisAreaId"].ToString();
                    repairList.Add(repairObject);

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return repairList.AsQueryable<RepairType>();

        }


        public List<RepairType> GetResolution(string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<RepairType> repairList = new List<RepairType>();
            try
            {
                DataTable resultTable = axHelper.GetResolutionList(userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    RepairType repairObject = new RepairType();
                    repairObject.ResolutionId = row["ResolutionId"].ToString();
                    repairObject.ResolutionName = row["Name"].ToString();

                    repairList.Add(repairObject);

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return repairList;

        }

        public List<RepairType> GetRepairStages(string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<RepairType> repairList = new List<RepairType>();
            try
            {
                DataTable resultTable = axHelper.GetRespairStageList(userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    RepairType repairObject = new RepairType();

                    repairObject.RepairStageId = row["RepairStageId"].ToString();
                    repairObject.RepairStageName = row["Name"].ToString();
                    repairList.Add(repairObject);

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return repairList;

        }


        public List<RepairType> GetRepairLineDetails(string serviceorderId, string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<RepairType> repairList = new List<RepairType>();
            try
            {
                DataTable resultTable = axHelper.GetRepairLines(serviceorderId, userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    RepairType repairObject = new RepairType();
                    repairObject.RepServiceOrder = row["RepServiceOrder"].ToString();
                    repairObject.UniqueId = row["UniqueId"].ToString();
                    repairObject.ServiceObjectRelation = row["SORelationID"].ToString();
                    repairObject.Description = row["Description"].ToString();
                    repairObject.ConditionId = row["ConditionId"].ToString();
                    repairObject.SymptomAreaId = row["SymptomAreaId"].ToString();
                    repairObject.SymptomCodeId = row["SymptomCodeId"].ToString();
                    repairObject.DiagonsisAreaId = row["DiagnosisAreaId"].ToString();
                    repairObject.DiagonsisCodeId = row["DiagnosisCodeId"].ToString();
                    repairObject.ResolutionId = row["ResolutionId"].ToString();
                    repairObject.RepairStageId = row["RepairStageId"].ToString();
                    repairObject.ServiceTechnicianNo = row["TechnicianNo"].ToString();
                    repairObject.ServiceTechnicianName = row["TechnicianName"].ToString();
                    //repairObject.Technician = new Models.ServiceTechnician(row["TechnicianName"].ToString(), row["TechnicianNo"].ToString());
                    repairObject.Comments = row["ServiceComments"].ToString();


                    repairList.Add(repairObject);

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return repairList;

        }



        public bool CreateRepairLineItems(string serviceOrderNo, string serviceOrderRelation, string conditionId, string symptomAreaId, string symptomCodeId, string diagonsisAreaId, string diagonsisCodeId, string resolutionId, string repairStageId, string technicianNo, string description, string serviceComments, string userName)
        {
            bool isSuccess = false;

            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            try
            {
                isSuccess = axHelper.CreateRepairLines(serviceOrderNo, serviceOrderRelation, conditionId, symptomAreaId, symptomCodeId, diagonsisAreaId, diagonsisCodeId, resolutionId, repairStageId, technicianNo, description, serviceComments, userName);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return isSuccess;

        }


        public bool UpdateRepairLineItems(string uniqueId, string serviceOrderNo, string serviceOrderRelation, string conditionId, string symptomAreaId, string symptomCodeId, string diagonsisAreaId, string diagonsisCodeId, string resolutionId, string repairStageId, string technicianNo, string description, string serviceComments, string userName)
        {
            bool isSuccess = false;

            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            try
            {
                isSuccess = axHelper.UpdateRepairLines(uniqueId, serviceOrderNo, serviceOrderRelation, conditionId, symptomAreaId, symptomCodeId, diagonsisAreaId, diagonsisCodeId, resolutionId, repairStageId, technicianNo, description, serviceComments, userName);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return isSuccess;

        }

        public bool DeleteRepairLineItems(string uniqueId, string userName)
        {
            bool isSuccess = false;

            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            try
            {
                isSuccess = axHelper.DeleteRepairLines(uniqueId, userName);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return isSuccess;

        }

    }

}