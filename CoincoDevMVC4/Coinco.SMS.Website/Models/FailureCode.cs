using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coinco.SMS.Website.Models
{
    public class FailureCode
    {
        public string FailureCodeNo { get; set; }
        public string FailureDescription { get; set; }

        public List<FailureCode> FailureCodeList { get; set; } 

        public FailureCode()
        {

        }

        public FailureCode(string failureCodeNo, string failureDescription)
        {
            this.FailureCodeNo = failureCodeNo;
            this.FailureDescription = failureDescription;
        }

    }
}