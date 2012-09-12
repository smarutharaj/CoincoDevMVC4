using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Coinco.SMS.AXWrapper;
using StructureMap;

namespace Coinco.SMS.Website.Models
{
    public class ServiceOrderLine
    {
        public string SerialNumber { get; set; }
        public string PartNumber { get; set; }
        public string PartType { get; set; }
        public string Quantity { get; set; }
        public string RepairType { get; set; }
        public string Warranty { get; set; }
        public string Comments { get; set; }
        public string LineProperty { get; set; }
        public string CustAccount { get; set; }
        public ServiceOrderLine()
        {

        }

        //- To get serial numbers by service order
        public List<ServiceOrderLine> GetServiceOrdersLinesByServiceOrder(string serviceOrder, string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<ServiceOrderLine> serviceOrderLineItemList = new List<ServiceOrderLine>();
            try
            {
                DataTable resultTable = axHelper.GetServiceOrderLinesByServiceOrderId(serviceOrder, userName); 


                foreach (DataRow row in resultTable.Rows)
                {
                    ServiceOrderLine serviceItemObject = new ServiceOrderLine();
                    serviceItemObject.SerialNumber = row["SerialNumber"].ToString();
                    serviceItemObject.PartNumber = row["PartNumber"].ToString();
                    serviceItemObject.PartType = row["PartType"].ToString();
                    serviceItemObject.Quantity = row["Quantity"].ToString();
                    serviceItemObject.Warranty = row["Warranty"].ToString();
                    serviceItemObject.RepairType = row["RepairType"].ToString();
                    serviceItemObject.Comments = row["Comments"].ToString();

                    serviceOrderLineItemList.Add(serviceItemObject);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return serviceOrderLineItemList;

        }

        public List<ServiceOrderLine> GetGetServiceOrderLinesBySerialNumberPartNumber(string serialId, string itemNumber, string custAccount, string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<ServiceOrderLine> serviceOrderList = new List<ServiceOrderLine>();
            try
            {
                DataTable resultTable = axHelper.GetGetServiceOrderLinesBySerialNumberPartNumber(serialId, itemNumber, custAccount, userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    ServiceOrderLine serviceObject = new ServiceOrderLine();
                    serviceObject.SerialNumber = row["SerialNumber"].ToString();
                    serviceObject.PartNumber = row["PartNumber"].ToString();
                    serviceObject.PartType = row["PartType"].ToString();
                    serviceObject.Quantity = row["Quantity"].ToString();
                    serviceObject.Warranty = row["Warranty"].ToString();
                    serviceObject.RepairType = row["RepairType"].ToString();
                    serviceObject.CustAccount = row["CustAccount"].ToString();
                    serviceOrderList.Add(serviceObject);

                }
            }
            catch (Exception e)
            {
                throw e;

            }
            return serviceOrderList;

        }

       
}
    
}