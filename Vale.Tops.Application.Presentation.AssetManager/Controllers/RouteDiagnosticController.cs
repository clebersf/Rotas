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
using MoreLinq;

namespace Vale.Tops.Application.Presentation.AssetManager.Controllers
{
    public class RouteDiagnosticController : Controller
    {
        private Ivw_Route_Consistency_Feeder_ReadOnly vw_Route_Consistency_Feeder_ReadOnly;
        private Ivw_Route_Consistency_Reversal_ReadOnly vw_Route_Consistency_Reversal_ReadOnly;
        private Ivw_Route_Consistency_Tripper_ReadOnly vw_Route_Consistency_Tripper_ReadOnly;

        private Ivw_Route_Tag_Eqp_Permission_ReadOnly vw_Route_Tag_Eqp_Permission_ReadOnly;
        private Ivw_Route_Tag_Feeder_Command_ReadOnly vw_Route_Tag_Feeder_Command_ReadOnly;
        private Ivw_Route_Tag_Feeder_Permission_ReadOnly vw_Route_Tag_Feeder_Permission_ReadOnly;
        private Ivw_Route_Tag_OxD_Permission_ReadOnly vw_Route_Tag_OxD_Permission_ReadOnly;
        private Ivw_Route_Tag_Reversal_Command_ReadOnly vw_Route_Tag_Reversal_Command_ReadOnly;
        private Ivw_Route_Tag_Reversal_Permission_ReadOnly vw_Route_Tag_Reversal_Permission_ReadOnly;
        private Ivw_Route_Tag_Tripper_Command_ReadOnly vw_Route_Tag_Tripper_Command_ReadOnly;
        private Ivw_Route_Tag_Tripper_Permission_ReadOnly vw_Route_Tag_Tripper_Permission_ReadOnly;

        private IRouteWriteRead RouteWriteRead;
        private ILogWriteRead LogWriteRead;
        private ILocationWriteRead LocationWriteRead;

        private IrInstrumentMeasureWriteRead rInstrumentMeasureWriteRead;

        private ITagWriteRead TagWriteRead;

        ILog log = LogManager.GetLogger("Admin Cadastro Location");
        public RouteDiagnosticController()
        {
            this.vw_Route_Consistency_Feeder_ReadOnly = new vw_Route_Consistency_Feeder_ReadOnly();
            this.vw_Route_Consistency_Reversal_ReadOnly = new vw_Route_Consistency_Reversal_ReadOnly();
            this.vw_Route_Consistency_Tripper_ReadOnly = new vw_Route_Consistency_Tripper_ReadOnly();
            this.vw_Route_Tag_Eqp_Permission_ReadOnly = new vw_Route_Tag_Eqp_Permission_ReadOnly();
            this.vw_Route_Tag_Feeder_Permission_ReadOnly = new vw_Route_Tag_Feeder_Permission_ReadOnly();
            this.vw_Route_Tag_Feeder_Command_ReadOnly = new vw_Route_Tag_Feeder_Command_ReadOnly();
            this.vw_Route_Tag_OxD_Permission_ReadOnly = new vw_Route_Tag_OxD_Permission_ReadOnly();
            this.vw_Route_Tag_Reversal_Command_ReadOnly = new vw_Route_Tag_Reversal_Command_ReadOnly();
            this.vw_Route_Tag_Reversal_Permission_ReadOnly = new vw_Route_Tag_Reversal_Permission_ReadOnly();
            this.vw_Route_Tag_Tripper_Command_ReadOnly = new vw_Route_Tag_Tripper_Command_ReadOnly();
            this.vw_Route_Tag_Tripper_Permission_ReadOnly = new vw_Route_Tag_Tripper_Permission_ReadOnly();
            this.rInstrumentMeasureWriteRead = new rInstrumentMeasureWriteRead();
            this.TagWriteRead = new TagWriteRead();
            this.LogWriteRead = new LogWriteRead();
            this.RouteWriteRead = new RouteWriteRead();
            this.LocationWriteRead = new LocationWriteRead();
        }

        // GET: Locationr
        public ActionResult Feeder()
        {
            var ieList = vw_Route_Consistency_Feeder_ReadOnly.All();
            RouteDiagnosticViewModel model = new RouteDiagnosticViewModel { cfeeders = ieList, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };
            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Feeder", model);
        }

