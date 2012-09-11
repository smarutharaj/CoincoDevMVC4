using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using StructureMap;
using Coinco.SMS.AXWrapper;

namespace Coinco.SMS.Website.Models
{
    public class PartDetails
    {
        public string ItemNumber { get; set; }
        public string ProductName { get; set; }
        public string ProductSubType { get; set; }

        public SelectList PartDetailsList { get; set; } 

        public PartDetails()
        {

        }

        public PartDetails(string itemNumber, string productName, string productSubType)
        {
            this.ItemNumber = itemNumber;
            this.ProductName = productName;
            this.ProductSubType = productSubType;
        }

        public IEnumerable<PartDetails> GetItemNumbers(string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<PartDetails> itemnumberList = new List<PartDetails>();
            try
            {
                DataTable resultTable = axHelper.GetItemNumbersList(userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    PartDetails partObject = new PartDetails();
                    partObject.ItemNumber = row["ItemNumber"].ToString();
                    partObject.ProductName = row["ProductName"].ToString();
                    partObject.ProductSubType = row["ProductSubType"].ToString();
                    itemnumberList.Add(partObject);

                }
            }
            catch (Exception e)
            {
                throw e;

            }
            return itemnumberList.AsEnumerable<PartDetails>();

        }
    }
}