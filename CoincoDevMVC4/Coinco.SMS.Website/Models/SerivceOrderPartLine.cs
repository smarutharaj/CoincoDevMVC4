using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coinco.SMS.Models
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
        

       
    }
}