        public ActionResult Reversal()
        {
            var ieList = vw_Route_Consistency_Reversal_ReadOnly.All();
            RouteDiagnosticViewModel model = new RouteDiagnosticViewModel { creversals = ieList, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };
            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Reversal", model);
        }
        public ActionResult Log(RouteDiagnosticViewModel model)
        {
            string filter = model.Filter == null ? "" : model.Filter;
            string negfilter = model.NegFilter == null | model.NegFilter == "" ? "##$$%%&&" : model.NegFilter;
            var log = from lg in this.LogWriteRead.All()
                      .Where(p => (p.Message.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0))
                .Where(p => (p.Message.IndexOf(negfilter, StringComparison.OrdinalIgnoreCase) < 0)).Take(500)
                      join rt in this.RouteWriteRead.All() 
                on lg.LocationId equals rt.Id
                      select lg;

            model = new RouteDiagnosticViewModel { lgs = log, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = log.Count() };
            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Log", model);
        }

        public ActionResult Consistency()
        {
            var iefeeder = from fed in vw_Route_Consistency_Feeder_ReadOnly.All()
                           select new vw_Route_Consistency_Tripper { AssetCurrent = fed.AssetCurrent, AssetCurrentId = fed.AssetCurrentId,
                               AssetNext = fed.AssetNext, AssetNextId = fed.AssetNextId, BoolVeredict = fed.BoolVeredict, Current = fed.Current,
                               Desired = fed.Desired, Id = fed.Id, ReferenceId = fed.ReferenceId, Route = fed.Route, TagId = fed.TagId, TagName = fed.TagName, Veredict = fed.Veredict,
                               Type = fed.Type, VwId = fed.VwId };
            var iereversal = from fed in vw_Route_Consistency_Reversal_ReadOnly.All()
                             select new vw_Route_Consistency_Tripper
                             {
                                 AssetCurrent = fed.AssetCurrent,
                                 AssetCurrentId = fed.AssetCurrentId,
                                 AssetNext = fed.AssetNext,
                                 AssetNextId = fed.AssetNextId,
                                 BoolVeredict = Convert.ToUInt16(fed.BoolVeredict),
                                 Current = fed.Current,
                                 Desired = Convert.ToDouble(fed.Desired),
                                 Id = fed.Id,
                                 ReferenceId = fed.ReferenceId,
                                 Route = fed.Route,
                                 TagId = fed.TagId,
                                 TagName = fed.TagName,
                                 Veredict = fed.Veredict,
                                 Type = fed.Type,
                                 VwId = fed.VwId
                             };
            var ietripper = vw_Route_Consistency_Tripper_ReadOnly.All();
            ietripper = ietripper.Union(iefeeder).Union(iereversal);
            RouteDiagnosticViewModel model = new RouteDiagnosticViewModel { ctrippers = ietripper, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ietripper.Count() };
            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Consistency", model);
        }

        public ActionResult TagWrite()
        {
            var pfeed = vw_Route_Tag_Feeder_Permission_ReadOnly.All();
            var cfeed = from comm in vw_Route_Tag_Feeder_Command_ReadOnly.All()
                        select new vw_Route_Tag_Feeder_Permission { Id = comm.Id, Reference = comm.Reference, Tag = comm.Tag, TagValue = comm.TagValue, Type = comm.Type, TagId = comm.TagId, Name = comm.Name };
            var ptag = from comm in vw_Route_Tag_Eqp_Permission_ReadOnly.All()
                       select new vw_Route_Tag_Feeder_Permission { Id = comm.Id, Tag = comm.Tag, TagValue = comm.TagValue, Type = comm.Type, TagId = comm.TagId, Reference = 1, Name = comm.Name };
            var poxd = from comm in vw_Route_Tag_OxD_Permission_ReadOnly.All()
                       select new vw_Route_Tag_Feeder_Permission { Id = comm.Id, Tag = comm.Tag, TagValue = comm.TagValue, Type = comm.Type, TagId = comm.TagId, Reference = 1, Name = comm.Name };
            var prev = from comm in vw_Route_Tag_Reversal_Permission_ReadOnly.All()
                       select new vw_Route_Tag_Feeder_Permission { Id = comm.Id, Tag = comm.Tag, TagValue = comm.TagValue, Type = comm.Type, TagId = comm.TagId, Reference = 1, Name = comm.Name };
            var crev = from comm in vw_Route_Tag_Reversal_Command_ReadOnly.All()
                       select new vw_Route_Tag_Feeder_Permission { Id = comm.Id, Tag = comm.Tag, TagValue = comm.TagValue, Type = comm.Type, TagId = comm.TagId, Reference = 1, Name = comm.Name };
            var ptrip = from comm in vw_Route_Tag_Tripper_Permission_ReadOnly.All()
                        select new vw_Route_Tag_Feeder_Permission { Id = comm.Id, Tag = comm.Tag, TagValue = comm.TagValue, Type = comm.Type, TagId = comm.TagId, Reference = 1, Name = comm.Name };
            var ctrip = from comm in vw_Route_Tag_Tripper_Command_ReadOnly.All()
                        select new vw_Route_Tag_Feeder_Permission { Id = comm.Id, Tag = comm.Tag, TagValue = comm.TagValue, Type = comm.Type, TagId = comm.TagId, Reference = 1, Name = comm.Name };

            pfeed = pfeed.Union(cfeed).Union(ptag).Union(poxd).Union(prev).Union(crev).Union(ptrip).Union(ctrip);
            RouteDiagnosticViewModel model = new RouteDiagnosticViewModel { permfeeder = pfeed.DistinctBy(p=>p.TagId), Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = pfeed.Count() };
            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("TagWrite", model);
        }

