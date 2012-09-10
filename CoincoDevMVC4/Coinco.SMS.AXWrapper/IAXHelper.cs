using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Coinco.SMS.AXWrapper
{
    public interface IAXHelper
    {
        DataTable GetDefaultSitesByUsername(string username);
        DataTable GetServiceOrders(string inventSiteId, string orderStatus, string userName);
        DataTable GetServiceOrderLinesByServiceOrderId(string serviceOrderId, string userName);
    }
}
