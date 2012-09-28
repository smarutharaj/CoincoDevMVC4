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
    public class Customer
    {
        public string CustomerAccount { get; set; }
        public string CustomerName { get; set; }

        public SelectList CustomerList { get; set; } 

        public Customer()
        {

        }

        public Customer(string customerAccount, string customerName)
        {
            this.CustomerAccount = customerAccount;
            this.CustomerName = customerName;
        }

        //- To get the GetCustomers for Check In Page

        public IEnumerable<Customer> GetCustomers(string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<Customer> customerList = new List<Customer>();
            try
            {
                DataTable resultTable = axHelper.GetCustomers(userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    Customer customerObject = new Customer();
                    customerObject.CustomerAccount = row["CustomerAccount"].ToString();
                    customerObject.CustomerName = row["CustomerAccount"].ToString() + " - " + row["CustomerName"].ToString();

                    customerList.Add(customerObject);

                }
            }
            catch (Exception e)
            {
                throw e;

            }
            return customerList.AsEnumerable<Customer>();

        }
    }
}