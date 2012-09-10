using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coinco.SMS.Website.Controllers
{
    public class CheckInController : Controller
    {
        //
        // GET: /CheckIn/

        public ActionResult CheckIn()
        {
            TempData.Keep();
            return View();
        }

    }
}
