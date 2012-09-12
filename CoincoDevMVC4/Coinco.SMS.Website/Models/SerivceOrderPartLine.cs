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


        public List<SerivceOrderPartLine> GetServiceObjectRelationByServiceOrder(string serviceOrderID, string userName)
        {

            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<SerivceOrderPartLine> serialList = new List<SerivceOrderPartLine>();
            string serialNumbers = ""; ;
            try
            {
                DataTable resultTable = axHelper.GetServiceObjectRelationByServiceOrder(serviceOrderID, userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    SerivceOrderPartLine serviceOrderPartObject = new SerivceOrderPartLine();
                    serviceOrderPartObject.ServiceObjectRelation = row["SORID"].ToString();
                    serialNumbers = row["SerialNumber"].ToString();
                    if (serialNumbers != "")
                    {
                        serviceOrderPartObject.SerialNumber = row["SerialNumber"].ToString();
                    }
                    else
                    {
                        serviceOrderPartObject.SerialNumber = "-";

                    }


                    serialList.Add(serviceOrderPartObject);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return serialList;

        }

    }
}