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
using MoreLinq;

namespace Vale.Tops.Application.Presentation.AssetManager.Controllers
{
    public class RouteController : Controller
    {
        private IConveyorWriteRead ConveyorWriteRead;
        private IOriginWriteRead OriginWriteRead;
        private IDestinationWriteRead DestinationWriteRead;
        private ILocationWriteRead LocationWriteRead;
        private IRouteWriteRead RouteWriteRead;
        private IrRouteOxDWriteRead rRouteOxDWriteRead;
        private IrRouteActiveWriteRead rRouteActiveWriteRead;
        private IrRouteSequenceWriteRead rRouteSequenceWriteRead;
        private ILogWriteRead LogWriteRead;
        private OxDWriteRead OxDWriteRead;
        ILog log = LogManager.GetLogger("Admin Cadastro Route");
        public RouteController ()
        {
            this.RouteWriteRead = new RouteWriteRead();
            this.rRouteOxDWriteRead = new rRouteOxDWriteRead();
            this.OxDWriteRead = new OxDWriteRead();
            this.rRouteActiveWriteRead = new rRouteActiveWriteRead();
            this.LogWriteRead = new LogWriteRead();
            this.LocationWriteRead = new LocationWriteRead();
            this.ConveyorWriteRead = new ConveyorWriteRead();
            this.OriginWriteRead = new OriginWriteRead();
            this.DestinationWriteRead = new DestinationWriteRead();
            this.rRouteSequenceWriteRead = new rRouteSequenceWriteRead();
        }

