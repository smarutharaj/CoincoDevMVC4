using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coinco.SMS.Models
{
    public class WareHouse
    {
        public string WareHouseCode { get; set; }
        public string WareHouseName { get; set; }
        public string PhyiscalQty { get; set; }

        public List<WareHouse> WareHouseList { get; set; } 

        public WareHouse()
        {

        }

        public WareHouse(string wareHouseCode, string wareHouseName, string phyiscalQty)
        {
            this.WareHouseCode = wareHouseCode;
            this.WareHouseName = wareHouseName;
            this.PhyiscalQty = phyiscalQty;
        }
    }
}