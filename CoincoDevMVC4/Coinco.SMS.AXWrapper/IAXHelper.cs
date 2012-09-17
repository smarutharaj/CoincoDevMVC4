using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Coinco.SMS.AXWrapper
{
    public interface IAXHelper
    {
        DataTable GetDefaultSitesByUsername(string username);
        DataTable GetServiceOrders(string inventSiteId, string orderStatus, string userName);
        DataTable GetServiceOrderLinesByServiceOrderId(string serviceOrderId, string userName);
        DataTable GetServiceOrderLinesBySerialNumberPartNumber(string serialId, string itemNumber, string custAccount, string userName);


        DataTable GetCustomerAddressList(string customerAccount, string userName);
        DataTable GetCustomers(string userName);
        DataTable GetSalesHistory(string salesSerialNumber, string userName);
        DataTable GetSalesInformation(string salesSerialNumber, string userName);
        DataTable GetWOClassificationList(string userName);
        DataTable GetTechnicians(string userName);
        DataTable GetTechniciansServiceOrderProcess(string transactionType, string specialityCode, string userName);
        DataTable GetItemNumbersList(string userName);
        string CreateServiceOrder(string siteId, string customerAccount, string AddressId, string CustomerPO, string ServiceTechnicianNo, string responsibleNo, string woClassification, string customerComments, string userName);
        bool CreateServiceOrderLinesList(string serviceOrderNo, string serialNumber, string partNumber, string partType, string quantity, string repairType, string warranty, string comments, string userName);


         // - Functions for Service Order Process

        DataTable GetFailureCodeList(string userName);
        DataTable GetSerialNumberByServiceOrder(string serviceOrder, string userName);
        DataTable GetLinePropertyList(string userName);
        DataTable GetSpecialityCodeList(string userName, string transactionId);
        DataTable GetServiceOrderPartLineByServiceOrder(string serviceOrderId, string userName);
        DataTable GetServiceOrderLinesDetailsBySerialNumber(string serialId, string itemNumber, string userName);
        DataTable GetSitesList(string userName);
        DataTable GetWareHouses(string itemNumber, string site, string userName);
        DataTable GetLocations(string itemNumber, string site, string wareHouse, string userName);
        DataTable GetTransactionSerialNumberList(string itemNumber, string site, string wareHouse, string locationId, string userName);
        bool CreateServiceOrderItemLines(string serviceOrderNo, string transactionType, string serviceTechnicianCode, string quantity, string specialityCode, string failureCode, string lineProperty, string serviceOrderRelation, string description, string serviceComments, string itemNumber, string site, string wareHouse, string transSerialCodeNo, string colorId, string sizeId, string configId, string locationId, string userName);
        bool UpdateServiceOrderPartLines(string uniqueId, string serviceOrderNo, string transactionType, string serviceTechnicianCode, string quantity, string specialityCode, string failureCode, string serviceType, string serviceOrderRelation, string description, string serviceComments, string itemNumber, string site, string wareHouse, string transSerialCodeNo, string colorId, string sizeId, string configId, string locationId, string userName);
        bool DeleteServiceOrderPartLines(string uniqueId, string userName);

        DataTable GetConditionList(string userName);
        DataTable GetSymptomAreaList(string userName);
        DataTable GetSymptomCodeList(string symptomArea, string userName);
        DataTable GetDiagnosisAreaList(string userName);
        DataTable GetDiagnosisCodeList(string diagnosisArea, string userName);
        DataTable GetResolutionList(string userName);
        DataTable GetRespairStageList(string userName);
        DataTable GetRepairLines(string serviceOrderId, string userName);
        bool CreateRepairLines(string serviceOrderNo, string serviceOrderRelation, string conditionId, string symptomAreaId, string symptomCodeId, string diagonsisAreaId, string diagonsisCodeId, string resolutionId, string repairStageId, string technicianNo, string description, string serviceComments, string userName);
        bool UpdateRepairLines(string uniqueId, string serviceOrderNo, string serviceOrderRelation, string conditionId, string symptomAreaId, string symptomCodeId, string diagonsisAreaId, string diagonsisCodeId, string resolutionId, string repairStageId, string technicianNo, string description, string serviceComments, string userName);
        bool DeleteRepairLines(string uniqueId, string userName);
    }
}
