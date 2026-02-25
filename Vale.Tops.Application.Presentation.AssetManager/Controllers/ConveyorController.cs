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
    public class ConveyorController : Controller
    {
        private ILocationWriteRead LocationWriteRead;
        private IConveyorWriteRead ConveyorWriteRead;
        private IrConveyorLenghtWriteRead rConveyorLenghtWriteRead;
        private IrConveyorConsumptionWriteRead rConveyorConsumptionWriteRead;
        private IrConveyorCapacityWriteRead rConveyorCapacityWriteRead;
        private IrConveyorSpeedWriteRead rConveyorSpeedWriteRead;
        private IrConveyorOutputPositionWriteRead rConveyorOutputPositionWriteRead;
        private IrConveyorInputPositionWriteRead rConveyorInputPositionWriteRead;
        private IrConveyorScaleWriteRead rConveyorScaleWriteRead;


        ILog log = LogManager.GetLogger("Admin Cadastro Location");
        public ConveyorController ()
        {
            this.LocationWriteRead = new LocationWriteRead();
            this.ConveyorWriteRead = new ConveyorWriteRead();
            this.rConveyorLenghtWriteRead = new rConveyorLenghtWriteRead();
            this.rConveyorSpeedWriteRead = new rConveyorSpeedWriteRead();
            this.rConveyorCapacityWriteRead = new rConveyorCapacityWriteRead();
            this.rConveyorConsumptionWriteRead = new rConveyorConsumptionWriteRead();
            this.rConveyorOutputPositionWriteRead = new rConveyorOutputPositionWriteRead();
            this.rConveyorInputPositionWriteRead = new rConveyorInputPositionWriteRead();
            this.rConveyorScaleWriteRead = new rConveyorScaleWriteRead();
        }

        // GET: Locationr
        public ActionResult Index()
        {
            var ieList = this.ConveyorWriteRead.All();
            //var ieList_ = (from Location in this.LocationWriteRead.All()
            //              join conv in con on Location.Id equals conv.Id
            //              select Location);
            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            ConveyorViewModel model = new ConveyorViewModel { Conveyors = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(ConveyorViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.ConveyorWriteRead.Delete(model.Id); 
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
                            this.ConveyorWriteRead.Delete(model.Selecteds[i].id);
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
                          join rem in this.ConveyorWriteRead.All() on Location.Id equals rem.Id
                          select Location);

            return PartialView(new ConveyorViewModel { Locations = ieList.Except(remove) });
        }

        [HttpPost]
        public ActionResult Create(ConveyorViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.ConveyorWriteRead.Save(new Conveyor { Id = model.Id});
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

        public ActionResult Size(int id)
        {
            var size = this.rConveyorLenghtWriteRead.All().FirstOrDefault();

            ConveyorViewModel model = new ConveyorViewModel { Id = id, Capacity = size==null?0:size.Lenght };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Size(ConveyorViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (this.rConveyorLenghtWriteRead.All().Where(p => p.Id == model.Id).Count() > 0)
                        this.rConveyorLenghtWriteRead.Edit(new rConveyorLenght { Id = model.Id, Lenght = model.Capacity });
                    else
                        this.rConveyorLenghtWriteRead.Save(new rConveyorLenght { Id = model.Id, Lenght = model.Capacity });

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

        public ActionResult Velocity(int id)
        {
            var velocity = this.rConveyorSpeedWriteRead.All().FirstOrDefault();

            ConveyorViewModel model = new ConveyorViewModel { Id = id, Speed = velocity == null ? 0 : velocity.Speed };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Velocity(ConveyorViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (this.rConveyorSpeedWriteRead.All().Where(p => p.Id == model.Id).Count() > 0)
                        this.rConveyorSpeedWriteRead.Edit(new rConveyorSpeed { Id = model.Id, Speed = model.Speed });
                    else
                        this.rConveyorSpeedWriteRead.Save(new rConveyorSpeed { Id = model.Id, Speed = model.Speed });

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

        public ActionResult Consumption(int id)
        {
            var consumption = this.rConveyorConsumptionWriteRead.All().FirstOrDefault();

            ConveyorViewModel model = new ConveyorViewModel { Id = id, Consumption = consumption == null ? 0 : consumption.Consumption };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Consumption(ConveyorViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (this.rConveyorConsumptionWriteRead.All().Where(p => p.Id == model.Id).Count() > 0)
                        this.rConveyorConsumptionWriteRead.Edit(new rConveyorConsumption { Id = model.Id, Consumption = model.Consumption });
                    else
                        this.rConveyorConsumptionWriteRead.Save(new rConveyorConsumption { Id = model.Id, Consumption = model.Consumption });

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

        public ActionResult Limit(int id)
        {
            var Limit = this.rConveyorCapacityWriteRead.All().FirstOrDefault();

            ConveyorViewModel model = new ConveyorViewModel { Id = id, Limit = Limit == null ? 0 : Limit.Capacity};
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Limit(ConveyorViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (this.rConveyorCapacityWriteRead.All().Where(p => p.Id == model.Id).Count() > 0)
                        this.rConveyorCapacityWriteRead.Edit(new rConveyorCapacity { Id = model.Id, Capacity = model.Limit });
                    else
                        this.rConveyorCapacityWriteRead.Save(new rConveyorCapacity { Id = model.Id, Capacity = model.Limit });

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

        public ActionResult OutputPosition(int id)
        {
            //var seq = from in this.ConveyorWriteRead.All().Except(this.rConveyorOutputPositionWriteRead.All())
            var _seq = from seq in this.rConveyorOutputPositionWriteRead.All().Where(p => p.LocationId == id)
                       join loc in this.ConveyorWriteRead.All() on seq.LocationId equals loc.Id
                       select new ConveyorOutputPosition { Id = seq.Id, LocationId = seq.LocationId.Value, Name = loc.Location.Alias, Position = seq.Position, ConveyorId = seq.LocationId.Value };
            _seq = _seq.DistinctBy(p => p.Name);
            ConveyorViewModel model = new ConveyorViewModel { Id = id, OutputPositions = _seq, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = _seq.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("OutputPosition", model);
        }

        [HttpPost]
        public ActionResult OutputPosition(ConveyorViewModel model)
        {
            try
            {
                this.rConveyorOutputPositionWriteRead.Delete(model.Id);
                log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                Session["Message"] = DateTime.Now.ToString() + " - " + "Deletado cadastro do registro " + model.Id.ToString();
                return RedirectToAction("OutputPosition");
            }
            catch (Exception ex)
            {
                Session["Message"] = DateTime.Now.ToString() + " - " + "Provavel erro de dependência de dados.";
                return RedirectToAction("Index");
            }
        }

        public ActionResult InputPosition(int id)
        {
            var _seq = from seq in this.rConveyorInputPositionWriteRead.All().Where(p => p.LocationAId == id)
                       join loc in this.LocationWriteRead.All() on seq.LocationAId equals loc.Id
                       select new ConveyorInputPosition { Id = seq.Id, LocationId = seq.LocationAId.Value, Name = loc.Alias, Position = seq.Position, ConveyorId = seq.LocationAId.Value };
            _seq = _seq.DistinctBy(p => p.Name);
            ConveyorViewModel model = new ConveyorViewModel { Id = id, InputPositions = _seq, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = _seq.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("InputPosition", model);
        }

        [HttpPost]
        public ActionResult InputPosition(ConveyorViewModel model)
        {
            try
            {
                this.rConveyorInputPositionWriteRead.Delete(model.Id);
                log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                Session["Message"] = DateTime.Now.ToString() + " - " + "Deletado cadastro do registro " + model.Id.ToString();
                return RedirectToAction("InputPosition");
            }
            catch (Exception ex)
            {
                Session["Message"] = DateTime.Now.ToString() + " - " + "Provavel erro de dependência de dados.";
                return RedirectToAction("Index");
            }
        }

        public ActionResult AddInput(int id)
        {
            var conveyor = from loc in this.LocationWriteRead.All()
                           join conv in this.ConveyorWriteRead.All() on loc.Id equals conv.Id
                           select loc;


            ConveyorViewModel model = new ConveyorViewModel
            {
                Id = id,
                Inputs = conveyor,
                Description = ""
            };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult AddInput(ConveyorViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rConveyorInputPositionWriteRead.Save(new rConveyorInputPosition { LocationAId = model.LocationId, Position = model.Position});
                    log.Info("Modificado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Modificado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("InputPosition/" + model.Id.ToString());
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
                return RedirectToAction("InputPosition/" + model.Id.ToString());
            }
        }

        public ActionResult AddOutput(int id)
        {
            var conveyor = from loc in this.LocationWriteRead.All()
                           join conv in this.ConveyorWriteRead.All() on loc.Id equals conv.Id
                           select loc;


            ConveyorViewModel model = new ConveyorViewModel
            {
                Id = id,
                Outputs = conveyor,
                Description = ""
            };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult AddOutput(ConveyorViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rConveyorOutputPositionWriteRead.Save(new rConveyorOutputPosition {LocationId = model.LocationId, Position = model.Position });
                    log.Info("Modificado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Modificado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("OutputPosition/" + model.Id.ToString());
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
                return RedirectToAction("OutputPosition/" + model.Id.ToString());
            }
        }

    }
    public class ConveyorOutputPosition
    {
        public long Id { get; set; }

        public double Position { get; set; }

        public long LocationId { get; set; }

        public string Name { get; set; }

        public long ConveyorId { get; set; }

    }

    public class ConveyorInputPosition
    {
        public long Id { get; set; }

        public double Position { get; set; }

        public long LocationId { get; set; }

        public string Name { get; set; }

        public long ConveyorId { get; set; }

    }
}
