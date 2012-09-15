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
    public class WareHouse
    {
        public string WareHouseCode { get; set; }
        public string WareHouseName { get; set; }
        public string PhyiscalQty { get; set; }

        public SelectList WareHouseList { get; set; } 

        public WareHouse()
        {

        }

        public WareHouse(string wareHouseCode, string wareHouseName, string phyiscalQty)
        {
            this.WareHouseCode = wareHouseCode;
            this.WareHouseName = wareHouseName;
            this.PhyiscalQty = phyiscalQty;
        }


        public List<WareHouse> GetWareHouses(string itemNumber, string site, string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<WareHouse> wareHouseList = new List<WareHouse>();

            try
            {
                DataTable resultTable = axHelper.GetWareHouses(itemNumber, site, userName);

                foreach (DataRow row in resultTable.Rows)
                {
                    WareHouse wareHouseObject = new WareHouse();
                    wareHouseObject.WareHouseCode = row["WareHouseID"].ToString();
                    wareHouseObject.WareHouseName = row["WareHouseName"].ToString();
                    wareHouseObject.PhyiscalQty = row["PhysicalQty"].ToString();
                    wareHouseList.Add(wareHouseObject);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return wareHouseList;

        }

    }
}