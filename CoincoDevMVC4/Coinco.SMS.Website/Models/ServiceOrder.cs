using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Coinco.SMS.AXWrapper;
using StructureMap;
namespace Coinco.SMS.Website.Models
{
    public class ServiceOrder
    {
        public string ServiceOrderId { get; set; }
        public string SiteId { get; set; }
        public string Description { get; set; }
        public DateTime ServiceOrderDate { get; set; }
        public string Status { get; set; }
        public string CustomerPO { get; set; }
        public string Comments { get; set; }
        public Address WOBillingAddress { get; set; }
        public Address WOShippingAddress { get; set; }
      
        public Customer Customer { get; set; }
        public PartDetails PartDetails { get; set; }
        public ServiceTechnician ServiceTechnician { get; set; }
        public ServiceTechnician ServiceResponsible { get; set; }
        public WOClassification WOClassification { get; set; }
        public List<Address> AddressList { get; set; }
        public List<Address> BillingAddressList { get; set; }
        public List<Address> ShippingAddressList { get; set; } 
        public ServiceOrder()
        {

        }


        //- To get Service orders  in Service Order with History

        public List<ServiceOrder> GetServiceOrders(string inventId, string progressId, string serviceorderId, string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<ServiceOrder> serviceOrderList = new List<ServiceOrder>();
            try
            {
                DataTable resultTable = axHelper.GetServiceOrders(inventId, progressId, userName);

                string status = "";
                foreach (DataRow row in resultTable.Rows)
                {
                    ServiceOrder serviceObject = new ServiceOrder();
                    serviceObject.ServiceOrderId = row["ServiceorderId"].ToString();
                    serviceObject.Customer = new Models.Customer( row["CustAccount"].ToString(),  row["CustomerName"].ToString() );
                    serviceObject.CustomerPO = row["CustomerPO"].ToString();
                    serviceObject.Description = row["Description"].ToString();
                    status = row["Status"].ToString();
                    serviceObject.WOClassification = new Models.WOClassification("",row["WOClassification"].ToString());
                    serviceObject.ServiceTechnician = new Models.ServiceTechnician(row["ServiceTechnician"].ToString(),"");
                    serviceObject.ServiceOrderDate = Convert.ToDateTime(row["EntryDate"].ToString());
                    serviceObject.WOBillingAddress = new Models.Address(row["BillingAddress"].ToString());
                    serviceObject.WOShippingAddress = new Models.Address(row["ShippingAddress"].ToString());
                    
                    if (status == "0")
                    {
                        serviceObject.Status = "In Process";
                    }
                    else if (status == "1")
                    {
                        serviceObject.Status = "Posted";
                    }
                    else if (status == "2")
                    {
                        serviceObject.Status = "Canceled";
                    }
                    serviceOrderList.Add(serviceObject);

                }
            }
            catch (Exception e)
            {
                throw e;

            }
            return serviceOrderList;

        }




        public bool CreateServiceOrder(string sitesId, string customerAccount, string addressId, string customerPo, string technicinanNo, string responsibleNo, string woClassification, string customerComments, out string newSerivceOrder, string userName)
        {
            bool isSuccess = false;
            object newSerivceOrderobject;
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            try
            {
                newSerivceOrderobject = axHelper.CreateServiceOrder(sitesId, customerAccount, addressId, customerPo, technicinanNo, responsibleNo, woClassification, customerComments, userName);
                newSerivceOrder = (string)newSerivceOrderobject;
                isSuccess = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return isSuccess;

        }

    
        
    }
}