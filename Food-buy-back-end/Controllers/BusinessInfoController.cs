using Food_buy_back_end.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Food_buy_back_end.Controllers
{
    public class BusinessInfoController : Controller
    {
        // GET: BusinessInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllBusinessUnits()
        {
            BusinessInfoProvider businessInfoProvider = new BusinessInfoProvider();
            var result = businessInfoProvider.GetAllBusinesUnits();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}