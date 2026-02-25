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
    public class HistoryLogController : Controller
    {        
        private IRouteWriteRead RouteWriteRead;
        private ILogWriteRead LogWriteRead;
        private ILocationWriteRead LocationWriteRead;
        private Ivw_Agent_History_Performance_ReadOnly vw_Agent_History_Performance_ReadOnly;

        ILog log = LogManager.GetLogger("Admin Cadastro Location");
        public HistoryLogController()
        {
            this.vw_Agent_History_Performance_ReadOnly = new vw_Agent_History_Performance_ReadOnly();
            this.LogWriteRead = new LogWriteRead();
            this.RouteWriteRead = new RouteWriteRead();
            this.LocationWriteRead = new LocationWriteRead();
        }

        public ActionResult Log(HistoryLogViewModel model)
        {
            string filter = model.Filter == null ? "" : model.Filter;
            string negfilter = model.NegFilter == null | model.NegFilter == "" ? "##$$%%&&" : model.NegFilter;
            var log = from lg in this.LogWriteRead.All()
                      .Where(p => (p.Message.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0))
                .Where(p => (p.Message.IndexOf(negfilter, StringComparison.OrdinalIgnoreCase) < 0)).Take(500)
                      join rt in this.RouteWriteRead.All() 
                on lg.LocationId equals rt.Id
                      select lg;

            model = new HistoryLogViewModel { lgs = log, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = log.Count() };
            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Log", model);
        }

        public ActionResult Job(HistoryLogViewModel model)
        {
            DateTime? gdhi = (DateTime?)HttpContext.Application["dhi-" + User.Identity.Name];
            DateTime? gdhf = (DateTime?)HttpContext.Application["dhf-" + User.Identity.Name];
            DateTime? dhi = model.dhpi == null ? gdhi != null ? gdhi : DateTime.Now.AddDays(-90) : model.dhpi;
            DateTime? dhf = model.dhpf == null ? gdhf != null ? gdhf : DateTime.Now.AddHours(23).AddMinutes(59).AddSeconds(59) : model.dhpf;
            string filter = model.Filter == null ? "" : model.Filter;
            string negfilter = model.NegFilter == null | model.NegFilter == "" ? "##$$%%&&" : model.NegFilter;
            var ieList = this.vw_Agent_History_Performance_ReadOnly.All()
                .Where(p => (p.Message.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 || p.Status.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 || p.Performance.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0))
                .Where(p => (p.Message.IndexOf(negfilter, StringComparison.OrdinalIgnoreCase) < 0 && p.Status.IndexOf(negfilter, StringComparison.OrdinalIgnoreCase) < 0 && p.Performance.IndexOf(negfilter, StringComparison.OrdinalIgnoreCase) < 0)).Take(500)
                .Where(p => p.DateTime >= dhi.Value & p.DateTime <= dhf.Value);

            model = new HistoryLogViewModel { agenthistory = ieList, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };
            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Job", model);
        }

    }
    

}
