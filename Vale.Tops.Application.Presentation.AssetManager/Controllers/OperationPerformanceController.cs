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
    public class OperationPerformanceController : Controller
    {
        private IvwEffectiveRateReadOnly EffectiveRateReadOnly;
        private ILocationWriteRead LocationWriteRead;
        private IAssetWriteRead AssetWriteRead;

        ILog log = LogManager.GetLogger("Admin Cadastro OperationPerformance");
        public OperationPerformanceController ()
        {
            this.EffectiveRateReadOnly = new vwEffectiveRateReadOnly();
            this.LocationWriteRead = new LocationWriteRead();
            this.AssetWriteRead = new AssetWriteRead();        }

        public ActionResult Painel(OperationPerformanceViewModel model)
        {
            return View("Painel", model);
        }
        // GET: OperationPerformancer
        public ActionResult History(OperationPerformanceViewModel model)
        {
            var ieLoc = (from location in this.LocationWriteRead.All().Where(p=>p.TypeId == 112 | p.TypeId == 207)
                          join asset in this.AssetWriteRead.All() on location.Id equals asset.Id
                          select location);
            DateTime? dhi = model.dhpi == null ? (DateTime.Now.AddDays(-3)).Date : model.dhpi;
            DateTime? dhf = model.dhpf == null ? (DateTime.Now.AddDays(1)).Date : model.dhpf;
            var l1 = this.EffectiveRateReadOnly.All().ToList().Where(p=>p.Inicio >= dhi & p.Inicio <= dhf);
            if (model.Origem != "" & model.Origem != null)
            {
                l1 = l1.Where(p => p.Origem == model.Origem).ToList();
            }
            

            model.EffectiveRates = l1.OrderByDescending(p=>p.Inicio);
            model.Locations = ieLoc;
            model.dhpi = dhi;
            model.dhpf = dhf;
            model.Message = DateTime.Now.ToString() + " - " + "Lista acessada.";
            model.lines = l1.Count();
            

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("History", model);
        }

        [AllowAnonymous]
        public ActionResult PowerBi(OperationPerformanceViewModel model,string Origem,  DateTime pdhi, DateTime pdhf)
        {

            model.Origem = Origem;
            DateTime? dhi = pdhi == null ? (DateTime.Now.AddDays(-3)).Date : pdhi;
            DateTime? dhf = pdhf == null ? (DateTime.Now.AddDays(1)).Date : pdhf;
            var ieLoc = (from location in this.LocationWriteRead.All().Where(p => p.TypeId == 112 | p.TypeId == 207)
                         join asset in this.AssetWriteRead.All() on location.Id equals asset.Id
                         select location);
            var l1 = this.EffectiveRateReadOnly.All().ToList().Where(p => p.Inicio >= dhi & p.Inicio <= dhf);
            if (model.Origem != "" & model.Origem != null)
            {
                l1 = l1.Where(p => p.Origem == model.Origem).ToList();
            }


            model.EffectiveRates = l1.OrderByDescending(p => p.Inicio);
            model.Locations = ieLoc;
            model.dhpi = dhi;
            model.dhpf = dhf;
            model.Message = DateTime.Now.ToString() + " - " + "Lista acessada.";
            model.lines = l1.Count();


            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("PowerBi", model);
        }


        public PartialViewResult Current(OperationPerformanceViewModel model)
        {
            
            var l1 = this.EffectiveRateReadOnly.All().ToList().Where(p=>p.Final == false);
            model.EffectiveRates = l1;
            model.Message = DateTime.Now.ToString() + " - " + "Lista acessada.";
            model.lines = l1.Count();


            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return PartialView("Current", model);
        }


    }
}
