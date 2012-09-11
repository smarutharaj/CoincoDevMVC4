using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coinco.SMS.Website.Models
{
    public class PartDetails
    {
        public string ItemNumber { get; set; }
        public string ProductName { get; set; }
        public string ProductSubType { get; set; }

        public List<PartDetails> PartDetailsList { get; set; } 

        public PartDetails()
        {

        }

        public PartDetails(string itemNumber, string productName, string productSubType)
        {
            this.ItemNumber = itemNumber;
            this.ProductName = productName;
            this.ProductSubType = productSubType;
        }

    }
}