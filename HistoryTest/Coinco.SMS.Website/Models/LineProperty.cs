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
    public class LineProperty
    {
        public string LinePropertyCode { get; set; }
        public string LinePropertyDescription { get; set; }

        public SelectList LinePropertyList { get; set; } 

        public LineProperty()
        {

        }

        public LineProperty(string linePropertyCode, string linePropertyDescription)
        {
            this.LinePropertyCode = linePropertyCode;
            this.LinePropertyDescription = linePropertyDescription;
        }


        public IEnumerable<LineProperty> GetLineProperty(string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<LineProperty> LinePropertyList = new List<LineProperty>();
            try
            {
                DataTable resultTable = axHelper.GetLinePropertyList(userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    LineProperty LinePropertyObject = new LineProperty();
                    LinePropertyObject.LinePropertyCode = row["LinePropertyCode"].ToString();
                    LinePropertyObject.LinePropertyDescription =  row["LinePropertyName"].ToString();

                    LinePropertyList.Add(LinePropertyObject);

                }
            }
            catch (Exception e)
            {
                throw e;

            }
            return LinePropertyList.AsEnumerable<LineProperty>();

        }
    }
}