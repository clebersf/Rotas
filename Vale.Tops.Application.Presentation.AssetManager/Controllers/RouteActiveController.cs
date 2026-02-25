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
    public class RouteActiveController : Controller
    {
        private IRouteWriteRead RouteWriteRead;
        private IrRouteActiveWriteRead rRouteActiveWriteRead;
        private IrRouteReplaceWriteRead rRouteReplaceWriteRead;
        private IOxDWriteRead OxDWriteRead;
        private Ivw_Rule_Route_ActiveReadOnly vw_Rule_Route_ActiveReadOnly;
        ILog log = LogManager.GetLogger("Admin Cadastro Route");
        public RouteActiveController ()
        {
            this.RouteWriteRead = new RouteWriteRead();
            this.rRouteActiveWriteRead = new rRouteActiveWriteRead();
            this.rRouteReplaceWriteRead = new rRouteReplaceWriteRead();
            this.OxDWriteRead = new OxDWriteRead();
            this.vw_Rule_Route_ActiveReadOnly = new vw_Rule_Route_ActiveReadOnly();
        }

        // GET: Router
        public ActionResult Index()
        {
            var ieList = from route in this.rRouteActiveWriteRead.All()
                         select route;
            var rep = rRouteReplaceWriteRead.All();
            RouteActiveViewModel model = new RouteActiveViewModel { ActiveRoutes = ieList, Replaces = rep,  Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(RouteActiveViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.rRouteActiveWriteRead.Delete(model.Id);
                    log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);                    
                    Session["Message"] =  DateTime.Now.ToString() + " - " + "Deletado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("Index");
                }
                else
                {
                    bool none = true;
                    for (int i = 0; i < model.Selecteds.Count(); i++)
                    {
                        if (model.Selecteds[i].isSelected == true)
                        {
                            this.rRouteActiveWriteRead.Delete(model.Selecteds[i].id);
                            log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                            none = false;
                        }                        
                    }
                    if (!none)
                    {
                        Session["Message"] = DateTime.Now.ToString() + " - " + "O(s) registros foram deletados.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Session["Message"] = DateTime.Now.ToString() + " - " + "Não houveram registros selecionados.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                Session["Message"] = DateTime.Now.ToString() + " - " + "Provavel erro de dependência de dados.";
                return RedirectToAction("Index");
            }
        }

        // GET: Asset/Edit/5
        public ActionResult Edit(int id)
        {
            var _Route = (from Route in this.RouteWriteRead.All().Where(p => p.Id.Equals(id))
                          select Route).First();
            bool active = _Route != null;
            RouteActiveViewModel model = new RouteActiveViewModel { Id = _Route.Id, Active = active};
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Edit(RouteActiveViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rRouteActiveWriteRead.Save(new rRouteActive {Id = model.Id });
                    log.Info("Modificado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);                    
                    Session["Message"] =  DateTime.Now.ToString() + " - " + "Modificado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("Index");
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
                Session["Message"] =  DateTime.Now.ToString() + " - " + "Erro: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}
