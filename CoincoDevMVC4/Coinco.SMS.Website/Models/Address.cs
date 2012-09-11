using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace Coinco.SMS.Models
{
    public class Address
    {
        public string AddressId { get; set; }
        public string AddressDescription { get; set; }
        public string AddressDetails { get; set; }
        public string AddresswithDesc { get; set; }
        public string IsBilling { get; set; }
        public string IsShipping { get; set; }

        public List<Address> AddressList { get; set; }

        public Address()
        {

        }


        //- To get the GetCustomersAddress for Check In Page and Service Order Process

        public List<Address> GetCustomerAddress(string customerAccount, string userName)
        {
            Coinco.SMS.AXWrapper.AXHelper axHelper = new AXWrapper.AXHelper();
            List<Address> addressList = new List<Address>();
            try
            {
                DataTable resultTable = axHelper.GetCustomerAddressList(customerAccount, userName);

                foreach (DataRow row in resultTable.Rows)
                {
                    Address addressObject = new Address();

                    addressObject.AddressId = row["AddressId"].ToString();
                    addressObject.AddressDescription = row["AddressDesc"].ToString();
                    addressObject.AddressDetails = row["Address"].ToString();
                    addressObject.AddresswithDesc = addressObject.AddressDescription + " " + addressObject.AddressDetails;
                    addressObject.IsBilling = row["IsBilling"].ToString();
                    addressObject.IsShipping = row["IsShipping"].ToString();
                    addressList.Add(addressObject);
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return addressList;
        }

    }
}