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
    public class InstrumentMeasureController : Controller
    {
        private ILocationWriteRead LocationWriteRead;
        private ITagWriteRead TagWriteRead;
        private IrInstrumentMeasureWriteRead rInstrumentMeasureWriteRead;
        ILog log = LogManager.GetLogger("Admin Cadastro Location");
        public InstrumentMeasureController ()
        {
            this.TagWriteRead = new TagWriteRead();
            this.LocationWriteRead = new LocationWriteRead();
            this.rInstrumentMeasureWriteRead = new rInstrumentMeasureWriteRead();
        }

        // GET: Locationr
        public ActionResult Index()
        {
            var ieList = this.rInstrumentMeasureWriteRead.All().ToList();
            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            InstrumentMeasureViewModel model = new InstrumentMeasureViewModel { rInstrumentMeasures = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(InstrumentMeasureViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.rInstrumentMeasureWriteRead.Delete(model.Id);
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
                            this.rInstrumentMeasureWriteRead.Delete(model.Selecteds[i].id);
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
            var ieList_1 = (from tag in this.TagWriteRead.All()
                         select tag);
            var ieList_2 = (from Location in this.LocationWriteRead.All()
                            select Location);
            return PartialView(new InstrumentMeasureViewModel { Tags = ieList_1, Locations = ieList_2 });
        }

        [HttpPost]
        public ActionResult Create(InstrumentMeasureViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rInstrumentMeasureWriteRead.Save(new rInstrumentMeasure { Id = model.LocationId,
                        TagId = model.TagId, Value = model.Value });
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

        // GET: Asset/Create
        public ActionResult Edit(int id)
        {
            var ieList_1 = (from tag in this.TagWriteRead.All()
                            select tag); 
            var InstrumentMeasure = rInstrumentMeasureWriteRead.All().Where(p => p.Id == id).First();
            return PartialView(new InstrumentMeasureViewModel {
                Id = id,
                //LocationId = InstrumentMeasure.LocationId,
                TagId = InstrumentMeasure.TagId,
                Value = InstrumentMeasure.Value,
                Tags = ieList_1});
        }

        [HttpPost]
        public ActionResult Edit(InstrumentMeasureViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rInstrumentMeasureWriteRead.Edit(new rInstrumentMeasure
                    {
                        TagId = model.TagId,
                        Value = model.Value
                    });
                    log.Info("Cadastro de novo registro realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Cadastro de novo registro realizado pelo usuário ";
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

        // GET: Asset/Create
        public ActionResult Value(int id)
        {
            var InstrumentMeasure = rInstrumentMeasureWriteRead.All().Where(p => p.Id == id).First();
            return PartialView(new InstrumentMeasureViewModel
            {
                Id = id,
                Value = InstrumentMeasure.Value
            });
        }

        [HttpPost]
        public ActionResult Value(InstrumentMeasureViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var obj = rInstrumentMeasureWriteRead.All().Where(p => p.Id == model.Id).First();
                    obj.Value = model.Value;
                    this.rInstrumentMeasureWriteRead.Edit(obj);
                    log.Info("Cadastro de novo registro realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Cadastro de novo registro realizado pelo usuário ";
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
