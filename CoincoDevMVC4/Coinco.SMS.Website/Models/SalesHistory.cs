using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Coinco.SMS.AXWrapper;
using StructureMap;
using System.Text;

namespace Coinco.SMS.Website.Models
{
    public class SalesHistory
    {
        public string SalesServiceOrder { get; set; }
        public string SalesSerialNumber { get; set; }
        public string Description { get; set; }
        public string ItemNumber { get; set; }
        public string CustomerName { get; set; }
        public string SalesOrderNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string SalesPrice { get; set; }
        public string DateExecution { get; set; }
        public string InvoiceNumber { get; set; }
      
        public List<SalesHistory> ServiceInfoList { get; set; }

        public SalesHistory()
        {

        }

        //- To get the GetSalesDetails for Sales History

        public List<SalesHistory> GetSalesDetails(string serialNumber, string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<SalesHistory> salesList = new List<SalesHistory>();
            try
            {
                DataTable resultTable = axHelper.GetSalesInformation(serialNumber, userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    SalesHistory salesObject = new SalesHistory();
                    salesObject.SalesOrderNumber = row["SalesNumber"].ToString();
                    salesObject.InvoiceNumber = row["InvoiceNumber"].ToString();
                    salesObject.InvoiceDate = row["InvoiceDate"].ToString();
                    salesObject.CustomerName = row["Name"].ToString();
                    salesObject.ItemNumber = row["ItemNumber"].ToString();
                    salesObject.SalesSerialNumber = serialNumber;


                    salesList.Add(salesObject);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return salesList;

        }


      //  - To get the GetServiceDetails for Sales History, Check In Page and Service Order with History Page

        public List<SalesHistory> GetServiceDetails(string serialNumber, string userName)
        {

            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<SalesHistory> salesList = new List<SalesHistory>();
            string salesprice;
            try
            {
                DataTable resultTable = axHelper.GetSalesHistory(serialNumber, userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    SalesHistory salesObject = new SalesHistory();
                    salesObject.SalesServiceOrder = row["ServiceOrderId"].ToString();
                    salesprice=row["SalesPrice"].ToString();
                    salesObject.SalesPrice = string.Format("{0:c}", salesprice);

                    salesObject.DateExecution = row["DateExecution"].ToString();
                    salesObject.Description = row["Description"].ToString();
                    salesObject.CustomerName = row["CustomerName"].ToString();


                    salesList.Add(salesObject);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return salesList;

        }

    }
}