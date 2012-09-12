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
    public class FailureCode
    {
        public string FailureCodeNo { get; set; }
        public string FailureDescription { get; set; }

        public SelectList FailureCodeList { get; set; } 

        public FailureCode()
        {

        }

        public FailureCode(string failureCodeNo, string failureDescription)
        {
            this.FailureCodeNo = failureCodeNo;
            this.FailureDescription = failureDescription;
        }


        public IEnumerable<FailureCode> GetFailureCode(string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<FailureCode> failureCodeList = new List<FailureCode>();
            try
            {
                DataTable resultTable = axHelper.GetFailureCodeList(userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    FailureCode failureCodeObject = new FailureCode();
                    failureCodeObject.FailureCodeNo = row["FailureCode"].ToString();
                    failureCodeObject.FailureDescription = row["FailureDescription"].ToString();

                    failureCodeList.Add(failureCodeObject);

                }
            }
            catch (Exception e)
            {
                throw e;

            }
            return failureCodeList.AsEnumerable<FailureCode>();

        }
    }
}