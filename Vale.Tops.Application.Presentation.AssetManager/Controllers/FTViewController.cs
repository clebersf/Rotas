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
    public class FTViewController : Controller
    {
        private IAlarmFtvReadOnly AlarmFtvReadOnly;
        private IEventFtvReadOnly EventFtvReadOnly;
        ILog log = LogManager.GetLogger("Admin Cadastro AlarmFtv");
        public FTViewController ()
        {
            this.AlarmFtvReadOnly = new AlarmFtvReadOnly();
            this.EventFtvReadOnly = new EventFtvReadOnly();
        }

        // GET: AlarmFtvr
        public ActionResult AlarmFtv(AlarmFtvViewModel model)
        {
            string filter = model.Filter == null ? "" : model.Filter;
            DateTime? dhi = model.dhpi == null ? (DateTime.Now.AddDays(-1)).Date: model.dhpi;
            DateTime? dhf = model.dhpf == null ? (DateTime.Now.AddDays(1)).Date : model.dhpf;
            var ieList = this.AlarmFtvReadOnly.All().Where(p=>p.EventTimeStamp >= dhi & p.EventTimeStamp <= dhf & p.Message.Contains(filter)).Take(3000).OrderByDescending(p=>p.EventTimeStamp);

            model.Alarms = ieList;
            model.dhpi = dhi;
            model.dhpf = dhf;
            model.ViewMessage = DateTime.Now.ToString() + " - " + "Lista acessada.";
            model.lines = ieList.Count();

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("AlarmFtv", model);
        }

        public ActionResult EventFtv(EventFtvViewModel model)
        {
            string filter = model.Filter == null ? "" : model.Filter;
            DateTime? dhi = model.dhpi == null ? (DateTime.Now.AddDays(-3)).Date : model.dhpi;
            DateTime? dhf = model.dhpf == null ? (DateTime.Now.AddDays(1)).Date : model.dhpf;
            var ieList = this.EventFtvReadOnly.All().Where(p => p.TimeStmp >= dhi & p.TimeStmp <= dhf & p.MessageText.Contains(filter)).Take(3000).OrderByDescending(p => p.TimeStmp);

            model.Events = ieList;
            model.dhpi = dhi;
            model.dhpf = dhf;
            model.ViewMessage = DateTime.Now.ToString() + " - " + "Lista acessada.";
            model.lines = ieList.Count();

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("EventFtv", model);
        }


    }
}