        // GET: Asset/Edit/5
        public ActionResult Simulation(int id)
        {
            var iefeeder = from fed in vw_Route_Consistency_Feeder_ReadOnly.All()
                           select new vw_Route_Consistency_Tripper
                           {
                               AssetCurrent = fed.AssetCurrent,
                               AssetCurrentId = fed.AssetCurrentId,
                               AssetNext = fed.AssetNext,
                               AssetNextId = fed.AssetNextId,
                               BoolVeredict = fed.BoolVeredict,
                               Current = fed.Current,
                               Desired = fed.Desired,
                               Id = fed.Id,
                               ReferenceId = fed.ReferenceId,
                               Route = fed.Route,
                               TagId = fed.TagId,
                               TagName = fed.TagName,
                               Veredict = fed.Veredict,
                               Type = fed.Type,
                               VwId = fed.VwId
                           };
            var iereversal = from fed in vw_Route_Consistency_Reversal_ReadOnly.All()
                             select new vw_Route_Consistency_Tripper
                             {
                                 AssetCurrent = fed.AssetCurrent,
                                 AssetCurrentId = fed.AssetCurrentId,
                                 AssetNext = fed.AssetNext,
                                 AssetNextId = fed.AssetNextId,
                                 BoolVeredict = Convert.ToUInt16(fed.BoolVeredict),
                                 Current = fed.Current,
                                 Desired = Convert.ToDouble(fed.Desired),
                                 Id = fed.Id,
                                 ReferenceId = fed.ReferenceId,
                                 Route = fed.Route,
                                 TagId = fed.TagId,
                                 TagName = fed.TagName,
                                 Veredict = fed.Veredict,
                                 Type = fed.Type,
                                 VwId = fed.VwId
                             };
            var ietripper = vw_Route_Consistency_Tripper_ReadOnly.All();
            ietripper = ietripper.Union(iefeeder).Union(iereversal);

            var result = from fed in ietripper.Where(p => p.TagId == id)
                             select new RouteDiagnosticViewModel
                             {
                                 Desired = Convert.ToDouble(fed.Desired).ToString(),
                                 TagName = fed.TagName,
                                 TagValue = fed.Current,
                                 TagId = fed.TagId
                             };
            RouteDiagnosticViewModel model = result.First();
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Simulation(RouteDiagnosticViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var meas = rInstrumentMeasureWriteRead.All().Where(p => p.TagId == model.TagId).First();
                    meas.Value = model.TagValue;
                    this.rInstrumentMeasureWriteRead.Edit(meas);
                    log.Info("Modificado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Modificado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("Consistency");
                }
                // model is not valid
                return Json(new
                {
                    status = "failure",
                    formErrors = ModelState.Select(kvp => new { key = kvp.Key, errors = kvp.Value.Errors.Select(e => e.ErrorMessage) })
                });
            }
            catch (Exception ex)
            {
                Session["Message"] = DateTime.Now.ToString() + " - " + "Erro: " + ex.Message;
                return RedirectToAction("Consistency");
            }
        }

    }
    

}
