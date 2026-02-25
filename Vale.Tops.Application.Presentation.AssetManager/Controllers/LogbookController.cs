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
using Vale.Tops.Integration.Infrastructure.AD;

namespace Vale.Tops.Application.Presentation.AssetManager.Controllers
{
    public class LogbookController : Controller
    {
        private ILogbookWriteRead LogbookWriteRead;
        private ILocationWriteRead LocationWriteRead;
        private IHistoryWriteRead HistoryWriteRead;
        private IrLogbookHistoryWriteRead rLogbookHistoryWriteRead;
        private IrLogbookEPEEWriteRead rLogbookEPEEWriteRead;
        private IrLogbookVVWriteRead rLogbookVVWriteRead;

        ILog log = LogManager.GetLogger("Admin Cadastro Diario de bordo");
        public LogbookController()
        {
            this.LogbookWriteRead = new LogbookWriteRead();
            this.LocationWriteRead = new LocationWriteRead();
            this.HistoryWriteRead = new HistoryWriteRead();
            this.rLogbookHistoryWriteRead = new rLogbookHistoryWriteRead();
            this.rLogbookEPEEWriteRead = new rLogbookEPEEWriteRead();
            this.rLogbookVVWriteRead = new rLogbookVVWriteRead();
        }

        public ActionResult IndexRC(LogbookRCViewModel model)
        {
            var ieList = (from location in this.LocationWriteRead.All().Where(p => p.TypeId == 207 | p.TypeId == 112)
                          select new Tops.Domain.Location
                          {
                              Description = location.Description,
                              Id = location.Id,
                              Name = location.Name,
                              Alias = location.Alias
                          });

            DateTime? dhi = model.dhpi == null ? (DateTime.Now.AddDays(-3)).Date : model.dhpi;
            DateTime? dhf = model.dhpf == null ? (DateTime.Now.AddDays(1)).Date : model.dhpf;

            var ie = this.LogbookWriteRead.All().ToList().Where(p => p.dh >= dhi && p.dh <= dhf && (p.Location.TypeId == 207 | p.Location.TypeId == 112));

            if (model.LocationId == null)
            {
                ie = ie.OrderByDescending(p => p.dh).ToList();
            }
            else
            {
                model.Asset = this.LocationWriteRead.All().Where(p => p.Id == model.LocationId).First().Alias;
                ie = ie.Where(p => p.Location.Id == model.LocationId).OrderByDescending(p => p.dh).ToList();
            }

            if (model.LocationId != null)
                model.Asset = ieList.First(c => c.Id == model.LocationId).Alias;

            model.Logbooks = ie;
            model.dhpi = dhi;
            model.dhpf = dhf;
            model.Locations = ieList;
            model.lines = ie == null ? 0 : ie.Count();
            model.Message = DateTime.Now.ToString() + " - " + "Lista acessada.";

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return PartialView("IndexRC", model);
        }

