using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coinco.SMS.Website.Models;
using Telerik.Web.Mvc;

namespace Coinco.SMS.Website.Controllers
{
    public class RepairLinesController : Controller
    {
        //
        // GET: /RepairLines/

        public ActionResult RepairLines()
        {

            RepairType repairType = new RepairType();
            repairType.ConditionList= repairType.GetCondtions("");

            TempData.Keep();
            return View("RepailLines", repairType);
        }

    }
}
