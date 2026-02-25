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
    public class TripperController : Controller
    {
        private ILocationWriteRead LocationWriteRead;
        private ITripperWriteRead TripperWriteRead;
        private IConveyorWriteRead ConveyorWriteRead;
        ILog log = LogManager.GetLogger("Admin Cadastro Location");
        public TripperController ()
        {
            this.LocationWriteRead = new LocationWriteRead();
            this.TripperWriteRead = new TripperWriteRead();
            this.ConveyorWriteRead = new ConveyorWriteRead();
        }

        // GET: Locationr
        [AllowAnonymous]
        public ActionResult Index()
        {
            var ieList = (from Location in this.LocationWriteRead.All()
                          join Tripper in this.TripperWriteRead.All() on Location.Id equals Tripper.Id
                          select Location);
            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            TripperViewModel model = new TripperViewModel { Locations = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(TripperViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.TripperWriteRead.Delete(model.Id); 
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
                            this.TripperWriteRead.Delete(model.Selecteds[i].id);
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
            var remove_1 = (from feed in this.TripperWriteRead.All()
                            join loc in this.LocationWriteRead.All() on feed.Id equals loc.Id
                            join conv in this.ConveyorWriteRead.All() on loc.ParentId equals conv.Id
                            select conv);
            var conveyor = this.ConveyorWriteRead.All().Except(remove_1);
            var remove_2 = (from trip in this.TripperWriteRead.All()
                            join loc in this.LocationWriteRead.All() on trip.Id equals loc.Id
                            join parent in this.TripperWriteRead.All() on loc.ParentId equals parent.Id
                            select parent);
            var tripper = this.TripperWriteRead.All().Except(remove_2);
            var add_1 = (from Location in this.LocationWriteRead.All()
                          join parent in tripper on Location.ParentId equals parent.Id
                          select Location);
            var add_2 = (from Location in this.LocationWriteRead.All()
                         join conv in conveyor on Location.ParentId equals conv.Id
                         select Location);
            var ieList = add_1.Union(add_2);
            return PartialView(new TripperViewModel { Locations = ieList });
        }

        [HttpPost]
        public ActionResult Create(TripperViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.TripperWriteRead.Save(new Tripper { Id = model.Id});
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
