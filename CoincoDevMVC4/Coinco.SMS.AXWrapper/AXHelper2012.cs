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
        public DataTable GetServiceOrders(string inventSiteId, string orderStatus, string userName)
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
            serviceTable.Columns.Add("EntryDate", typeof(DateTime));
            try
            {
               
                ax = new Axapta();
                ax.LogonAs(userName.Trim(), "", networkCredentials, axCompany, "", "", "");

                axRecord = (AxaptaRecord)ax.CallStaticClassMethod("ServiceOrderManagement", "getSMAServiceOrders", inventSiteId, orderStatus);
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

    }
}