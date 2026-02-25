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
    public class StockController : Controller
    {
        private ILocationWriteRead LocationWriteRead;
        private IStockWriteRead StockWriteRead;
        ILog log = LogManager.GetLogger("Admin Cadastro Location");
        public StockController ()
        {
            this.LocationWriteRead = new LocationWriteRead();
            this.StockWriteRead = new StockWriteRead();
        }

        // GET: Locationr
        public ActionResult Index()
        {
            var ieList = (from Location in this.LocationWriteRead.All()
                          join Stock in this.StockWriteRead.All() on Location.Id equals Stock.Id
                          select Location);
            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            StockViewModel model = new StockViewModel { Locations = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(StockViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.StockWriteRead.Delete(model.Id); 
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
                            this.StockWriteRead.Delete(model.Selecteds[i].id);
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
            var ieList = (from Location in this.LocationWriteRead.All()
                          select Location);

            var remove = (from Location in this.LocationWriteRead.All()
                          join rem in this.StockWriteRead.All() on Location.Id equals rem.Id
                          select Location);
            return PartialView(new StockViewModel { Locations = ieList.Except(remove) });
        }

 
        [HttpPost]
        public ActionResult Create(StockViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.StockWriteRead.Save(new Stock { Id = model.Id});
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
