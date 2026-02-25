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
    public class FeederController : Controller
    {
        private ILocationWriteRead LocationWriteRead;
        private IFeederWriteRead FeederWriteRead;
        private IConveyorWriteRead ConveyorWriteRead;
        ILog log = LogManager.GetLogger("Admin Cadastro Location");
        public FeederController ()
        {
            this.LocationWriteRead = new LocationWriteRead();
            this.FeederWriteRead = new FeederWriteRead();
            this.ConveyorWriteRead = new ConveyorWriteRead();
        }

        // GET: Locationr
        public ActionResult Index()
        {
            var ieList = (from Location in this.LocationWriteRead.All()
                          join Feeder in this.FeederWriteRead.All() on Location.Id equals Feeder.Id
                          select Location);
            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            FeederViewModel model = new FeederViewModel { Locations = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(FeederViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.FeederWriteRead.Delete(model.Id); 
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
                            this.FeederWriteRead.Delete(model.Selecteds[i].id);
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
            var remove_1 = (from feed in this.FeederWriteRead.All()
                          join loc in this.LocationWriteRead.All() on feed.Id equals loc.Id
                          join conv in this.ConveyorWriteRead.All() on loc.ParentId equals conv.Id
                          select conv);
            var conveyor = this.ConveyorWriteRead.All().Except(remove_1);
            var remove_2 = (from feed in this.FeederWriteRead.All()
                            join loc in this.LocationWriteRead.All() on feed.Id equals loc.Id
                            join parent in this.FeederWriteRead.All() on loc.ParentId equals parent.Id
                            select parent);
            var feeder = this.FeederWriteRead.All().Except(remove_2);
            var add_1 = (from Location in this.LocationWriteRead.All()
                          join parent in feeder on Location.ParentId equals parent.Id
                          select Location);
            var add_2 = (from Location in this.LocationWriteRead.All()
                          join conv in conveyor on Location.ParentId equals conv.Id
                          select Location);
            var ieList = add_1.Union(add_2);
            return PartialView(new FeederViewModel { Locations = ieList });
        }

        [HttpPost]
        public ActionResult Create(FeederViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.FeederWriteRead.Save(new Feeder { Id = model.Id});
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
