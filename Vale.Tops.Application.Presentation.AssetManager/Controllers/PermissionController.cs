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
    public class PermissionController : Controller
    {
        private ILocationWriteRead LocationWriteRead;
        private IPermissionWriteRead PermissionWriteRead;
        ILog log = LogManager.GetLogger("Admin Cadastro Location");
        public PermissionController ()
        {
            this.LocationWriteRead = new LocationWriteRead();
            this.PermissionWriteRead = new PermissionWriteRead();
        }

        // GET: Locationr
        public ActionResult Index()
        {
            var ieList = (from Permission in this.PermissionWriteRead.All()
                          join Location in LocationWriteRead.All() on Permission.Id equals Location.Id
                          select Location);
            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            PermissionViewModel model = new PermissionViewModel { Locations = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(PermissionViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.PermissionWriteRead.Delete(model.Id); 
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
                            this.PermissionWriteRead.Delete(model.Selecteds[i].id);
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

        // GET: Permission/Create
        public ActionResult Create()
        {
            var ieList = (from Location in this.LocationWriteRead.All()
                          select Location);

            var remove = (from Location in this.LocationWriteRead.All()
                          join rem in this.PermissionWriteRead.All() on Location.Id equals rem.Id
                          select Location);

            return PartialView(new PermissionViewModel { Locations = ieList.Except(remove) });
        }

      
        [HttpPost]
        public ActionResult Create(PermissionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.PermissionWriteRead.Save(new Permission { Id = model.Id});
                    log.Info("Cadastro de novo registro realizado pelo usuário " + User.Identity.Name);                    
                    Session["Message"] =  DateTime.Now.ToString() + " - " + "Cadastro de novo registro realizado pelo usuário ";
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
