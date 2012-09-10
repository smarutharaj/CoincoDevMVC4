using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coinco.SMS.Website.Controllers
{
    public class RepairLinesController : Controller
    {
        //
        // GET: /RepairLines/

        public ActionResult RepairLines()
        {
            TempData.Keep();
            return View();
        }

    }
}
