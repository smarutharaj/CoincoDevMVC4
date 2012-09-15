using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using Coinco.SMS.AXWrapper;
using StructureMap;

namespace Coinco.SMS.Website.Models
{
    public class SerivceOrderPartLine
    {
        public string ServiceOrder { get; set; }
        public string ServiceObjectRelation { get; set; }
        public string SerialNumber { get; set; }
        public string ServiceComments { get; set; }
        public string Quantity { get; set; }
        public string Description { get; set; }
        public string TransactionSerialNumber { get; set; }
        public string ColorType { get; set; }
        public string SizeType { get; set; }
        public string ConfigType { get; set; }
        public string UniqueId { get; set; }
        public string SalesPrice { get; set; }
        public string Status { get; set; }

        public TransactionType TransactionType { get; set; }
        public Site Site { get; set; }
        public FailureCode FailureCode { get; set; }
        public PartDetails PartDetails { get; set; }
        public ServiceTechnician ServiceTechnician { get; set; }
        public LineProperty LineProperty { get; set; }
        public Location Location { get; set; }
        public WareHouse WareHouse { get; set; }
        public SpecialtyCode SpecialtyCode { get; set; }
        public ServiceTechnician ServiceResponsible { get; set; }
        public ServiceOrderLine ServiceOrderLine { get; set; }
        public ServiceOrder ServiceOrders { get; set; }
        public Address Address { get; set; }
        public Customer Customer { get; set; }
        public SelectList ServiceOrderPartLineList { get; set; }

        public IEnumerable<SerivceOrderPartLine> GetSerialNumberByServiceOrder(string serviceOrderID, string userName)
        {

            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<SerivceOrderPartLine> soRelationList = new List<SerivceOrderPartLine>();
           
            try
            {
                DataTable resultTable = axHelper.GetSerialNumberByServiceOrder(serviceOrderID, userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    SerivceOrderPartLine serviceOrderPartObject = new SerivceOrderPartLine();
                    serviceOrderPartObject.ServiceObjectRelation = row["SORID"].ToString();
                    serviceOrderPartObject.SerialNumber = row["SerialNumber"].ToString();
                    soRelationList.Add(serviceOrderPartObject);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return soRelationList.AsEnumerable<SerivceOrderPartLine>();

        }

        public List<SerivceOrderPartLine> GetServiceOrderPartLineByServiceOrder(string serviceorderId, string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<SerivceOrderPartLine> serviceOrderList = new List<SerivceOrderPartLine>();
            try
            {
                DataTable resultTable = axHelper.GetServiceOrderPartLineByServiceOrder(serviceorderId, userName);

                List<SerivceOrderPartLine> serviceOrder = new List<SerivceOrderPartLine>();
               
                string status = "";
                string transaction = "";
                foreach (DataRow row in resultTable.Rows)
                {
                    SerivceOrderPartLine serviceObject = new SerivceOrderPartLine();
                    serviceObject.SerialNumber = row["SerialNumber"].ToString();
                    serviceObject.ServiceObjectRelation = row["SORelationID"].ToString();
                    transaction = row["TransactionType"].ToString();
                  
                    if (transaction == "3")
                    {
                        serviceObject.TransactionType = TransactionType.Item;
                    }
                    else if (transaction == "1")
                    {
                        serviceObject.TransactionType = TransactionType.Hour;
                    }
                    else if (transaction == "2")
                    {
                        serviceObject.TransactionType = TransactionType.Expense;
                    }
                    else if (transaction == "4")
                    {
                        serviceObject.TransactionType = TransactionType.Fee;
                    }
                    serviceObject.Description = row["Description"].ToString();

                    serviceObject.SpecialtyCode = new Models.SpecialtyCode(row["SpecialityCode"].ToString(),"");
                    serviceObject.FailureCode = new Models.FailureCode(row["FailureCode"].ToString(), "");
                    serviceObject.Quantity = row["Qty"].ToString();
                    serviceObject.SalesPrice = row["SalesPrice"].ToString();
                    serviceObject.ServiceTechnician = new Models.ServiceTechnician(row["Technician"].ToString(), "");
                    serviceObject.ServiceComments = row["ServiceComments"].ToString();
                    serviceObject.UniqueId = row["UniqueId"].ToString();
                    serviceObject.PartDetails = new Models.PartDetails(row["ItemNumber"].ToString(), "", "");
                    serviceObject.Site = new Models.Site("",row["Site"].ToString(),"","");
                    serviceObject.WareHouse =new Models.WareHouse(row["WareHouse"].ToString(),"","");
                    serviceObject.SizeType = row["Size"].ToString();
                    serviceObject.ColorType = row["Color"].ToString();
                    serviceObject.ConfigType = row["Config"].ToString();
                    serviceObject.Location = new Models.Location(row["LocationId"].ToString(),"","");
                    serviceObject.TransactionSerialNumber = row["TransSerialNumber"].ToString();
                    if (status == "0")
                    {
                        serviceObject.Status = "In Process";
                    }
                    else if (status == "1")
                    {
                        serviceObject.Status = "Posted";
                    }



                    serviceOrderList.Add(serviceObject);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return serviceOrderList;

        }

        public List<SerivceOrderPartLine> GetTransactionSerialNumbers(string itemNumber, string site, string wareHouse, string locationId, string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<SerivceOrderPartLine> serialNumberList = new List<SerivceOrderPartLine>();
            try
            {
                DataTable resultTable = axHelper.GetTransactionSerialNumberList(itemNumber, site, wareHouse, locationId, userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    SerivceOrderPartLine serviceObject = new SerivceOrderPartLine();
                    serviceObject.TransactionSerialNumber = row["SerialNumber"].ToString();
                    serialNumberList.Add(serviceObject);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return serialNumberList;

        }


        public bool CreateServiceOrderItemLines(string serviceOrderNo, string transactionType, string serviceTechnicianCode, string quantity,  string specialityCode, string failureCode, string serviceType, string serviceOrderRelation, string description, string serviceComments, string itemNumber, string site, string wareHouse, string transSerialCodeNo, string colorId, string sizeId, string configId, string locationId, string userName)
        {
            bool isSuccess = false;

            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            try
            {
                isSuccess = axHelper.CreateServiceOrderItemLines(serviceOrderNo, transactionType, serviceTechnicianCode, quantity, specialityCode, failureCode, serviceType, serviceOrderRelation, description, serviceComments, itemNumber, site, wareHouse, transSerialCodeNo, colorId, sizeId, configId, locationId, userName);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return isSuccess;

        }
    }
}