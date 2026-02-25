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
    public class OxDController : Controller
    {
        private ILocationWriteRead LocationWriteRead;
        private IOriginWriteRead OriginWriteRead;
        private IDestinationWriteRead DestinationWriteRead;
        private IOxDWriteRead OxDWriteRead;
        ILog log = LogManager.GetLogger("Admin Cadastro Location");
        public OxDController ()
        {
            this.LocationWriteRead = new LocationWriteRead();
            this.OriginWriteRead = new OriginWriteRead();
            this.DestinationWriteRead = new DestinationWriteRead();
            this.OxDWriteRead = new OxDWriteRead();
        }

        // GET: Locationr
        [AllowAnonymous]
        public ActionResult Index()
        {
            var ieList = this.OxDWriteRead.All().ToList();
            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            OxDViewModel model = new OxDViewModel { OxDs = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(OxDViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.OriginWriteRead.Delete(model.Id); 
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
                            this.OriginWriteRead.Delete(model.Selecteds[i].id);
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

        // GET: Asset/Create
        public ActionResult Create()
        {
            var ieList_1 = (from Location in this.LocationWriteRead.All()
                          join tpo in this.OriginWriteRead.All() on Location.Id equals tpo.Id
                            select Location);
            var ieList_2 = (from Location in this.LocationWriteRead.All()
                            join tpd in this.DestinationWriteRead.All() on Location.Id equals tpd.Id
                            select Location);
            return PartialView(new OxDViewModel { Locations_1 = ieList_1, Locations_2 = ieList_2 });
        }

        [HttpPost]
        public ActionResult Create(OxDViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.OxDWriteRead.Save(new OxD { Id = 1, OriginId = model.OriginId, DestinationId = model.DestinationId  });
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