        // GET: Router
        public ActionResult Index()
        {
            var ieList = this.rRouteOxDWriteRead.All();
            var oxds = from oxd in this.OxDWriteRead.All() select new Domain.Type {
                Name = oxd.Origin.Alias + " x " + oxd.Destination.Alias, Id = oxd.Id};
            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            RouteViewModel model = new RouteViewModel { Routes = ieList, OxDs = oxds, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(RouteViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.rRouteOxDWriteRead.Delete(model.Id);
                    this.RouteWriteRead.Delete(model.Id); 
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
                            this.rRouteOxDWriteRead.Delete(model.Selecteds[i].id);
                            this.RouteWriteRead.Delete(model.Selecteds[i].id);
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
            var oxds = from oxd in this.OxDWriteRead.All()
                       select new Domain.Type
                       {
                           Name = oxd.Location.Alias,
                           Id = oxd.Id
                       };
            return PartialView(new RouteViewModel { OxDs = oxds });
        }

        // GET: Asset/Edit/5
        public ActionResult Copy(int id)
        {
            var _Route = (from Route in this.RouteWriteRead.All().Where(p => p.Id.Equals(id))
                         select Route).First();
            RouteViewModel model = new RouteViewModel {
                //Description = _Route.Description, Reduced = _Route.Reduced,
                Id = _Route.Id };
            return PartialView("Create",model);
        }

        [HttpPost]
        public ActionResult Create(RouteViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var routemodel = this.RouteWriteRead.All().OrderByDescending(p => p.Id).First().Location;
                    this.LocationWriteRead.Save(new Tops.Domain.Location
                    {
                        Description = model.Description, Alias = model.Reduced, ParentId= routemodel.ParentId, TypeId=routemodel.TypeId, 
                    });

                    this.RouteWriteRead.Save(new Tops.Domain.Route {
                        Id = this.LocationWriteRead.All().OrderByDescending(p => p.Id).First().Id });
                    var _Route = this.RouteWriteRead.All().OrderByDescending(p=>p.Id).First();
                    this.rRouteOxDWriteRead.Save(new rRouteOxD { OxDId = model.OxDId, Id = _Route.Id });
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
            var _Route = (from rot in this.LocationWriteRead.All().Where(p => p.Id.Equals(id))
                          select rot).First();
            var oxdId = this.rRouteOxDWriteRead.All().Where(p => p.Id.Equals(id)).First().OxDId;
            var oxds = from oxd in this.OxDWriteRead.All()
                       select new Domain.Type
                       {
                           Name = oxd.Origin.Alias + " x " + oxd.Destination.Alias,
                           Id = oxd.Id
                       };
            RouteViewModel model = new RouteViewModel { OxDs = oxds,
                Description = _Route.Description, Reduced = _Route.Alias,
                Id = _Route.Id, OxDId = oxdId};
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Edit(RouteViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var loc = this.LocationWriteRead.All().Where(p => p.Id== model.Id).First();
                    loc.Description = model.Description;
                    loc.Alias = model.Reduced;
                    this.LocationWriteRead.Edit(loc);
                    this.rRouteOxDWriteRead.Edit(new rRouteOxD { OxDId = model.OxDId });
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

        // GET: Asset/Edit/5
        public ActionResult Activate(int id)
        { 
            RouteViewModel model = new RouteViewModel { Id = id};
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Activate(RouteViewModel model)
        {
            try
            {                
                this.rRouteActiveWriteRead.Save(new rRouteActive { Id = model.Id, dh = DateTime.Now });
                log.Info("Modificado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                Session["Message"] = DateTime.Now.ToString() + " - " + "Modificado cadastro do registro " + model.Id.ToString();
                return RedirectToAction("Index");                
            }
            catch (Exception ex)
            {
                Session["Message"] = DateTime.Now.ToString() + " - " + "Erro: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Asset/Edit/5
        public ActionResult Deactivate(int id)
        {
            RouteViewModel model = new RouteViewModel { Id = id };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Deactivate(RouteViewModel model)
        {
            try
            {
                this.rRouteActiveWriteRead.Delete(model.Id);
                log.Info("Modificado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                Session["Message"] = DateTime.Now.ToString() + " - " + "Modificado cadastro do registro " + model.Id.ToString();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Session["Message"] = DateTime.Now.ToString() + " - " + "Erro: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Router
        public ActionResult Log()
        {
            var log = from lg in this.LogWriteRead.All()
                      join rt in this.RouteWriteRead.All() on lg.LocationId equals rt.Id
                      select lg;

            RouteViewModel model = new RouteViewModel { logs = log, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = log.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Log", model);
        }

        public ActionResult Sequence(int id)
        {
            var _seq = from seq in this.rRouteSequenceWriteRead.All().Where(p => p.RouteId == id) 
                       join loc in this.LocationWriteRead.All() on seq.LocationId equals loc.Id
                       select new RouteSequence { Id = seq.Id, LocationId = seq.LocationId, Name = loc.Alias, Order = seq.Order, RouteId = seq.RouteId  };
            _seq = _seq.DistinctBy(p => p.Name);
            RouteViewModel model = new RouteViewModel { Id = id, Sequences = _seq, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = _seq.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Sequence", model);
        }

        [HttpPost]
        public ActionResult Sequence(RouteViewModel model)
        {
            try
            {                
                    this.rRouteSequenceWriteRead.Delete(model.Id);
                    log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Deletado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("Sequence");               
            }
            catch (Exception ex)
            {
                Session["Message"] = DateTime.Now.ToString() + " - " + "Provavel erro de dependência de dados.";
                return RedirectToAction("Index");
            }
        }

        // GET: Asset/Edit/5
        public ActionResult AddEqp(int id)
        {
            var conveyor = from loc in this.LocationWriteRead.All()
                           join conv in this.ConveyorWriteRead.All() on loc.Id equals conv.Id
                           select loc;

            var origin = from loc in this.LocationWriteRead.All()
                           join or in this.OriginWriteRead.All() on loc.Id equals or.Id
                           select loc;

            var destination = from loc in this.LocationWriteRead.All()
                         join des in this.DestinationWriteRead.All() on loc.Id equals des.Id
                         select loc;

            RouteViewModel model = new RouteViewModel
            {
                Id = id,
                eqps = conveyor.Union(origin).Union(destination),
                Description = ""
            };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult AddEqp(RouteViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rRouteSequenceWriteRead.Save(new rRouteSequence { LocationId = model.LocationId, Order = model.Order, RouteId = model.Id });
                    log.Info("Modificado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Modificado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("Sequence/"+model.Id.ToString());
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
                return RedirectToAction("Sequence/" + model.Id.ToString());
            }
        }

    }

    public class RouteSequence
    {
        public long Id { get; set; }

        public int Order { get; set; }

        public long LocationId { get; set; }

        public string Name { get; set; }

        public long RouteId { get; set; }

    }


}
