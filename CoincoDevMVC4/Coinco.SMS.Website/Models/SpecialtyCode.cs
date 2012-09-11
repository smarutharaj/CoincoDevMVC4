using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coinco.SMS.Wesite.Models
{
    public class SpecialtyCode
    {
        public string SpecialityCodeNo { get; set; }
        public string SpecialityDescription { get; set; }
        public List<SpecialtyCode> FailureCodeList { get; set; } 

        public SpecialtyCode()
        {

        }

        public SpecialtyCode(string specialityCodeNo, string specialityDescription)
        {
            this.SpecialityCodeNo = specialityCodeNo;
            this.SpecialityDescription = specialityDescription;
        }
    }
}