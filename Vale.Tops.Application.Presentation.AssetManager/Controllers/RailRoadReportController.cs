using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vale.Tops.Domain;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Class;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Interface;
using Vale.Tops.Application.Presentation.AssetManager.Models;

namespace Vale.Tops.Application.Presentation.AssetManager.Controllers
{
    public class RailRoadReportController : Controller
    {
        private IvwRailRoadReportReadOnly vwRailRoadReportReadOnly;
        public RailRoadReportController()
        {
            this.vwRailRoadReportReadOnly = new vwRailRoadReportReadOnly();
        }

        // GET: Typer
        [AllowAnonymous]
        public ActionResult Index()
        {
            var ieList = this.vwRailRoadReportReadOnly.All();
            RailRoadReportModel model = new RailRoadReportModel();
            model.ReportLines = ieList.ToList();
            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }
        [AllowAnonymous]
        public ActionResult Tables()
        {
            var ieList = this.vwRailRoadReportReadOnly.All();
            RailRoadReportModel model = new RailRoadReportModel();
            model.ReportLines = ieList.ToList();
            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return PartialView(model);
        }

        public JsonResult GetData()
        {
            var lst = this.vwRailRoadReportReadOnly.All().ToList();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

    }
}
