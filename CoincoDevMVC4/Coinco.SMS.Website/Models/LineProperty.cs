using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coinco.SMS.Website.Models
{
    public class LineProperty
    {
        public string LinePropertyCode { get; set; }
        public string LinePropertyDescription { get; set; }

        public List<LineProperty> LinePropertyList { get; set; } 

        public LineProperty()
        {

        }

        public LineProperty(string linePropertyCode, string linePropertyDescription)
        {
            this.LinePropertyCode = linePropertyCode;
            this.LinePropertyDescription = linePropertyDescription;
        }
    }
}