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
    public class ReferenceController : Controller
    {
        private ILocationWriteRead LocationWriteRead;
        private IReferenceWriteRead ReferenceWriteRead;
        ILog log = LogManager.GetLogger("Admin Cadastro Location");
        public ReferenceController ()
        {
            this.LocationWriteRead = new LocationWriteRead();
            this.ReferenceWriteRead = new ReferenceWriteRead();
        }

        // GET: Locationr
        public ActionResult Index()
        {
            var ieList = (from Reference in this.ReferenceWriteRead.All()
                          join Location in LocationWriteRead.All() on Reference.Id equals Location.Id
                          select Location);
            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            ReferenceViewModel model = new ReferenceViewModel { Locations = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(ReferenceViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.ReferenceWriteRead.Delete(model.Id); 
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
                            this.ReferenceWriteRead.Delete(model.Selecteds[i].id);
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

        // GET: Reference/Create
        public ActionResult Create()
        {
            var ieList = (from Location in this.LocationWriteRead.All()
                          select Location);
            var remove = (from Location in this.LocationWriteRead.All()
                          join rem in this.ReferenceWriteRead.All() on Location.Id equals rem.Id
                          select Location);
            return PartialView(new ReferenceViewModel { Locations = ieList.Except(remove) });
        }

      
        [HttpPost]
        public ActionResult Create(ReferenceViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.ReferenceWriteRead.Save(new Reference { Id = model.Id});
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

        // GET: Asset/Edit/5
        public ActionResult Edit(int id)
        {
            var refer = this.ReferenceWriteRead.All().Where(p => p.Id == id).FirstOrDefault();
            return PartialView(new ReferenceViewModel {Id = id, Value = refer.Value });
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Edit(ReferenceViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var refer = this.ReferenceWriteRead.All().Where(p => p.Id == model.Id).FirstOrDefault() ;
                    refer.Value = model.Value;
                    this.ReferenceWriteRead.Edit(refer);
                    log.Info("Modificado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Modificado cadastro do registro " + model.Id.ToString();
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
                Session["Message"] = DateTime.Now.ToString() + " - " + "Erro: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}
