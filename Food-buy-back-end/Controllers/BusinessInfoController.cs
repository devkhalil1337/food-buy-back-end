using Food_buy_back_end.Models;
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

        /*public ActionResult GetAllBusinessUnits()
        {
            BusinessInfoProvider businessInfoProvider = new BusinessInfoProvider();
            var result = businessInfoProvider.GetAllBusinesUnits();
            return Json(result, JsonRequestBehavior.AllowGet);
        }*/

        public ActionResult AddNewBusinessUnit(BusinessInfo businessInfo)
        {
            BusinessInfoProvider businessInfoProvider = new BusinessInfoProvider();
            var result = businessInfoProvider.AddNewBusinessUnit(businessInfo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateBusinessUnit(BusinessInfo businessInfo)
        {
            BusinessInfoProvider businessInfoProvider = new BusinessInfoProvider();
            var result = businessInfoProvider.UpdateBusinessUnit(businessInfo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBusinessUnitById(int BusinessId)
        {
            BusinessInfoProvider businessInfoProvider = new BusinessInfoProvider();
            var result = businessInfoProvider.GetBusinessUnitById(BusinessId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteBusinessUnit(long BusinessId)
        {
            BusinessInfoProvider businessInfoProvider = new BusinessInfoProvider();
            var result = businessInfoProvider.DeleteBusinessUnit(BusinessId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}