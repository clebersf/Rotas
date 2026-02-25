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
    public class ProductionController : Controller
    {
        private IProductionWriteRead ProductionWriteRead;
        private IrProductionStockWriteRead rProductionStockWriteRead;
        private IrProductionBoardingWriteRead rProductionBoardingWriteRead;
        private IrProductionOriginWriteRead rProductionOriginWriteRead;
        private IOriginWriteRead OriginWriteRead;
        private IDestinationWriteRead DestinationWriteRead;
        private IStockWriteRead StockWriteRead;
        private IBoardingWriteRead BoardingWriteRead;
        private IBerthWriteRead BerthWriteRead;
        private IrRouteActiveWriteRead rRouteActiveWriteRead;
        private IRouteWriteRead RouteWriteRead;
        private IrProductionRouteWriteRead rProductionRouteWriteRead;
        private IvwProductionBoardingPresentationReadOnly vwProductionBoardingPresentationReadOnly;
        ILog log = LogManager.GetLogger("Admin Cadastro Production");
        public ProductionController ()
        {
            this.ProductionWriteRead = new ProductionWriteRead();
            this.rProductionStockWriteRead = new rProductionStockWriteRead();
            this.rProductionBoardingWriteRead = new rProductionBoardingWriteRead();
            this.rProductionOriginWriteRead = new rProductionOriginWriteRead();
            this.rProductionRouteWriteRead = new rProductionRouteWriteRead();
            this.OriginWriteRead = new OriginWriteRead();
            this.BerthWriteRead = new BerthWriteRead();
            this.DestinationWriteRead = new DestinationWriteRead();
            this.StockWriteRead = new StockWriteRead();
            this.BoardingWriteRead = new BoardingWriteRead();
            this.rRouteActiveWriteRead = new rRouteActiveWriteRead();
            this.RouteWriteRead = new RouteWriteRead();
            this.vwProductionBoardingPresentationReadOnly = new vwProductionBoardingPresentationReadOnly();
        }

        [AllowAnonymous]
        public ActionResult Split()
        {
            var ieList = this.vwProductionBoardingPresentationReadOnly.All().Where(p=>p.Final == false);
            ProductionViewModel model = new ProductionViewModel { vwProductionBoardingPresentations = ieList, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Split",model);
        }

        [AllowAnonymous]
        public ActionResult Tables()
        {
            var ieList = this.vwProductionBoardingPresentationReadOnly.All().Where(p => p.Final == false);
            ProductionViewModel model = new ProductionViewModel { vwProductionBoardingPresentations = ieList, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return PartialView(model);
        }

        [AllowAnonymous]
        public ActionResult History(ProductionViewModel model)
        {
            long i = 0;
            DateTime? dhi = model.dhpi == null ? DateTime.Now.AddDays(-1) : model.dhpi;
            DateTime? dhf = model.dhpf == null ? DateTime.Now.AddHours(23).AddMinutes(59).AddSeconds(59) : model.dhpf;
            model.dhpi = dhi;
            model.dhpf = dhf;
            var ieList = this.vwProductionBoardingPresentationReadOnly.All().Where(p => p.dhi >= model.dhpi.Value &
            p.dhi < model.dhpf.Value & p.Load > 0);
            model.ProductionBoardingPresentations = from rel in ieList.OrderBy(p => p.dhi)
                                                      select new  PresentationProductionBoarding
                                                      { 
                                                        Berth = rel.Berth,
                                                        Cod = i++,
                                                         Compartment = rel.Compartment,
                                                         Destination = rel.Destination,
                                                         dhf = rel.dhf,
                                                         dhi = rel.dhi,
                                                         ERG1 = rel.ERG1,
                                                         ERG2 = rel.ERG2,
                                                         ERG3 = rel.ERG3,
                                                         Final = rel.Final,
                                                         Load = rel.Load,
                                                         Id = rel.Id,
                                                         VVG1 = rel.VVG1,
                                                         VVG2 = rel.VVG2,
                                                         VVG3 = rel.VVG3
                                                         
                                                      };
            model.Message = DateTime.Now.ToString() + " - " + "Lista acessada.";
            model.lines = ieList.Count();

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("History",model);
        }

        // GET: Productionr
        public ActionResult Index()
        {
            var ieList = this.ProductionWriteRead.All();
            List<SelectedGuid> ieSelecteds = new List<SelectedGuid>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedGuid obj = new SelectedGuid();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            ProductionViewModel model = new ProductionViewModel { Productions = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(ProductionViewModel model)
        {
            try
            {
                if (model.Id != Guid.Empty)
                {
                    //this.rProductionBoardingWriteRead.Delete(model.Id);
                    //this.rProductionStockWriteRead.Delete(model.Id);
                    //this.rProductionOriginWriteRead.DeleteQuery(p=>p.ProductionId == model.Id);
                    this.ProductionWriteRead.Delete(model.Id); 
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
                            this.ProductionWriteRead.Delete(model.Selecteds[i].id);
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

        public ActionResult Stock()
        {
            var ieList = this.rProductionStockWriteRead.All();
            List<SelectedGuid> ieSelecteds = new List<SelectedGuid>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedGuid obj = new SelectedGuid();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            ProductionViewModel model = new ProductionViewModel { ProductionStocks = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Stock", model);
        }

        [HttpPost]
        public ActionResult Stock(ProductionViewModel model)
        {
            try
            {
                if (model.Id != Guid.Empty)
                {
                    this.rProductionStockWriteRead.Delete(model.Id);
                    log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Deletado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("Stock");
                }
                else
                {
                    bool none = true;
                    for (int i = 0; i < model.Selecteds.Count(); i++)
                    {
                        if (model.Selecteds[i].isSelected == true)
                        {
                            this.rProductionStockWriteRead.Delete(model.Selecteds[i].id);
                            log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                            none = false;
                        }
                    }
                    if (!none)
                    {
                        Session["Message"] = DateTime.Now.ToString() + " - " + "O(s) registros foram deletados.";
                        return RedirectToAction("Stock");
                    }
                    else
                    {
                        Session["Message"] = DateTime.Now.ToString() + " - " + "Não houveram registros selecionados.";
                        return RedirectToAction("Stock");
                    }
                }
            }
            catch (Exception ex)
            {
                Session["Message"] = DateTime.Now.ToString() + " - " + "Provavel erro de dependência de dados.";
                return RedirectToAction("Stock");
            }
        }

        public ActionResult Boarding()
        {
            var ieList = this.rProductionBoardingWriteRead.All();
            List<SelectedGuid> ieSelecteds = new List<SelectedGuid>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedGuid obj = new SelectedGuid();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            ProductionViewModel model = new ProductionViewModel { ProductionBoardings = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Boarding", model);
        }

        [HttpPost]
        public ActionResult Boarding(ProductionViewModel model)
        {
            try
            {
                if (model.Id != Guid.Empty)
                {
                    this.rProductionBoardingWriteRead.Delete(model.Id);
                    log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Deletado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("Boarding");
                }
                else
                {
                    bool none = true;
                    for (int i = 0; i < model.Selecteds.Count(); i++)
                    {
                        if (model.Selecteds[i].isSelected == true)
                        {
                            this.rProductionBoardingWriteRead.Delete(model.Selecteds[i].id);
                            log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                            none = false;
                        }
                    }
                    if (!none)
                    {
                        Session["Message"] = DateTime.Now.ToString() + " - " + "O(s) registros foram deletados.";
                        return RedirectToAction("Boarding");
                    }
                    else
                    {
                        Session["Message"] = DateTime.Now.ToString() + " - " + "Não houveram registros selecionados.";
                        return RedirectToAction("Boarding");
                    }
                }
            }
            catch (Exception ex)
            {
                Session["Message"] = DateTime.Now.ToString() + " - " + "Provavel erro de dependência de dados.";
                return RedirectToAction("Boarding");
            }
        }

        public ActionResult Origin()
        {

            var ax = this.rProductionBoardingWriteRead.All();

            var ieList = this.rProductionOriginWriteRead.All().ToList();
            List<SelectedGuid> ieSelecteds = new List<SelectedGuid>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedGuid obj = new SelectedGuid();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            ProductionViewModel model = new ProductionViewModel { ProductionOrigins = ieList, ProductionBoardings = ax.ToList(), Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Origin", model);
        }

        [HttpPost]
        public ActionResult Origin(ProductionViewModel model)
        {
            try
            {
                if (model.Id != Guid.Empty)
                {
                    this.rProductionOriginWriteRead.Delete(model.Id);
                    log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Deletado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("Origin");
                }
                else
                {
                    bool none = true;
                    for (int i = 0; i < model.Selecteds.Count(); i++)
                    {
                        if (model.Selecteds[i].isSelected == true)
                        {
                            this.rProductionOriginWriteRead.Delete(model.Selecteds[i].id);
                            log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                            none = false;
                        }
                    }
                    if (!none)
                    {
                        Session["Message"] = DateTime.Now.ToString() + " - " + "O(s) registros foram deletados.";
                        return RedirectToAction("Origin");
                    }
                    else
                    {
                        Session["Message"] = DateTime.Now.ToString() + " - " + "Não houveram registros selecionados.";
                        return RedirectToAction("Origin");
                    }
                }
            }
            catch (Exception ex)
            {
                Session["Message"] = DateTime.Now.ToString() + " - " + "Provavel erro de dependência de dados.";
                return RedirectToAction("Origin");
            }
        }

        // GET: Asset/Create
        public ActionResult Create()
        {
            return PartialView();
        }


        [HttpPost]
        public ActionResult Create(ProductionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.ProductionWriteRead.Save(new Tops.Domain.Production { Id = Guid.NewGuid(), dhi = model.dhi, dhf = model.dhf });
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


        public ActionResult StockCreate()
        {
            var products = this.ProductionWriteRead.All().Where(p => p.rProductionStock == null && p.Final == false);
            return PartialView(new ProductionViewModel { Productions = products});
        }


        [HttpPost]
        public ActionResult StockCreate(ProductionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rProductionStockWriteRead.Save(new Tops.Domain.rProductionStock { Id = model.ProductionId,GoalI = model.GoalI, GoalF = model.GoalF, Load = model.Load});
                    log.Info("Cadastro de novo registro realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Cadastro de novo registro realizado pelo usuário ";
                    return RedirectToAction("Stock");
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
                return RedirectToAction("Stock");
            }
        }

        public ActionResult OriginCreate()
        {
            var products = this.ProductionWriteRead.All().Where(p => p.Final == false);
            return PartialView(new ProductionViewModel { Productions = products});
        }


        [HttpPost]
        public ActionResult OriginCreate(ProductionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rProductionOriginWriteRead.Save(new Tops.Domain.rProductionOrigin { Id = Guid.NewGuid(), ProductionId = model.ProductionId,  GoalI = model.GoalI, GoalF = model.GoalF});
                    log.Info("Cadastro de novo registro realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Cadastro de novo registro realizado pelo usuário ";
                    return RedirectToAction("Origin");
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
                return RedirectToAction("Origin");
            }
        }

        public ActionResult BoardingCreate()
        {
            var products = this.ProductionWriteRead.All().Where(p => p.Final == false);
            var berths = (from brt in this.BerthWriteRead.All()
                          select new Domain.Type { Name = brt.Location.Alias, Id = brt.Id }).ToList();
            return PartialView(new ProductionViewModel { Productions = products, Berths = berths});
        }


        [HttpPost]
        public ActionResult BoardingCreate(ProductionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rProductionBoardingWriteRead.Save(new Tops.Domain.rProductionBoarding { Id = model.ProductionId,BerthId = model.BerthId, Compartment = model.Compartment, Load = model.Load });
                    log.Info("Cadastro de novo registro realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Cadastro de novo registro realizado pelo usuário ";
                    return RedirectToAction("Boarding");
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
                return RedirectToAction("Boarding");
            }
        }

        // GET: Asset/Edit/5
        public ActionResult Edit(Guid id)
        {
            var _Production = this.ProductionWriteRead.All().Where(p => p.Id.Equals(id)).First();
            ProductionViewModel model = new ProductionViewModel { Id = id, Active = _Production.Active, Final = _Production.Final, dhi = _Production.dhi, dhf = _Production.dhf };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Edit(ProductionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.ProductionWriteRead.Edit(new Tops.Domain.Production { Id = model.Id, dhi = model.dhi, dhf = model.dhf, Final = model.Final, Active = model.Active });
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

        public ActionResult StockEdit(Guid id)
        {
            var products = this.ProductionWriteRead.All().Where(p => p.rProductionStock == null && p.Final == false);
            var obj = this.rProductionStockWriteRead.All().Where(p => p.Id.Equals(id)).First();
            ProductionViewModel model = new ProductionViewModel { ProductionId = obj.Id, Productions = products, GoalI = obj.GoalI, GoalF = obj.GoalF, Load = obj.Load, Id = obj.Id };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult StockEdit(ProductionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rProductionStockWriteRead.Edit(new Tops.Domain.rProductionStock { GoalI = model.GoalI, GoalF = model.GoalF, Load = model.Load,Id = model.ProductionId});
                    log.Info("Modificado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Modificado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("Stock");
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
                return RedirectToAction("Stock");
            }
        }

        public ActionResult BoardingEdit(Guid id)
        {
            var obj = this.rProductionBoardingWriteRead.All().Where(p => p.Id.Equals(id)).First();
            var products = this.ProductionWriteRead.All().Where(p=>p.rProductionBoarding == null && p.Final == false);
            var berths = (from brt in this.BerthWriteRead.All()
                          select new Domain.Type { Name = brt.Location.Alias, Id = brt.Id }).ToList();
            ProductionViewModel model = new ProductionViewModel {
                BerthId = obj.BerthId, Load = obj.Load, Productions = products,
                Id = obj.Id, Compartment = obj.Compartment, Berths = berths.ToList(),
                ProductionId = obj.Id };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult BoardingEdit(ProductionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rProductionBoardingWriteRead.Edit(new Tops.Domain.rProductionBoarding { BerthId = model.BerthId, Compartment = model.Compartment, Load = model.Load, Id = model.ProductionId });
                    log.Info("Modificado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Modificado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("Boarding");
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
                return RedirectToAction("Boarding");
            }
        }

        public ActionResult OriginEdit(Guid id)
        {
            var obj = this.rProductionOriginWriteRead.All().Where(p => p.Id.Equals(id)).First();
            var products = this.ProductionWriteRead.All().Where(p => p.Final == false);
            ProductionViewModel model = new ProductionViewModel { Productions = products, ProductionId = obj.ProductionId, Id = obj.Id, GoalI = obj.GoalI, GoalF = obj.GoalF };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult OriginEdit(ProductionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rProductionOriginWriteRead.Edit(new Tops.Domain.rProductionOrigin { ProductionId = model.ProductionId, Id = model.Id });
                    log.Info("Modificado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Modificado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("Origin");
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
                return RedirectToAction("Origin");
            }
        }

        public ActionResult Route()
        {
            var ieList = this.rProductionRouteWriteRead.All().ToList();
            List<SelectedGuid> ieSelecteds = new List<SelectedGuid>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedGuid obj = new SelectedGuid();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            ProductionViewModel model = new ProductionViewModel { ProductionRoutes = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Route", model);
        }

        [HttpPost]
        public ActionResult Route(ProductionViewModel model)
        {
            try
            {
                if (model.Id != Guid.Empty)
                {
                    this.rProductionRouteWriteRead.Delete(model.Id);
                    log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Deletado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("Route");
                }
                else
                {
                    bool none = true;
                    for (int i = 0; i < model.Selecteds.Count(); i++)
                    {
                        if (model.Selecteds[i].isSelected == true)
                        {
                            this.rProductionRouteWriteRead.Delete(model.Selecteds[i].id);
                            log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                            none = false;
                        }
                    }
                    if (!none)
                    {
                        Session["Message"] = DateTime.Now.ToString() + " - " + "O(s) registros foram deletados.";
                        return RedirectToAction("Route");
                    }
                    else
                    {
                        Session["Message"] = DateTime.Now.ToString() + " - " + "Não houveram registros selecionados.";
                        return RedirectToAction("Route");
                    }
                }
            }
            catch (Exception ex)
            {
                Session["Message"] = DateTime.Now.ToString() + " - " + "Provavel erro de dependência de dados.";
                return RedirectToAction("Route");
            }
        }

        public ActionResult RouteEdit(Guid id)
        {
            var route = (from rt in this.RouteWriteRead.All()//.Where(p => p.rRouteActive != null)
                         select new Domain.Type {
                             //Name = rt.Description,
                             Id = rt.Id }).ToList();
            var obj = this.rProductionRouteWriteRead.All().Where(p => p.Id.Equals(id)).First();
            var products = this.ProductionWriteRead.All().Where(p => p.Final == false);
            ProductionViewModel model = new ProductionViewModel { dhi = obj.dhi, dhf = obj.dhf, Routes = route, Productions = products, RouteId = obj.RouteId, ProductionId = obj.ProductionId, Load = obj.Load, Id = obj.Id };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult RouteEdit(ProductionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rProductionRouteWriteRead.Edit(new Tops.Domain.rProductionRoute { ProductionId = model.ProductionId, RouteId = model.RouteId, Load = model.Load, Id = model.Id, dhi = model.dhi, dhf = model.dhf });
                    log.Info("Modificado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Modificado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("Route");
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
                return RedirectToAction("Route");
            }
        }

        public ActionResult RouteCreate()
        {
            var products = this.ProductionWriteRead.All().Where(p => p.Final == false);

            var route = (from rt in this.RouteWriteRead.All().Where(p => p != null)
                         select new Domain.Type {
                             //Name = rt.Description,
                             Id = rt.Id}).ToList();
            return PartialView(new ProductionViewModel { Routes = route, Productions = products });
        }


        [HttpPost]
        public ActionResult RouteCreate(ProductionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rProductionRouteWriteRead.Save(new Tops.Domain.rProductionRoute { Id = Guid.NewGuid(), ProductionId = model.ProductionId, Load = model.Load, RouteId = model.RouteId, dhi = model.dhi, dhf = model.dhf });
                    log.Info("Cadastro de novo registro realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Cadastro de novo registro realizado pelo usuário ";
                    return RedirectToAction("Route");
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
                return RedirectToAction("Route");
            }
        }

    }
}
