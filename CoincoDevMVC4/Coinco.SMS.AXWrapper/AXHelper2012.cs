using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.Dynamics.BusinessConnectorNet;
using System.Security.Principal;
using System.Reflection;
using System.Configuration;

namespace Coinco.SMS.AXWrapper
{
    public class AXHelper2012 : IAXHelper
    {
        static string axUserName = ConfigurationManager.AppSettings["AXUserName"].ToString();
        static string axPassword = ConfigurationManager.AppSettings["AXPassword"].ToString();
        string axCompany = ConfigurationManager.AppSettings["AXDefaultCompanyName"].ToString();

        //static string axUserName = "TBWIR";
        //static string axPassword = "Admin@123";
        //string axCompany = "CON";

        private System.Net.NetworkCredential networkCredentials = new System.Net.NetworkCredential(axUserName, axPassword);

        public DataTable GetDefaultSitesByUsername(string username)
        {
         
            object objUser, objInventId, objDefault, objSiteName;
            DataTable siteTable = new DataTable();
            Axapta ax = null;
            //string strCurrUserName = username.Split('\\')[1];
            try
            {
                ax = new Axapta();
             
                siteTable.Columns.Add("User", typeof(String));
                siteTable.Columns.Add("Sites", typeof(String));
                siteTable.Columns.Add("SitesName", typeof(String));
                siteTable.Columns.Add("Default", typeof(String));
                ax.LogonAs(username.Trim(), "", networkCredentials, axCompany, "", "", "");
                AxaptaRecord axRecord;
                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMASitesUser", username);



                axRecord.ExecuteStmt("select * from %1");
                // Loop through the set of retrieved records.

                while (axRecord.Found)
                {
                    objUser = axRecord.get_Field("NetworkAlias");
                    objInventId = axRecord.get_Field("InventSiteId");
                    objDefault = axRecord.get_Field("Default");
                    objSiteName = axRecord.get_Field("Name");
                    DataRow row = siteTable.NewRow();

                    row["User"] = objUser.ToString();
                    row["Sites"] = objInventId.ToString();
                    row["Default"] = objDefault.ToString();
                    row["SitesName"] = objSiteName.ToString();
                    siteTable.Rows.Add(row);

                    axRecord.Next();

                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }


            return siteTable;
        }

        //orderstatus - 0: Inprocess; 1-Posted; 2-Cancelled; -1: All
        public DataTable GetServiceOrders(string inventSiteId, string orderStatus, string serviceOrder, string userName)
        {

            Axapta ax = null;
            AxaptaRecord axRecord;
            DataTable serviceTable = new DataTable();
            serviceTable.Columns.Add("ServiceorderId", typeof(String));
            serviceTable.Columns.Add("CustAccount", typeof(String));
            serviceTable.Columns.Add("CustomerPO", typeof(String));
            serviceTable.Columns.Add("CustomerName", typeof(String));
            serviceTable.Columns.Add("Description", typeof(String));
            serviceTable.Columns.Add("Status", typeof(String));
            serviceTable.Columns.Add("WOClassification", typeof(String));
            serviceTable.Columns.Add("ServiceTechnician", typeof(String));
            serviceTable.Columns.Add("ServiceResponsible", typeof(String));
            serviceTable.Columns.Add("BillingAddress", typeof(String));
            serviceTable.Columns.Add("ShippingAddress", typeof(String));
            serviceTable.Columns.Add("EntryDate", typeof(DateTime));
            try
            {
               
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");

                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMAServiceOrders", inventSiteId, orderStatus, serviceOrder);
                    axRecord.ExecuteStmt("select * from %1 order by %1.DateEntry desc");

                    while (axRecord.Found)
                    {
                        DataRow row = serviceTable.NewRow();
                        row["ServiceorderId"] = axRecord.get_Field("ServiceOrderId");
                        row["CustAccount"] = axRecord.get_Field("CustomerAccount");
                        row["CustomerPO"] = axRecord.get_Field("CustomerPO");
                        row["CustomerName"] = axRecord.get_Field("CustomerName");
                        row["Description"] = axRecord.get_Field("CustomerComments");
                        row["Status"] = axRecord.get_Field("Status");
                        row["WOClassification"] = axRecord.get_Field("WOClassification");
                        row["ServiceTechnician"] = axRecord.get_Field("ServiceTechnician");
                        row["ServiceResponsible"] = axRecord.get_Field("ServiceResponsible");
                        row["BillingAddress"] = axRecord.get_Field("BillingAddress");
                        row["ShippingAddress"] = axRecord.get_Field("ShippingAddress");
                        row["EntryDate"] = axRecord.get_Field("DateEntry");
                        serviceTable.Rows.Add(row);
                        axRecord.Next();
                    }

            }
            catch (Exception e)
            {
               // Take other error action as needed.
               throw e; 
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return serviceTable;
        }

        public DataTable GetServiceOrderLinesByServiceOrderId(string serviceOrderId, string userName)
        {

            Axapta ax = null;
            AxaptaRecord axRecord;
            DataTable serialTable = new DataTable();
            serialTable.Columns.Add("SerialNumber", typeof(String));
            serialTable.Columns.Add("PartNumber", typeof(String));
            serialTable.Columns.Add("PartType", typeof(String));
            serialTable.Columns.Add("Quantity", typeof(String));
            serialTable.Columns.Add("Warranty", typeof(String));
            serialTable.Columns.Add("RepairType", typeof(String));
            serialTable.Columns.Add("Comments", typeof(String));
            try
            {

                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");

                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMASerialNumberDetailsBySO", serviceOrderId);
                axRecord.ExecuteStmt("select * from %1");

                while (axRecord.Found)
                {
                    DataRow row = serialTable.NewRow();
                    row["SerialNumber"] = axRecord.get_Field("SerialNumber");
                    row["PartNumber"] = axRecord.get_Field("ItemID");
                    row["PartType"] = axRecord.get_Field("ItemGroup");
                    row["Quantity"] = axRecord.get_Field("Quantity");
                    row["Warranty"] = axRecord.get_Field("Warranty");
                    row["RepairType"] = axRecord.get_Field("RepairType");
                    row["Comments"] = axRecord.get_Field("Comments");
                    serialTable.Rows.Add(row);
                    axRecord.Next();
                }

            }
            catch (Exception e)
            {
                // Take other error action as needed.
                throw e;
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return serialTable;
        }

        public DataTable GetServiceOrderLinesBySerialNumberPartNumber(string serialId, string itemNumber, string custAccount, string userName)
        {

            Axapta ax = null;
            AxaptaRecord axRecord;
            DataTable serialTable = new DataTable();
            serialTable.Columns.Add("SerialNumber", typeof(String));
            serialTable.Columns.Add("PartNumber", typeof(String));
            serialTable.Columns.Add("PartType", typeof(String));
            serialTable.Columns.Add("Quantity", typeof(String));
            serialTable.Columns.Add("Warranty", typeof(String));
            serialTable.Columns.Add("RepairType", typeof(String));
            serialTable.Columns.Add("CustAccount", typeof(String));
            object[] param = new object[3];
            try
            {

                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                param[0] = serialId;
                param[1] = itemNumber;
                param[2] = custAccount;
                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMASerialNumberDetailsCust", param);
                axRecord.ExecuteStmt("select * from %1");

                while (axRecord.Found)
                {
                    DataRow row = serialTable.NewRow();
                    row["SerialNumber"] = axRecord.get_Field("SerialNumber");
                    row["PartNumber"] = axRecord.get_Field("ItemID");
                    row["PartType"] = axRecord.get_Field("ItemGroup");
                    row["Quantity"] = axRecord.get_Field("Quantity");
                    row["Warranty"] = axRecord.get_Field("Warranty");
                    row["RepairType"] = axRecord.get_Field("RepairType");
                    row["CustAccount"] = axRecord.get_Field("CustAccount");
                    serialTable.Rows.Add(row);
                    axRecord.Next();
                }




            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return serialTable;
        }

        public DataTable GetCustomerAddressList(string customerAccount, string userName)
        {


            Axapta ax = null;
            ax = new Axapta();
            DataTable addressTable = new DataTable();
            addressTable.Columns.Add("AddressID", typeof(String));
            addressTable.Columns.Add("AddressDesc", typeof(String));
            addressTable.Columns.Add("Address", typeof(String));
            addressTable.Columns.Add("IsBilling", typeof(String));
            addressTable.Columns.Add("IsShipping", typeof(String));
            try
            {
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                AxaptaRecord axRecord;
                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMACustomerAddresses", customerAccount);
                axRecord.ExecuteStmt("select * from %1");

                while (axRecord.Found)
                {
                    DataRow row = addressTable.NewRow();
                    row["AddressID"] = axRecord.get_Field("AddressID");
                    row["AddressDesc"] = axRecord.get_Field("AddressDesc");
                    row["Address"] = axRecord.get_Field("Address");
                    row["IsBilling"] = axRecord.get_Field("IsBilling");
                    row["IsShipping"] = axRecord.get_Field("Isshipping");
                    addressTable.Rows.Add(row);
                    axRecord.Next();

                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return addressTable;
        }

        public DataTable GetCustomers(string userName)
        {


            Axapta ax = null;

            ax = new Axapta();
            DataTable customerTable = new DataTable();
            customerTable.Columns.Add("CustomerAccount", typeof(String));
            customerTable.Columns.Add("CustomerName", typeof(String));
            try
            {
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                AxaptaRecord axRecord;
                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMACustomers");
                axRecord.ExecuteStmt("select * from %1");
                while (axRecord.Found)
                {
                    DataRow row = customerTable.NewRow();
                    row["CustomerAccount"] = axRecord.get_Field("CustAccount");
                    row["CustomerName"] = axRecord.get_Field("custName");
                    customerTable.Rows.Add(row);
                    axRecord.Next();

                }

            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return customerTable;
        }

        public DataTable GetWOClassificationList(string userName)
        {
            DataTable resultTable = new DataTable();
            Axapta ax = null;
            AxaptaRecord axRecord;
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                resultTable.Columns.Add("WOCode", typeof(String));
                resultTable.Columns.Add("WODescription", typeof(String));
                using (axRecord = ax.CreateAxaptaRecord("SMAWOClassification"))
                {
                    // Execute the query on the table.
                    axRecord.ExecuteStmt("select Code, Description from %1 where %1.DataAreaID=='" + axCompany + "'");
                    // Loop through the set of retrieved records.
                    while (axRecord.Found)
                    {

                        DataRow row = resultTable.NewRow();
                        row["WOCode"] = axRecord.get_Field("Code");
                        row["WODescription"] = axRecord.get_Field("Description");
                        resultTable.Rows.Add(row);
                        axRecord.Next();

                    }


                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return resultTable;
        }

        public DataTable GetTechnicians(string userName)
        {


            Axapta ax = null;

            ax = new Axapta();
            DataTable techniciansTable = new DataTable();
            techniciansTable.Columns.Add("Name", typeof(String));
            techniciansTable.Columns.Add("Number", typeof(String));
            try
            {
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                AxaptaRecord axRecord;
                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMATechnicians");
                axRecord.ExecuteStmt("select * from %1");


                while (axRecord.Found)
                {
                    DataRow row = techniciansTable.NewRow();
                    row["Name"] = axRecord.get_Field("Name");
                    row["Number"] = axRecord.get_Field("Number");
                    techniciansTable.Rows.Add(row);
                    axRecord.Next();

                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }

            return techniciansTable;
        }

       

        public DataTable GetItemNumbersList(string userName)
        {
            DataTable resultTable = new DataTable();
            Axapta ax = null;
            AxaptaRecord axRecord;
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                resultTable.Columns.Add("ItemNumber", typeof(String));
                resultTable.Columns.Add("ProductName", typeof(String));
                resultTable.Columns.Add("ProductSubType", typeof(String));
                using (axRecord = ax.CreateAxaptaRecord("InventTableExpanded"))
                {
                    // Execute the query on the table.
                    axRecord.ExecuteStmt("select ItemID, ProductName, ProductSubType  from %1 where %1.DataAreaID=='" + axCompany + "'");
                    // Loop through the set of retrieved records.
                    while (axRecord.Found)
                    {

                        DataRow row = resultTable.NewRow();
                        row["ItemNumber"] = axRecord.get_Field("ItemID");
                        row["ProductName"] = axRecord.get_Field("ProductName");
                        row["ProductSubType"] = axRecord.get_Field("ProductSubType");
                        resultTable.Rows.Add(row);
                        axRecord.Next();

                    }


                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return resultTable;
        }

        public string CreateServiceOrder(string siteId, string customerAccount, string AddressId, string CustomerPO, string ServiceTechnicianNo, string responsibleNo, string woClassification, string customerComments, string userName)
        {
           
            Axapta ax = null;
            object[] param = new object[8];
            string serviceOrderId;
            try
            {
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");

                param[0] = siteId;
                param[1] = customerAccount;
                param[2] = AddressId;
                param[3] = CustomerPO;
                param[4] = ServiceTechnicianNo;
                param[5] = responsibleNo;
                param[6] = woClassification;
                param[7] = customerComments;
                serviceOrderId = ax.CallStaticClassMethod("ServiceOrderManagement", "createSMAServiceOrder", param).ToString();

                if (serviceOrderId == "")
                {
                    string parameterString = "";
                    for (int i = 0; i < param.Length; i++)
                    {
                        parameterString += "param[" + i + "]" + param[i].ToString() + "; ";
                    }

                    throw new Exception(String.Format("AX Failure:- Method='{0}' Parameters:Values = {1} - ", "createSMAServiceOrder", parameterString));
                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return serviceOrderId;
        }

        public bool CreateServiceOrderLinesList(string serviceOrderNo, string serialNumber, string partNumber, string partType, string quantity, string repairType, string warranty, string comments, string userName)
        {


            bool isSuccess = false;
            Axapta ax = null;

            object retval;
            try
            {
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                bool flagValue;
                object[] param = new object[4];

                param[0] = serviceOrderNo;
                param[1] = partNumber;
                param[2] = serialNumber;
                param[3] = comments;

                retval = ax.CallStaticClassMethod("ServiceOrderManagement", "createSMAServiceObjectRelation", param).ToString();

                if (bool.TryParse(retval.ToString(), out flagValue))
                {
                    isSuccess = flagValue;
                }

                if (!isSuccess)
                {
                    string parameterString = "";
                    for (int i = 0; i < param.Length; i++)
                    {
                        parameterString += "param[" + i + "]" + param[i].ToString() + "; ";
                    }

                    throw new Exception(String.Format("AX Failure:- Method='{0}' Parameters:Values = {1} - ", "createSMAServiceObjectRelation", parameterString));
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return isSuccess;
        }
        #region "Service Order Process"

        // get Service Object Relation by Service Order for Service Order Process
        public DataTable GetSerialNumberByServiceOrder(string serviceOrder, string userName)
        {

            Axapta ax = null;
            AxaptaRecord axRecord;
            DataTable serialTable = new DataTable();

            serialTable.Columns.Add("SORID", typeof(String));
            serialTable.Columns.Add("SerialNumber", typeof(String));

            try
            {

                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");

                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMASerialNumber", serviceOrder);
                axRecord.ExecuteStmt("select * from %1");

                while (axRecord.Found)
                {
                    DataRow row = serialTable.NewRow();
                    row["SORID"] = axRecord.get_Field("SORID");
                    row["SerialNumber"] = axRecord.get_Field("SerialNumber");
                    serialTable.Rows.Add(row);
                    axRecord.Next();
                }




            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return serialTable;
        }

        // Get Service Order Part lines by service order for service order process

        public DataTable GetServiceOrderPartLineByServiceOrder(string serviceOrderId, string userName)
        {

            Axapta ax = null;
            AxaptaRecord axRecord;
            DataTable serviceOrderLineTable = new DataTable();
            serviceOrderLineTable.Columns.Add("SerialNumber", typeof(String));
            serviceOrderLineTable.Columns.Add("SORelationID", typeof(String));
            serviceOrderLineTable.Columns.Add("TransactionType", typeof(String));
            serviceOrderLineTable.Columns.Add("Description", typeof(String));
            serviceOrderLineTable.Columns.Add("SpecialityCode", typeof(String));
            serviceOrderLineTable.Columns.Add("FailureCode", typeof(String));
            serviceOrderLineTable.Columns.Add("LineProperty", typeof(String));
            serviceOrderLineTable.Columns.Add("Qty", typeof(String));
            serviceOrderLineTable.Columns.Add("SalesPrice", typeof(String));
            serviceOrderLineTable.Columns.Add("TechnicianNo", typeof(String));
            serviceOrderLineTable.Columns.Add("Technician", typeof(String));
            serviceOrderLineTable.Columns.Add("ServiceComments", typeof(String));
            serviceOrderLineTable.Columns.Add("UniqueId", typeof(String));
            serviceOrderLineTable.Columns.Add("ItemNumber", typeof(String));
            serviceOrderLineTable.Columns.Add("Status", typeof(String));
            serviceOrderLineTable.Columns.Add("Site", typeof(String));
            serviceOrderLineTable.Columns.Add("WareHouse", typeof(String));
            serviceOrderLineTable.Columns.Add("Size", typeof(String));
            serviceOrderLineTable.Columns.Add("Color", typeof(String));
            serviceOrderLineTable.Columns.Add("Config", typeof(String));
            serviceOrderLineTable.Columns.Add("LocationId", typeof(String));
            serviceOrderLineTable.Columns.Add("TransSerialNumber", typeof(String));







            try
            {

                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");

                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMASOLineDetails", serviceOrderId);
                axRecord.ExecuteStmt("select * from %1 order by %1.UniqueID desc");

                while (axRecord.Found)
                {
                    DataRow row = serviceOrderLineTable.NewRow();
                    row["SerialNumber"] = axRecord.get_Field("SerialNumber");
                    row["SORelationID"] = axRecord.get_Field("ServiceObjectRelationId");
                    row["TransactionType"] = axRecord.get_Field("TransactionType");
                    row["Description"] = axRecord.get_Field("Description");
                    row["SpecialityCode"] = axRecord.get_Field("ProjCategoryId");
                    row["FailureCode"] = axRecord.get_Field("SMAFailureCode");
                    row["LineProperty"] = axRecord.get_Field("ProjLinePropertyId");
                    row["Qty"] = axRecord.get_Field("Qty");
                    row["SalesPrice"] = axRecord.get_Field("ProjSalesPrice");
                    row["TechnicianNo"] = axRecord.get_Field("Worker");
                    row["Technician"] = axRecord.get_Field("WorkerName");
                    row["ServiceComments"] = axRecord.get_Field("DescriptionService");
                    row["ItemNumber"] = axRecord.get_Field("ItemId");
                    row["Status"] = axRecord.get_Field("ServiceOrderStatus");
                    row["UniqueId"] = axRecord.get_Field("UniqueID");
                    row["Site"] = axRecord.get_Field("InventSiteId");
                    row["WareHouse"] = axRecord.get_Field("InventLocationId");
                    row["Size"] = axRecord.get_Field("InventSizeId");
                    row["Color"] = axRecord.get_Field("InventColorId");
                    row["Config"] = axRecord.get_Field("configId");
                    row["LocationId"] = axRecord.get_Field("WMSLocationid");
                    row["TransSerialNumber"] = axRecord.get_Field("InventSerialId");

                    serviceOrderLineTable.Rows.Add(row);
                    axRecord.Next();
                }

            }

            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return serviceOrderLineTable;
        }

        //- Get the Service Order Line Details by Serial Number or Item Number in Service Order Process

        public DataTable GetServiceOrderLinesDetailsBySerialNumber(string serialId, string itemNumber, string userName)
        {

            Axapta ax = null;
            AxaptaRecord axRecord;
            DataTable serialTable = new DataTable();
            serialTable.Columns.Add("SerialNumber", typeof(String));
            serialTable.Columns.Add("PartNumber", typeof(String));
            serialTable.Columns.Add("PartType", typeof(String));
            serialTable.Columns.Add("Quantity", typeof(String));
            serialTable.Columns.Add("Warranty", typeof(String));
            serialTable.Columns.Add("RepairType", typeof(String));
            serialTable.Columns.Add("LineProperty", typeof(String));
            object[] param = new object[2];
            try
            {

                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                param[0] = serialId;
                param[1] = itemNumber;

                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMASerialNumberDetails", param);
                axRecord.ExecuteStmt("select * from %1");

                while (axRecord.Found)
                {
                    DataRow row = serialTable.NewRow();
                    row["SerialNumber"] = axRecord.get_Field("SerialNumber");
                    row["PartNumber"] = axRecord.get_Field("ItemID");
                    row["PartType"] = axRecord.get_Field("ItemGroup");
                    row["Quantity"] = axRecord.get_Field("Quantity");
                    row["Warranty"] = axRecord.get_Field("Warranty");
                    row["RepairType"] = axRecord.get_Field("RepairType");
                    row["LineProperty"] = axRecord.get_Field("LineProperty");
                    serialTable.Rows.Add(row);
                    axRecord.Next();
                }




            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return serialTable;
        }




        //  Get technicians for service order process

        public DataTable GetTechniciansServiceOrderProcess(string transactionType, string specialityCode, string userName)
        {


            Axapta ax = null;

            ax = new Axapta();
            DataTable techniciansTable = new DataTable();
            techniciansTable.Columns.Add("Name", typeof(String));
            techniciansTable.Columns.Add("Number", typeof(String));
            try
            {
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                AxaptaRecord axRecord;
                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMATechniciansParts", transactionType, specialityCode);
                axRecord.ExecuteStmt("select * from %1");


                while (axRecord.Found)
                {
                    DataRow row = techniciansTable.NewRow();
                    row["Name"] = axRecord.get_Field("Name");
                    row["Number"] = axRecord.get_Field("Number");
                    techniciansTable.Rows.Add(row);
                    axRecord.Next();

                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }

            return techniciansTable;
        }

        // get Get Failure Code for service order process

        public DataTable GetFailureCodeList(string userName)
        {
            DataTable resultTable = new DataTable();
            Axapta ax = null;
            AxaptaRecord axRecord;
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                resultTable.Columns.Add("FailureCode", typeof(String));
                resultTable.Columns.Add("FailureDescription", typeof(String));
                using (axRecord = ax.CreateAxaptaRecord("SMAServiceTask"))
                {
                    // Execute the query on the table.
                    axRecord.ExecuteStmt("select ServiceTaskID, Description from %1 where %1.DataAreaID=='" + axCompany + "'");
                    // Loop through the set of retrieved records.
                    while (axRecord.Found)
                    {

                        DataRow row = resultTable.NewRow();
                        row["FailureCode"] = axRecord.get_Field("ServiceTaskID");
                        row["FailureDescription"] = axRecord.get_Field("Description");
                        resultTable.Rows.Add(row);
                        axRecord.Next();

                    }


                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return resultTable;
        }

        public DataTable GetLinePropertyList(string userName)
        {
            DataTable resultTable = new DataTable();
            Axapta ax = null;
            AxaptaRecord axRecord;
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                resultTable.Columns.Add("LinePropertyCode", typeof(String));
                resultTable.Columns.Add("LinePropertyName", typeof(String));
                using (axRecord = ax.CreateAxaptaRecord("ProjLineProperty"))
                {
                    // Execute the query on the table.
                    axRecord.ExecuteStmt("select LinePropertyID, Name from %1 where %1.DataAreaID=='" + axCompany + "'");
                    // Loop through the set of retrieved records.
                    while (axRecord.Found)
                    {

                        DataRow row = resultTable.NewRow();
                        row["LinePropertyCode"] = axRecord.get_Field("LinePropertyID");
                        row["LinePropertyName"] = axRecord.get_Field("Name");
                        resultTable.Rows.Add(row);
                        axRecord.Next();

                    }


                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return resultTable;
        }


        public DataTable GetSpecialityCodeList(string userName, string transactionId)
        {
            DataTable resultTable = new DataTable();
            Axapta ax = null;
            AxaptaRecord axRecord;
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                resultTable.Columns.Add("SpecialityCode", typeof(String));
                resultTable.Columns.Add("SpecialityDescription", typeof(String));
                using (axRecord = ax.CreateAxaptaRecord("CategoryTable"))
                {
                    // Execute the query on the table.e
                    //axRecord.ExecuteStmt("select CategoryID, CategoryName from %1 where %1.DataAreaID=='" + axCompany + "'");
                    axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMACategoryTable", transactionId);
                    axRecord.ExecuteStmt("select * from %1");


                    // Loop through the set of retrieved records.
                    while (axRecord.Found)
                    {

                        DataRow row = resultTable.NewRow();
                        row["SpecialityCode"] = axRecord.get_Field("CategoryId");
                        row["SpecialityDescription"] = axRecord.get_Field("CategoryName");
                        resultTable.Rows.Add(row);
                        axRecord.Next();

                    }


                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return resultTable;
        }

        public DataTable GetSitesList(string userName)
        {
            DataTable resultTable = new DataTable();
            Axapta ax = null;
            AxaptaRecord axRecord;
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                resultTable.Columns.Add("SiteID", typeof(String));
                resultTable.Columns.Add("SiteName", typeof(String));
                using (axRecord = ax.CreateAxaptaRecord("InventSite"))
                {
                    // Execute the query on the table.
                    axRecord.ExecuteStmt("select SiteID, Name from %1 where %1.DataAreaID=='" + axCompany + "'");
                    // Loop through the set of retrieved records.
                    while (axRecord.Found)
                    {

                        DataRow row = resultTable.NewRow();
                        row["SiteID"] = axRecord.get_Field("SiteID");
                        row["SiteName"] = axRecord.get_Field("Name");
                        resultTable.Rows.Add(row);
                        axRecord.Next();

                    }


                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return resultTable;
        }

        public DataTable GetWareHouses(string itemNumber, string site, string userName)
        {
            DataTable resultTable = new DataTable();
            Axapta ax = null;
            AxaptaRecord axRecord;
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                resultTable.Columns.Add("WareHouseID", typeof(String));
                resultTable.Columns.Add("WareHouseName", typeof(String));
                resultTable.Columns.Add("PhysicalQty", typeof(String));
                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMAWarehouse", site, itemNumber);                    // Loop through the set of retrieved records.
                axRecord.ExecuteStmt("select * from %1");
                while (axRecord.Found)
                {

                    DataRow row = resultTable.NewRow();
                    row["WareHouseID"] = axRecord.get_Field("InventLocationID");
                    row["WareHouseName"] = axRecord.get_Field("Name");
                    row["PhysicalQty"] = axRecord.get_Field("AvaiPhysicalQty");
                    resultTable.Rows.Add(row);
                    axRecord.Next();

                }


            }

            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return resultTable;
        }

        public DataTable GetLocations(string itemNumber, string site, string wareHouse, string userName)
        {
            DataTable resultTable = new DataTable();
            Axapta ax = null;
            AxaptaRecord axRecord;
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                resultTable.Columns.Add("LocationID", typeof(String));
                resultTable.Columns.Add("LocationName", typeof(String));
                resultTable.Columns.Add("PhysicalQty", typeof(String));
                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMAWMSLocationNew", site, wareHouse, itemNumber);                    // Loop through the set of retrieved records.
                axRecord.ExecuteStmt("select * from %1");
                while (axRecord.Found)
                {

                    DataRow row = resultTable.NewRow();
                    row["LocationID"] = axRecord.get_Field("wMSLocationId");
                    //row["LocationName"] = axRecord.get_Field("locationType");
                    row["PhysicalQty"] = axRecord.get_Field("AvaiPhysicalQty");
                    resultTable.Rows.Add(row);
                    axRecord.Next();

                }


            }

            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return resultTable;
        }


        public DataTable GetTransactionSerialNumberList(string itemNumber, string site, string wareHouse, string locationId, string userName)
        {

            Axapta ax = null;
            AxaptaRecord axRecord;
            DataTable serialTable = new DataTable();


            serialTable.Columns.Add("SerialNumber", typeof(String));

            try
            {

                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");

                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMATransactionSerialNumbersLocation", itemNumber, wareHouse, site, locationId);
                axRecord.ExecuteStmt("select * from %1");

                while (axRecord.Found)
                {
                    DataRow row = serialTable.NewRow();
                    row["SerialNumber"] = axRecord.get_Field("SerialNumber");
                    serialTable.Rows.Add(row);
                    axRecord.Next();
                }




            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return serialTable;
        }

        public bool CreateServiceOrderItemLines(string serviceOrderNo, string transactionType, string serviceTechnicianCode, string quantity, string specialityCode, string failureCode, string lineProperty, string serviceOrderRelation, string description, string serviceComments, string itemNumber, string site, string wareHouse, string transSerialCodeNo, string colorId, string sizeId, string configId, string locationId, string userName)
        {

            Axapta ax = null;
            object[] param = new object[19];
            object axObject;
            bool flagValue;
            bool isSuccess = false;
            string salesPrice = "";
            try
            {
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");

                param[0] = serviceOrderNo;
                param[1] = transactionType;
                param[2] = serviceTechnicianCode;
                param[3] = quantity;
                param[4] = salesPrice;
                param[5] = specialityCode;
                param[6] = failureCode;
                param[7] = lineProperty;
                param[8] = serviceOrderRelation;
                param[9] = description;
                param[10] = serviceComments;
                param[11] = itemNumber;
                param[12] = site;
                param[13] = wareHouse;
                param[14] = transSerialCodeNo;
                param[15] = colorId;
                param[16] = sizeId;
                param[17] = configId;
                param[18] = locationId;
                axObject = ax.CallStaticClassMethod("ServiceOrderManagement", "createSMAServiceOrderLine", param).ToString();
                if (bool.TryParse(axObject.ToString(), out flagValue))
                {
                    isSuccess = flagValue;
                }

                //If false, log exception
                if (!isSuccess)
                {
                    string parameterString = "";
                    for (int i = 0; i < param.Length; i++)
                    {
                        parameterString += "param[" + i + "]" + param[i].ToString() + "; ";
                    }

                    throw new Exception(String.Format("AX Failure:- Method='{0}' Parameters:Values = {1} - ", "createSMAServiceOrderLine", parameterString));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return isSuccess;
        }

        public bool UpdateServiceOrderPartLines(string uniqueId, string serviceOrderNo, string transactionType, string serviceTechnicianCode, string quantity, string specialityCode, string failureCode, string serviceType, string serviceOrderRelation, string description, string serviceComments, string itemNumber, string site, string wareHouse, string transSerialCodeNo, string colorId, string sizeId, string configId, string locationId, string userName)
        {

            Axapta ax = null;
            object[] param = new object[20];
            object axObject;
            bool flagValue;
            bool isSuccess = false;
            string salesPrice = "";
            try
            {
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                param[0] = uniqueId;
                param[1] = serviceOrderNo;

                param[2] = transactionType;
                param[3] = serviceTechnicianCode;
                param[4] = quantity;
                param[5] = salesPrice;
                param[6] = specialityCode;
                param[7] = failureCode;
                param[8] = serviceType;
                param[9] = serviceOrderRelation;
                param[10] = description;
                param[11] = serviceComments;
                param[12] = itemNumber;
                param[13] = site;
                param[14] = wareHouse;
                param[15] = transSerialCodeNo;
                param[16] = colorId;
                param[17] = sizeId;
                param[18] = configId;
                param[19] = locationId;

                axObject = ax.CallStaticClassMethod("ServiceOrderManagement", "updateSMAServiceOrderLine", param).ToString();
                if (bool.TryParse(axObject.ToString(), out flagValue))
                {
                    isSuccess = flagValue;
                }
                if (!isSuccess)
                {
                    string parameterString = "";
                    for (int i = 0; i < param.Length; i++)
                    {
                        parameterString += "param[" + i + "]" + param[i].ToString() + "; ";
                    }

                    throw new Exception(String.Format("AX Failure:- Method='{0}' Parameters:Values = {1} - ", "updateSMAServiceOrderLine", parameterString));
                }

            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return isSuccess;
        }

        public bool DeleteServiceOrderPartLines(string uniqueId, string userName)
        {

            Axapta ax = null;
            object[] param = new object[1];
            object axObject;
            bool flagValue;
            bool isSuccess = false;
            try
            {
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                param[0] = uniqueId;
                axObject = ax.CallStaticClassMethod("ServiceOrderManagement", "deleteSMAServiceOrderLine", param).ToString();
                if (bool.TryParse(axObject.ToString(), out flagValue))
                {
                    isSuccess = flagValue;
                }
                if (!isSuccess)
                {
                    string parameterString = "";
                    for (int i = 0; i < param.Length; i++)
                    {
                        parameterString += "param[" + i + "]" + param[i].ToString() + "; ";
                    }

                    throw new Exception(String.Format("AX Failure:- Method='{0}' Parameters:Values = {1} - ", "deleteSMAServiceOrderLine", parameterString));
                }

            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return isSuccess;
        }

        #endregion

        #region "Sales Details"

        public DataTable GetSalesInformation(string salesSerialNumber, string userName)
        {

            Axapta ax = null;
            AxaptaRecord axRecord;
            DataTable salesTable = new DataTable();


            salesTable.Columns.Add("SalesNumber", typeof(String));
            salesTable.Columns.Add("InvoiceNumber", typeof(String));
            salesTable.Columns.Add("InvoiceDate", typeof(String));
            salesTable.Columns.Add("Name", typeof(String));
            salesTable.Columns.Add("ItemNumber", typeof(String));


            try
            {

                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");

                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMASalesInformation", salesSerialNumber);
                axRecord.ExecuteStmt("select * from %1");

                while (axRecord.Found)
                {
                    DataRow row = salesTable.NewRow();

                    row["SalesNumber"] = axRecord.get_Field("OrigSalesId");
                    row["InvoiceNumber"] = axRecord.get_Field("InvoiceId");
                    row["InvoiceDate"] = axRecord.get_Field("InvoiceDate");
                    row["Name"] = axRecord.get_Field("Name");
                    row["ItemNumber"] = axRecord.get_Field("ItemId");




                    salesTable.Rows.Add(row);
                    axRecord.Next();
                }

            }

            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return salesTable;
        }

        public DataTable GetSalesHistory(string salesSerialNumber, string userName)
        {

            Axapta ax = null;
            AxaptaRecord axRecord;
            DataTable salesTable = new DataTable();


            salesTable.Columns.Add("ServiceOrderId", typeof(String));
            salesTable.Columns.Add("SalesPrice", typeof(String));
            salesTable.Columns.Add("DateExecution", typeof(String));
            salesTable.Columns.Add("Description", typeof(String));



            try
            {

                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");

                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMAServiceHistory", salesSerialNumber);
                axRecord.ExecuteStmt("select * from %1");

                while (axRecord.Found)
                {
                    DataRow row = salesTable.NewRow();

                    row["ServiceOrderId"] = axRecord.get_Field("ServiceOrderId");
                    row["SalesPrice"] = axRecord.get_Field("ProjSalesPrice");
                    row["DateExecution"] = axRecord.get_Field("DateExecution");
                    row["Description"] = axRecord.get_Field("DescriptionService");





                    salesTable.Rows.Add(row);
                    axRecord.Next();
                }

            }

            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return salesTable;
        }

        #endregion

        #region "Repair Type"

        public DataTable GetConditionList(string userName)
        {
            DataTable resultTable = new DataTable();
            Axapta ax = null;
            AxaptaRecord axRecord;
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                resultTable.Columns.Add("ConditionId", typeof(String));
                resultTable.Columns.Add("Name", typeof(String));
                using (axRecord = ax.CreateAxaptaRecord("SMAConditionTable"))
                {
                    // Execute the query on the table.
                    axRecord.ExecuteStmt("select ConditionId, Name from %1 where %1.dataAreaID=='" + axCompany + "'");
                    // Loop through the set of retrieved records.
                    while (axRecord.Found)
                    {

                        DataRow row = resultTable.NewRow();
                        row["ConditionId"] = axRecord.get_Field("ConditionId");
                        row["Name"] = axRecord.get_Field("Name");
                        resultTable.Rows.Add(row);
                        axRecord.Next();

                    }


                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return resultTable;
        }


        public DataTable GetSymptomAreaList(string userName)
        {
            DataTable resultTable = new DataTable();
            Axapta ax = null;
            AxaptaRecord axRecord;
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                resultTable.Columns.Add("Name", typeof(String));
                resultTable.Columns.Add("SymptomAreaId", typeof(String));
                using (axRecord = ax.CreateAxaptaRecord("SMASymptomArea"))
                {
                    // Execute the query on the table.
                    axRecord.ExecuteStmt("select Name, SymptomAreaId from %1 where %1.DataAreaID=='" + axCompany + "'");
                    // Loop through the set of retrieved records.
                    while (axRecord.Found)
                    {

                        DataRow row = resultTable.NewRow();
                        row["Name"] = axRecord.get_Field("Name");
                        row["SymptomAreaId"] = axRecord.get_Field("SymptomAreaId");
                        resultTable.Rows.Add(row);
                        axRecord.Next();

                    }


                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return resultTable;
        }



        public DataTable GetSymptomCodeList(string symptomArea, string userName)
        {
            DataTable resultTable = new DataTable();
            Axapta ax = null;
            AxaptaRecord axRecord;
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                resultTable.Columns.Add("Name", typeof(String));
                resultTable.Columns.Add("SMASymptomAreaId", typeof(String));
                resultTable.Columns.Add("SMASymptomCodeId", typeof(String));
                using (axRecord = ax.CreateAxaptaRecord("SMASymptomCode"))
                {
                    // Execute the query on the table.
                    axRecord.ExecuteStmt("select Name, SMASymptomAreaId, SMASymptomCodeId from %1 where %1.DataAreaID=='" + axCompany + "'");
                    // Loop through the set of retrieved records.
                    while (axRecord.Found)
                    {

                        DataRow row = resultTable.NewRow();
                        row["Name"] = axRecord.get_Field("Name");
                        row["SMASymptomAreaId"] = axRecord.get_Field("SMASymptomAreaId");
                        row["SMASymptomCodeId"] = axRecord.get_Field("SMASymptomCodeId");
                        resultTable.Rows.Add(row);
                        axRecord.Next();

                    }


                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return resultTable;
        }


        public DataTable GetDiagnosisAreaList(string userName)
        {
            DataTable resultTable = new DataTable();
            Axapta ax = null;
            AxaptaRecord axRecord;
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                resultTable.Columns.Add("Name", typeof(String));
                resultTable.Columns.Add("DiagnosisAreaId", typeof(String));

                using (axRecord = ax.CreateAxaptaRecord("SMADiagnosisArea"))
                {
                    // Execute the query on the table.
                    axRecord.ExecuteStmt("select DiagnosisAreaId, Name from %1 where %1.DataAreaID=='" + axCompany + "'");
                    // Loop through the set of retrieved records.
                    while (axRecord.Found)
                    {

                        DataRow row = resultTable.NewRow();

                        row["DiagnosisAreaId"] = axRecord.get_Field("DiagnosisAreaId");
                        row["Name"] = axRecord.get_Field("Name");

                        resultTable.Rows.Add(row);
                        axRecord.Next();

                    }


                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return resultTable;
        }

        public DataTable GetDiagnosisCodeList(string diagnosisArea, string userName)
        {
            DataTable resultTable = new DataTable();
            Axapta ax = null;
            AxaptaRecord axRecord;
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                resultTable.Columns.Add("Name", typeof(String));
                resultTable.Columns.Add("DiagnosisAreaId", typeof(String));
                resultTable.Columns.Add("DiagnosisCodeId", typeof(String));
                using (axRecord = ax.CreateAxaptaRecord("SMADiagnosisCode"))
                {
                    // Execute the query on the table.
                    axRecord.ExecuteStmt("select Name, DiagnosisCodeId, DiagnosisAreaId from %1 where %1.DataAreaID=='" + axCompany + "'");
                    // Loop through the set of retrieved records.
                    while (axRecord.Found)
                    {

                        DataRow row = resultTable.NewRow();
                        row["Name"] = axRecord.get_Field("Name");
                        row["DiagnosisCodeId"] = axRecord.get_Field("DiagnosisCodeId");
                        row["DiagnosisAreaId"] = axRecord.get_Field("DiagnosisAreaId");
                        resultTable.Rows.Add(row);
                        axRecord.Next();

                    }


                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return resultTable;
        }


        public DataTable GetResolutionList(string userName)
        {
            DataTable resultTable = new DataTable();
            Axapta ax = null;
            AxaptaRecord axRecord;
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                resultTable.Columns.Add("Name", typeof(String));
                resultTable.Columns.Add("ResolutionId", typeof(String));

                using (axRecord = ax.CreateAxaptaRecord("SMAResolutionTable"))
                {
                    // Execute the query on the table.
                    axRecord.ExecuteStmt("select ResolutionId, Name from %1 where %1.DataAreaID=='" + axCompany + "'");
                    // Loop through the set of retrieved records.
                    while (axRecord.Found)
                    {

                        DataRow row = resultTable.NewRow();

                        row["ResolutionId"] = axRecord.get_Field("ResolutionId");
                        row["Name"] = axRecord.get_Field("Name");

                        resultTable.Rows.Add(row);
                        axRecord.Next();

                    }


                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return resultTable;
        }


        public DataTable GetRespairStageList(string userName)
        {
            DataTable resultTable = new DataTable();
            Axapta ax = null;
            AxaptaRecord axRecord;
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                resultTable.Columns.Add("Name", typeof(String));
                resultTable.Columns.Add("RepairStageId", typeof(String));

                using (axRecord = ax.CreateAxaptaRecord("SMARepairStage"))
                {
                    // Execute the query on the table.
                    axRecord.ExecuteStmt("select RepairStageId, Name from %1 where %1.DataAreaID=='" + axCompany + "'");
                    // Loop through the set of retrieved records.
                    while (axRecord.Found)
                    {

                        DataRow row = resultTable.NewRow();
                        row["Name"] = axRecord.get_Field("Name");
                        row["RepairStageId"] = axRecord.get_Field("RepairStageId");


                        resultTable.Rows.Add(row);
                        axRecord.Next();

                    }


                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return resultTable;
        }

        public DataTable GetRepairLines(string serviceOrderId, string userName)
        {

            Axapta ax = null;
            AxaptaRecord axRecord;
            DataTable repairLineTable = new DataTable();

            repairLineTable.Columns.Add("RepServiceOrder", typeof(String));
            repairLineTable.Columns.Add("UniqueId", typeof(String));
            repairLineTable.Columns.Add("SORelationID", typeof(String));
            repairLineTable.Columns.Add("Description", typeof(String));
            repairLineTable.Columns.Add("ConditionId", typeof(String));
            repairLineTable.Columns.Add("SymptomAreaId", typeof(String));
            repairLineTable.Columns.Add("SymptomCodeId", typeof(String));
            repairLineTable.Columns.Add("DiagnosisAreaId", typeof(String));
            repairLineTable.Columns.Add("DiagnosisCodeId", typeof(String));


            repairLineTable.Columns.Add("ResolutionId", typeof(String));
            repairLineTable.Columns.Add("RepairStageId", typeof(String));
            repairLineTable.Columns.Add("TechnicianNo", typeof(String));
            repairLineTable.Columns.Add("TechnicianName", typeof(String));
            repairLineTable.Columns.Add("ServiceComments", typeof(String));

            try
            {

                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");

                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMARepairLine", serviceOrderId);
                axRecord.ExecuteStmt("select * from %1");

                while (axRecord.Found)
                {
                    DataRow row = repairLineTable.NewRow();
                    row["RepServiceOrder"] = axRecord.get_Field("ServiceOrderId");
                    row["UniqueId"] = axRecord.get_Field("UniqueID");
                    row["SORelationID"] = axRecord.get_Field("ServiceObjectRelationId");

                    row["Description"] = axRecord.get_Field("Description");
                    row["ConditionId"] = axRecord.get_Field("ConditionId");
                    row["SymptomAreaId"] = axRecord.get_Field("SymptomAreaId");
                    row["SymptomCodeId"] = axRecord.get_Field("SymptomCodeId");
                    row["DiagnosisAreaId"] = axRecord.get_Field("DiagnosisAreaId");
                    row["DiagnosisCodeId"] = axRecord.get_Field("DiagnosisCodeId");
                    row["ResolutionId"] = axRecord.get_Field("ResolutionId");
                    row["RepairStageId"] = axRecord.get_Field("RepairStageId");
                    row["TechnicianNo"] = axRecord.get_Field("Worker");
                    row["TechnicianName"] = axRecord.get_Field("WorkerName");
                    row["ServiceComments"] = axRecord.get_Field("Name");

                    repairLineTable.Rows.Add(row);
                    axRecord.Next();
                }

            }

            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return repairLineTable;
        }

        public bool CreateRepairLines(string serviceOrderNo, string serviceOrderRelation, string conditionId, string symptomAreaId, string symptomCodeId, string diagonsisAreaId, string diagonsisCodeId, string resolutionId, string repairStageId, string technicianNo, string description, string serviceComments, string userName)
        {

            Axapta ax = null;
            object[] param = new object[12];
            object axObject;
            bool flagValue;
            bool isSuccess = false;
            try
            {
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");

                param[0] = serviceOrderNo;
                param[1] = serviceOrderRelation;
                param[2] = conditionId;
                param[3] = symptomAreaId;
                param[4] = symptomCodeId;
                param[5] = diagonsisAreaId;
                param[6] = diagonsisCodeId;
                param[7] = resolutionId;
                param[8] = repairStageId;
                param[9] = technicianNo;
                param[10] = description;
                param[11] = serviceComments;

                axObject = ax.CallStaticClassMethod("ServiceOrderManagement", "createSMARepairLine", param).ToString();
                if (bool.TryParse(axObject.ToString(), out flagValue))
                {
                    isSuccess = flagValue;
                }

                if (!isSuccess)
                {
                    string parameterString = "";
                    for (int i = 0; i < param.Length; i++)
                    {
                        parameterString += "param[" + i + "]" + param[i].ToString() + "; ";
                    }

                    throw new Exception(String.Format("AX Failure:- Method='{0}' Parameters:Values = {1} - ", "createSMARepairLine", parameterString));
                }
            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return isSuccess;
        }


        public bool UpdateRepairLines(string uniqueId, string serviceOrderNo, string serviceOrderRelation, string conditionId, string symptomAreaId, string symptomCodeId, string diagonsisAreaId, string diagonsisCodeId, string resolutionId, string repairStageId, string technicianNo, string description, string serviceComments, string userName)
        {

            Axapta ax = null;
            object[] param = new object[13];
            object axObject;
            bool flagValue;
            bool isSuccess = false;
            try
            {
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                param[0] = uniqueId;
                param[1] = serviceOrderNo;
                param[2] = serviceOrderRelation;
                param[3] = conditionId;
                param[4] = symptomAreaId;
                param[5] = symptomCodeId;
                param[6] = diagonsisAreaId;
                param[7] = diagonsisCodeId;
                param[8] = resolutionId;
                param[9] = repairStageId;
                param[10] = technicianNo;
                param[11] = description;
                param[12] = serviceComments;

                axObject = ax.CallStaticClassMethod("ServiceOrderManagement", "updateSMARepairLine", param).ToString();
                if (bool.TryParse(axObject.ToString(), out flagValue))
                {
                    isSuccess = flagValue;
                }
                if (!isSuccess)
                {
                    string parameterString = "";
                    for (int i = 0; i < param.Length; i++)
                    {
                        parameterString += "param[" + i + "]" + param[i].ToString() + "; ";
                    }

                    throw new Exception(String.Format("AX Failure:- Method='{0}' Parameters:Values = {1} - ", "updateSMARepairLine", parameterString));
                }

            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return isSuccess;
        }

        public bool DeleteRepairLines(string uniqueId, string userName)
        {

            Axapta ax = null;
            object[] param = new object[1];
            object axObject;
            bool flagValue;
            bool isSuccess = false;
            try
            {
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");
                param[0] = uniqueId;
                axObject = ax.CallStaticClassMethod("ServiceOrderManagement", "deleteSMARepairLine", param).ToString();
                if (bool.TryParse(axObject.ToString(), out flagValue))
                {
                    isSuccess = flagValue;
                }
                if (!isSuccess)
                {
                    string parameterString = "";
                    for (int i = 0; i < param.Length; i++)
                    {
                        parameterString += "param[" + i + "]" + param[i].ToString() + "; ";
                    }

                    throw new Exception(String.Format("AX Failure:- Method='{0}' Parameters:Values = {1} - ", "deleteSMARepairLine", parameterString));
                }

            }
            catch (Exception e)
            {
                throw e;
                // Take other error action as needed.
            }
            finally
            {
                if (ax != null) ax.Logoff();
            }
            return isSuccess;
        }

      
        #endregion

    }
}