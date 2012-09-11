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
    public class WOClassification
    {
        public string WOClassificationCode { get; set; }
        public string WOClassificationName { get; set; }

        public SelectList WOClassificationList { get; set; } 
        public WOClassification()
        {

        }

        public WOClassification(string woClassificationCode, string woClassificationName)
        {
            this.WOClassificationCode = woClassificationCode;
            this.WOClassificationName = woClassificationName;
        }

        //- To get the GetWOClassification for Check In Page 

        public IEnumerable<WOClassification> GetWOClassification(string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<WOClassification> woClassificationList = new List<WOClassification>();
            try
            {
                DataTable resultTable = axHelper.GetWOClassificationList(userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    WOClassification woObject = new WOClassification();
                    woObject.WOClassificationCode = row["WOCode"].ToString();
                    woObject.WOClassificationName = row["WODescription"].ToString();

                    woClassificationList.Add(woObject);

                }
            }
            catch (Exception e)
            {
                throw e;

            }

            return woClassificationList.AsEnumerable<WOClassification>();

        }
    }
}