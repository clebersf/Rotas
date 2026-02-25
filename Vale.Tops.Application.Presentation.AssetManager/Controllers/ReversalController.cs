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
using RazorEngine.Compilation.ImpromptuInterface.Dynamic;

namespace Vale.Tops.Application.Presentation.AssetManager.Controllers
{
    public class ReversalController : Controller
    {
        private ILocationWriteRead LocationWriteRead;
        private IReversalWriteRead ReversalWriteRead;
        private IConveyorWriteRead ConveyorWriteRead;
        private IConsistencyWriteRead ConsistencyWriteRead;
        private IReferenceWriteRead ReferenceWriteRead;
        private IPermissionWriteRead PermissionWriteRead;
        private IInstrumentWriteRead InstrumentWriteRead;
        ILog log = LogManager.GetLogger("Admin Cadastro Location");
        public ReversalController ()
        {
            this.LocationWriteRead = new LocationWriteRead();
            this.ReversalWriteRead = new ReversalWriteRead();
            this.ConveyorWriteRead = new ConveyorWriteRead();
            this.ConsistencyWriteRead = new ConsistencyWriteRead();
            this.ReferenceWriteRead = new ReferenceWriteRead();
            this.PermissionWriteRead = new PermissionWriteRead();
            this.InstrumentWriteRead = new InstrumentWriteRead();
        }

        // GET: Locationr
        public ActionResult Index()
        {
            var ieList = (from rev in this.ReversalWriteRead.All()
                          join loc in this.LocationWriteRead.All() on rev.Location.ParentId equals loc.Id
                          join cons in this.LocationWriteRead.All() on rev.Location.Id equals cons.ParentId
                          join consist in this.ConsistencyWriteRead.All() on cons.Id equals consist.Id
                          select loc);
            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            ReversalViewModel model = new ReversalViewModel { Reversals = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(ReversalViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.ReversalWriteRead.Delete(model.Id); 
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
                            this.ReversalWriteRead.Delete(model.Selecteds[i].id);
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
            var Revs = (from rev in this.ReversalWriteRead.All()
                        join loc in this.LocationWriteRead.All() on rev.Location.ParentId equals loc.Id
                        join cons in this.LocationWriteRead.All() on rev.Location.Id equals cons.ParentId
                        join consist in this.ConsistencyWriteRead.All() on cons.Id equals consist.Id
                        select loc);
            var Convs  = (from loc in this.ConveyorWriteRead.All()
                       select loc.Location);

            var Elects = (from loc in this.ConveyorWriteRead.All()
                         select loc.Location).Except(Revs);
            return PartialView(new ReversalViewModel { Reversals = Revs, Eligible = Elects, References = Convs  });
        }

        [HttpPost]
        public ActionResult Create(ReversalViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Conveyor = this.LocationWriteRead.All().Where(p => p.Id == model.Id).First();
                    var locrev = new Location { TypeId = 14,
                        ParentId = Conveyor.Id,
                        Name = Conveyor.Name + "_RV", 
                        Alias = Conveyor.Name + "_RV",
                        Description = "Reversão da Correia " + Conveyor.Name };                    
                    this.LocationWriteRead.Save(locrev);

                    var Last = this.LocationWriteRead.All().Last();
                    var loccons = new Location
                    {
                        TypeId = 7,
                        ParentId = Last.Id,
                        Name = "RV_" + Conveyor.Name + "_CON",
                        Alias = "RV_" + Conveyor.Name + "_CON",
                        Description = "Consistência de Reversão da Correia" + Conveyor.Name
                    };
                    this.LocationWriteRead.Save(loccons);
                    this.ReversalWriteRead.Save(new Reversal { Id = locrev.Id});
                    this.ConsistencyWriteRead.Save(new Consistency { Id = loccons.Id });
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
        public ActionResult Reference(int id)
        {
            var Cons = (from rev in this.ReversalWriteRead.All()
                        join loc in this.LocationWriteRead.All().Where(p=>p.Id == id) on rev.Location.ParentId equals loc.Id
                        join cons in this.LocationWriteRead.All() on rev.Location.Id equals cons.ParentId
                        select cons).First();
            ReversalViewModel model = new ReversalViewModel { Id = Cons.Id };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Reference(ReversalViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var locref = new Location
                    {
                        TypeId = 8,
                        ParentId = model.Id,
                        Name = "RV_"+ model.Name + "_REF_"+ model.TypeRef,
                        Alias = "RV_" + model.Name + "_REF_" + model.TypeRef,
                        Description = "Referência da Correia " + model.Name+" Sentido " + model.TypeRef
                    };
                    this.LocationWriteRead.Save(locref);
                    var lref = LocationWriteRead.All().Last();
                    var refer = new Reference
                    {
                        Id = lref.Id,
                        LocReferenceId = model.LocRefId, Value = 0
                    };
                    var locperm = new Location
                    {
                        TypeId = 9,
                        ParentId = locref.Id,
                        Name = "RV_" + model.Name + "_PERM_" + model.TypeRef,
                        Alias = "RV_" + model.Name + "_PERM_" + model.TypeRef,
                        Description = "Permissão da Correia " + model.Name + " Sentido " + model.TypeRef
                    };
                    this.LocationWriteRead.Save(locperm);
                    var lperm = LocationWriteRead.All().Last();
                    var perme = new Permission
                    {
                        Id = lperm.Id,
                    };
                    var locinst = new Location
                    {
                        TypeId = 4,
                        ParentId = locperm.Id,
                        Name = "INST_RV_" + model.Name + "_PERM_" + model.TypeRef,
                        Alias = "INST_RV_" + model.Name + "_PERM_" + model.TypeRef,
                        Description = "Intrumento de Permissão da Correia " + model.Name + " Sentido " + model.TypeRef
                    };
                    this.LocationWriteRead.Save(locinst);
                    var linst = LocationWriteRead.All().Last();
                    var instr = new Instrument
                    {
                        Id = linst.Id,
                    };
                    this.ReferenceWriteRead.Save(refer);
                    //this.ReversalWriteRead.Edit(new Tops.Domain.Type { Description = model.Description, Id = model.Id, Name = model.Name });
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
