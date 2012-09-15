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
    public class SpecialtyCode
    {
        public string SpecialityCodeNo { get; set; }
        public string SpecialityDescription { get; set; }
        public SelectList SpecialityCodeList { get; set; } 

        public SpecialtyCode()
        {

        }

        public SpecialtyCode(string specialityCodeNo, string specialityDescription)
        {
            this.SpecialityCodeNo = specialityCodeNo;
            this.SpecialityDescription = specialityDescription;
        }

        public List<SpecialtyCode> GetSpecialCodes(string userName, string TransactionId)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<SpecialtyCode> SpecialtyCodeList = new List<SpecialtyCode>();
            try
            {

                DataTable resultTable = axHelper.GetSpecialityCodeList(userName, TransactionId.ToString());


                foreach (DataRow row in resultTable.Rows)
                {
                    SpecialtyCode SpecialtyCodeObject = new SpecialtyCode();
                    SpecialtyCodeObject.SpecialityCodeNo = row["SpecialityCode"].ToString();
                    SpecialtyCodeObject.SpecialityDescription = row["SpecialityDescription"].ToString();

                    SpecialtyCodeList.Add(SpecialtyCodeObject);

                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return SpecialtyCodeList;

        }

    }
}