        public ActionResult CreateRC()
        {
            var ielocation = (from location in this.LocationWriteRead.All().Where(p => p.TypeId == 207 | p.TypeId == 112)
                              select new Location
                              {
                                  Description = location.Description,
                                  Id = location.Id,
                                  Name = location.Name,
                                  Alias = location.Alias
                              });
            LogbookRCViewModel model = new LogbookRCViewModel { Locations = ielocation };
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult CreateRC(LogbookRCViewModel model)
        {
            try
            {
                var ie = (from location in this.LocationWriteRead.All().Where(p => p.Id == model.LocationId)
                          select new Location
                          {
                              Description = location.Description,
                              Id = location.Id,
                              Name = location.Name,
                              Alias = location.Alias
                          });

                if (ModelState.IsValid)
                {
                    var logbook = new Domain.Logbook
                    {
                        Id = Guid.NewGuid(),
                        LocationId = model.LocationId,
                        dh = model.dh,
                        User = model.User,
                        Asset = ie.First().Alias,
                        AreaSelected = model.Area,
                        hi = model.hi,
                        mi = model.mi,
                        hf = model.hf,
                        mf = model.mf,
                        Cod = model.Cod,
                        Conjunct = model.Conjunct,
                        ShipCradle = model.ShipCradle,
                        Observation = model.Observation,
                        Quantity = model.Quantity,
                        WeightAsset = model.WeightAsset,
                        WeightConjunct = model.WeightConjunct,
                        Hold = model.Hold,
                        Material = model.Material,
                        FlowMeasured = model.FlowMeasured,
                        FlowReal = model.FlowReal,
                        LandmarkFinal = model.LandmarkFinal,
                        LandmarkInitial = model.LandmarkInitial,
                        isFail = model.isFail
                    };

                    this.LogbookWriteRead.Save(logbook);

                    log.Info("Cadastro de novo registro realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Cadastro de novo registro realizado pelo usuário ";
                    return RedirectToAction("IndexRC");
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
                return RedirectToAction("IndexRC");
            }
        }

        // GET: Asset/Edit/5
        public ActionResult EditRC(Guid id)
        {
            var ielocation = (from location in this.LocationWriteRead.All().Where(p => p.TypeId == 207 | p.TypeId == 112)
                              select new Location
                              {
                                  Description = location.Description,
                                  Id = location.Id,
                                  Name = location.Name,
                                  Alias = location.Alias
                              });
            var _Logbook = (from Logbook in this.LogbookWriteRead.All().Where(p => p.Id.Equals(id))
                            select Logbook).First();

            LogbookRCViewModel model = new LogbookRCViewModel
            {
                Hold = _Logbook.Hold!=null ? _Logbook.Hold : 0,
                dh = _Logbook.dh,
                ShipCradle = _Logbook.ShipCradle != null ? _Logbook.ShipCradle : 0,
                Observation = _Logbook.Observation,
                LocationId = _Logbook.LocationId,
                Asset = _Logbook.Asset,
                User = _Logbook.User,
                Id = _Logbook.Id,
                FlowMeasured = _Logbook.FlowMeasured != null ? _Logbook.FlowMeasured : 0,
                FlowReal = _Logbook.FlowReal != null ? _Logbook.FlowReal : 0,
                hf = _Logbook.hf != null ? _Logbook.hf : 0,
                mf = _Logbook.mf != null ? _Logbook.mf : 0,
                hi = _Logbook.hi != null ? _Logbook.hi : 0,
                mi = _Logbook.mi != null ? _Logbook.mi : 0,
                Quantity = _Logbook.Quantity != null ? _Logbook.Quantity : 0,
                WeightAsset = _Logbook.WeightAsset != null ? _Logbook.WeightAsset : 0,
                WeightConjunct = _Logbook.WeightConjunct != null ? _Logbook.WeightConjunct : 0,
                Material = _Logbook.Material,
                Cod = _Logbook.Cod,
                Conjunct = _Logbook.Conjunct,
                Area = _Logbook.AreaSelected,
                LandmarkInitial = _Logbook.LandmarkInitial != null ? _Logbook.LandmarkInitial : 0,
                LandmarkFinal = _Logbook.LandmarkFinal != null ? _Logbook.LandmarkFinal : 0,
                isFail = _Logbook.isFail== true? true: false,
                Locations = ielocation
            };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult EditRC(LogbookRCViewModel model)
        {
            try
            {
                var ie = (from location in this.LocationWriteRead.All().Where(p => p.Id == model.LocationId)
                          select new Location
                          {
                              Description = location.Description,
                              Id = location.Id,
                              Name = location.Name,
                              Alias = location.Alias
                          });
                var change = this.LogbookWriteRead.All().Where(p => p.Id == model.Id).First();
                if (ModelState.IsValid)
                {
                    change.Hold = model.Hold;
                    change.ShipCradle = model.ShipCradle;
                    change.Observation = model.Observation;
                    change.FlowMeasured = model.FlowMeasured;
                    change.FlowReal = model.FlowReal;
                    change.hf = model.hf;
                    change.mf = model.mf;
                    change.hi = model.hi;
                    change.mi = model.mi;
                    change.Quantity = model.Quantity;
                    change.WeightAsset = model.WeightAsset;
                    change.WeightConjunct = model.WeightConjunct;
                    change.Material = model.Material;
                    change.Cod = model.Cod;
                    change.Conjunct = model.Conjunct;
                    change.AreaSelected = model.Area;
                    change.LandmarkInitial = model.LandmarkInitial;
                    change.LandmarkFinal = model.LandmarkFinal;
                    change.isFail = model.isFail;
                    this.LogbookWriteRead.Edit(change);
                    RegHistory(User.Identity.Name, "Editou o diário de bordo", change.Id);
                    log.Info("Modificado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Modificado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("IndexRC");
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
                return RedirectToAction("IndexRC");
            }
        }

        // GET: Asset/Edit/5
        public ActionResult Delete(Guid id, string Index)
        {
            LogbookViewModel model = new LogbookViewModel
            {
                Id = id,
                IndexPage = Index
            };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Delete(LogbookViewModel model)
        {
            try
            {

                this.LogbookWriteRead.Delete(model.Id);
                log.Info("Deletado registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                Session["Message"] = DateTime.Now.ToString() + " - " + "Deletado registro " + model.Id.ToString();
                return RedirectToAction(model.IndexPage);

            }
            catch (Exception ex)
            {
                Session["Message"] = DateTime.Now.ToString() + " - " + "Erro: " + ex.Message;
                return RedirectToAction(model.IndexPage);
            }
        }

        public ActionResult IndexEP(LogbookEPViewModel model)
        {
            var ieList = (from location in this.LocationWriteRead.All().Where(p => p.TypeId == 111 | p.TypeId == 112)
                          select new Tops.Domain.Location
                          {
                              Description = location.Description,
                              Id = location.Id,
                              Name = location.Name,
                              Alias = location.Alias
                          });

            DateTime? dhi = model.dhpi == null ? (DateTime.Now.AddDays(-3)).Date : model.dhpi;
            DateTime? dhf = model.dhpf == null ? (DateTime.Now.AddDays(1)).Date : model.dhpf;

            var ie = this.LogbookWriteRead.All().ToList().Where(p => p.dh >= dhi && p.dh <= dhf && (p.Location.TypeId == 111 | p.Location.TypeId == 112));

            if (model.LocationId == null)
            {
                ie = ie.OrderByDescending(p => p.dh).ToList();
            }
            else
            {
                model.Asset = this.LocationWriteRead.All().Where(p => p.Id == model.LocationId).First().Alias;
                ie = ie.Where(p => p.Location.Id == model.LocationId).OrderByDescending(p => p.dh).ToList();
            }

            if (model.LocationId != null)
                model.Asset = ieList.First(c => c.Id == model.LocationId).Alias;

            model.Logbooks = ie;
            model.dhpi = dhi;
            model.dhpf = dhf;
            model.Locations = ieList;
            model.lines = ie == null ? 0 : ie.Count();
            model.Message = DateTime.Now.ToString() + " - " + "Lista acessada.";

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return PartialView("IndexEP", model);
        }

        // GET: Asset/Create
        public ActionResult CreateEP()
        {
            var ielocation = (from location in this.LocationWriteRead.All().Where(p => p.TypeId == 111 | p.TypeId == 112)
                              select new Location
                              {
                                  Description = location.Description,
                                  Id = location.Id,
                                  Name = location.Name,
                                  Alias = location.Alias
                              });
            LogbookEPViewModel model = new LogbookEPViewModel { Locations = ielocation };
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult CreateEP(LogbookEPViewModel model)
        {
            try
            {
                var ie = (from location in this.LocationWriteRead.All().Where(p => p.Id == model.LocationId)
                          select new Location
                          {
                              Description = location.Description,
                              Id = location.Id,
                              Name = location.Name,
                              Alias = location.Alias
                          });


                if (ModelState.IsValid)
                {
                    var logbook = new Domain.Logbook
                    {
                        Id = Guid.NewGuid(),
                        LocationId = model.LocationId,
                        dh = model.dh,
                        User = model.User,
                        Asset = ie.First().Alias,
                        AreaSelected = model.Area,
                        hi = model.hi,
                        mi = model.mi,
                        hf = model.hf,
                        mf = model.mf,
                        Cod = model.Cod,
                        Conjunct = model.Conjunct,
                        LotNumber = model.LotNumber,
                        LotPrefix = model.LotPrefix,
                        Observation = model.Observation,
                        LotSLandmarkFinal = model.LotSLandmarkFinal,
                        LotSLandmarkInitial = model.LotSLandmarkInitial,
                        RestrictRoute = model.RestrictRoute,
                        StackEnd = model.StackEnd,
                        Material = model.Material,
                        FlowObservation = model.FlowObservation,
                        MaterialQuality = model.MaterialQuality,
                        LandmarkFinal = model.LandmarkFinal,
                        LandmarkInitial = model.LandmarkInitial,
                        IsHopper = model.IsHopper,
                        StackingWay = model.StackingWay,
                        NumberOfWagons = model.NumberOfWagons,
                        hP0 = model.hP0,
                        mP0 = model.mP0,
                        hAcoplado = model.hAcoplado,
                        mAcoplado = model.mAcoplado,
                        hUsina = model.hUsina,
                        mUsina = model.mUsina,
                        Steps = model.Steps,
                        LowFlowOrParalization = model.LowFlowOrParalization,
                        OutOfStack = model.OutOfStack
                    };

                    this.LogbookWriteRead.Save(logbook);

                    log.Info("Cadastro de novo registro realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Cadastro de novo registro realizado pelo usuário ";
                    return RedirectToAction("IndexEP");
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
                return RedirectToAction("IndexEP");
            }
        }

        // GET: Asset/Edit/5
        public ActionResult EditEP(Guid id)
        {
            var ielocation = (from location in this.LocationWriteRead.All().Where(p => p.TypeId == 111 | p.TypeId == 112)
                              select new Location
                              {
                                  Description = location.Description,
                                  Id = location.Id,
                                  Name = location.Name,
                                  Alias = location.Alias
                              });
            var _Logbook = (from Logbook in this.LogbookWriteRead.All().Where(p => p.Id.Equals(id))
                            select Logbook).First();
            LogbookEPViewModel model = new LogbookEPViewModel
            {
                Id = _Logbook.Id,
                LocationId = _Logbook.LocationId,
                dh = _Logbook.dh,
                User = _Logbook.User,
                Asset = _Logbook.Asset,
                Area = _Logbook.AreaSelected,
                hi = _Logbook.hi != null ? _Logbook.hi : 0,
                mi = _Logbook.mi != null ? _Logbook.mi : 0,
                hf = _Logbook.hf != null ? _Logbook.hf : 0,
                mf = _Logbook.mf != null ? _Logbook.mf : 0,
                Cod = _Logbook.Cod,
                Conjunct = _Logbook.Conjunct,
                LotNumber = _Logbook.LotNumber != null ? _Logbook.LotNumber : 0,
                LotPrefix = _Logbook.LotPrefix != null ? _Logbook.LotPrefix : 0,
                Observation = _Logbook.Observation,
                LotSLandmarkFinal = _Logbook.LotSLandmarkFinal != null ? _Logbook.LotSLandmarkFinal : 0,
                LotSLandmarkInitial = _Logbook.LotSLandmarkInitial != null ? _Logbook.LotSLandmarkInitial : 0,
                RestrictRoute = _Logbook.RestrictRoute == true ? true : false,
                StackEnd = _Logbook.StackEnd == true ? true : false,
                Material = _Logbook.Material,
                FlowObservation = _Logbook.FlowObservation,
                MaterialQuality = _Logbook.MaterialQuality == true ? true : false,
                LandmarkFinal = _Logbook.LandmarkFinal != null ? _Logbook.LandmarkFinal : 0,
                LandmarkInitial = _Logbook.LandmarkInitial != null ? _Logbook.LandmarkInitial : 0,
                IsHopper = _Logbook.IsHopper == true ? true : false,
                StackingWay = _Logbook.StackingWay,
                NumberOfWagons = _Logbook.NumberOfWagons != null ? _Logbook.NumberOfWagons : 0,
                hP0 = _Logbook.hP0 != null ? _Logbook.hP0 : 0,
                mP0 = _Logbook.mP0 != null ? _Logbook.mP0 : 0,
                hAcoplado = _Logbook.hAcoplado != null ? _Logbook.hAcoplado : 0,
                mAcoplado = _Logbook.mAcoplado != null ? _Logbook.mAcoplado : 0,
                hUsina = _Logbook.hUsina != null ? _Logbook.hUsina : 0,
                mUsina = _Logbook.mUsina != null ? _Logbook.mUsina : 0,
                Steps = _Logbook.Steps != null ? _Logbook.Steps : 0,
                LowFlowOrParalization = _Logbook.LowFlowOrParalization == true ? true : false,
                OutOfStack = _Logbook.OutOfStack,
                Locations = ielocation
            };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult EditEP(LogbookEPViewModel model)
        {
            try
            {
                var ie = (from location in this.LocationWriteRead.All().Where(p => p.Id == model.LocationId)
                          select location);
                var change = this.LogbookWriteRead.All().Where(p => p.Id == model.Id).First();
                if (ModelState.IsValid)
                {
                    change.AreaSelected = model.Area;
                    change.hi = model.hi;
                    change.mi = model.mi;
                    change.hf = model.hf;
                    change.mf = model.mf;
                    change.Cod = model.Cod;
                    change.Conjunct = model.Conjunct;
                    change.LotNumber = model.LotNumber;
                    change.LotPrefix = model.LotPrefix;
                    change.Observation = model.Observation;
                    change.LotSLandmarkFinal = model.LotSLandmarkFinal;
                    change.LotSLandmarkInitial = model.LotSLandmarkInitial;
                    change.RestrictRoute = model.RestrictRoute;
                    change.StackEnd = model.StackEnd;
                    change.Material = model.Material;
                    change.FlowObservation = model.FlowObservation;
                    change.MaterialQuality = model.MaterialQuality;
                    change.LandmarkFinal = model.LandmarkFinal;
                    change.LandmarkInitial = model.LandmarkInitial;
                    change.IsHopper = model.IsHopper;
                    change.StackingWay = model.StackingWay;
                    change.NumberOfWagons = model.NumberOfWagons;
                    change.hP0 = model.hP0;
                    change.mP0 = model.mP0;
                    change.hAcoplado = model.hAcoplado;
                    change.mAcoplado = model.mAcoplado;
                    change.hUsina = model.hUsina;
                    change.mUsina = model.mUsina;
                    change.Steps = model.Steps;
                    change.LowFlowOrParalization = model.LowFlowOrParalization;
                    change.OutOfStack = model.OutOfStack;
                    this.LogbookWriteRead.Edit(change);
                    RegHistory(User.Identity.Name, "Editou o diário de bordo", change.Id);
                    log.Info("Modificado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Modificado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("IndexEP");
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
                return RedirectToAction("IndexEP");
            }
        }       

        public ActionResult IndexEPEE(LogbookEPEEViewModel model)
        {
            var ieList = (from location in this.LocationWriteRead.All().Where(p => p.TypeId == 111 | p.TypeId == 112)
                          select new Tops.Domain.Location
                          {
                              Description = location.Description,
                              Id = location.Id,
                              Name = location.Name,
                              Alias = location.Alias
                          });

            DateTime? dhi = model.dhpi == null ? (DateTime.Now.AddDays(-3)).Date : model.dhpi;
            DateTime? dhf = model.dhpf == null ? (DateTime.Now.AddDays(1)).Date : model.dhpf;

            var ie = this.rLogbookEPEEWriteRead.All().Where(p => p.Logbook.dh >= dhi && p.Logbook.dh <= dhf && (p.Logbook.Location.TypeId == 111 | p.Logbook.Location.TypeId == 112));

            if (model.LocationId == null)
            {
                ie = ie.OrderByDescending(p => p.Logbook.dh).ToList();
            }
            else
            {
                model.Asset = this.LocationWriteRead.All().Where(p => p.Id == model.LocationId).First().Alias;
                ie = ie.Where(p => p.Logbook.Location.Id == model.LocationId).OrderByDescending(p => p.Logbook.dh).ToList();
            }

            if (model.LocationId != null)
                model.Asset = ieList.First(c => c.Id == model.LocationId).Alias;

            model.Logbookepees = ie;
            model.dhpi = dhi;
            model.dhpf = dhf;
            model.Locations = ieList;
            model.lines = ie == null ? 0 : ie.Count();
            model.Message = DateTime.Now.ToString() + " - " + "Lista acessada.";

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return PartialView("IndexEPEE", model);
        }

        [AllowAnonymous]
        public ActionResult PowerBiEPEE(LogbookEPEEViewModel model, string Asset, DateTime pdhi, DateTime pdhf)
        {
           

            DateTime? dhi = pdhi == null ? (DateTime.Now.AddDays(-3)).Date : pdhi;
            DateTime? dhf = pdhf == null ? (DateTime.Now.AddDays(1)).Date : pdhf;

            var ie = this.rLogbookEPEEWriteRead.All().Where(p => p.Logbook.dh >= dhi && p.Logbook.dh <= dhf && (p.Logbook.Location.TypeId == 111 | p.Logbook.Location.TypeId == 112) 
            & p.Logbook.Location.Alias == Asset);

            if (model.LocationId == null)
            {
                ie = ie.OrderByDescending(p => p.Logbook.dh).ToList();
            }
            else
            {
                model.Asset = Asset;// this.LocationWriteRead.All().Where(p => p.Id == model.LocationId).First().Alias;
                ie = ie.Where(p => p.Logbook.Location.Alias == Asset).OrderByDescending(p => p.Logbook.dh).ToList();
            }

            if (model.LocationId != null)
                model.Asset = Asset;

            model.Logbookepees = ie;
            model.dhpi = dhi;
            model.dhpf = dhf;
            //model.Locations = ieList;
            model.lines = ie == null ? 0 : ie.Count();
            model.Message = DateTime.Now.ToString() + " - " + "Lista acessada.";

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return PartialView("PowerBiEPEE", model);
        }

        public ActionResult IndexVV(LogbookVVViewModel model)
        {
            var ieList = (from location in this.LocationWriteRead.All().Where(p => p.Type.Id == 275)
                          select new Tops.Domain.Location
                          {
                              Description = location.Description,
                              Id = location.Id,
                              Name = location.Name,
                              Alias = location.Alias
                          });

            DateTime? dhi = model.dhpi == null ? (DateTime.Now.AddDays(-3)).Date : model.dhpi;
            DateTime? dhf = model.dhpf == null ? (DateTime.Now.AddDays(1)).Date : model.dhpf;

            var ie = this.rLogbookVVWriteRead.All().Where(p => p.Logbook.dh >= dhi && p.Logbook.dh <= dhf && (p.Logbook.Location.TypeId == 275));

            if (model.LocationId == null)
            {
                ie = ie.OrderByDescending(p => p.Logbook.dh).ToList();
            }
            else
            {
                model.Asset = this.LocationWriteRead.All().Where(p => p.Id == model.LocationId).First().Alias;
                ie = ie.Where(p => p.Logbook.Location.Id == model.LocationId).OrderByDescending(p => p.Logbook.dh).ToList();
            }

            if (model.LocationId != null)
                model.Asset = ieList.First(c => c.Id == model.LocationId).Alias;

            model.Logbookvvs = ie;
            model.dhpi = dhi;
            model.dhpf = dhf;
            model.Locations = ieList;
            model.lines = ie == null ? 0 : ie.Count();
            model.Message = DateTime.Now.ToString() + " - " + "Lista acessada.";

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return PartialView("IndexVV", model);
        }


        public PartialViewResult History(Guid id)
        {
            var ie = (from history in this.HistoryWriteRead.All().OrderBy(p => p.dh)
                      join lb in this.rLogbookHistoryWriteRead.All().Where(p => p.LogbookId.Equals(id)) on history.Id equals lb.HistoryId
                      select history);

            return PartialView(new LogbookViewModel { lines = ie.Count(), Historys = ie });
        }

        public void RegHistory(string _user, string _action, Guid _Id)
        {
            Guid _historyId = Guid.NewGuid();
            this.HistoryWriteRead.Save(new History { Action = _action, dh = DateTime.Now, Id = _historyId, User = _user });
            this.rLogbookHistoryWriteRead.Save(new rLogbookHistory { Id = Guid.NewGuid(), LogbookId = _Id, HistoryId = _historyId });
        }
    }
}
