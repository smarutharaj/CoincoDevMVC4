using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coinco.SMS.Models
{
    public class Location
    {
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationQty { get; set; }

        public List<Location> LocationList { get; set; } 

        public Location()
        {

        }

        public Location(string locationId, string locationName, string locationQty)
        {
            this.LocationId = locationId;
            this.LocationName = locationName;
            this.LocationQty = locationQty;
        }
    }
}