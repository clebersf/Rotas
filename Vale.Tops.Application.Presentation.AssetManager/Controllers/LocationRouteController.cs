//using log4net;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Vale.Tops.Domain;
//using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Class;
//using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Interface;
//using Vale.Tops.Application.Presentation.AssetManager.Models;

//namespace Vale.Tops.Application.Presentation.AssetManager.Controllers
//{
//    public class LocationRouteController : Controller
//    {
//        private ILocationWriteRead LocationWriteRead;
//        private IRouteWriteRead RouteWriteRead;
//        private IrLocationRouteWriteRead rLocationRouteWriteRead;
//        private IrTypeInstrumentWriteRead rTypeInstrumentWriteRead;
//        ILog log = LogManager.GetLogger("Admin Cadastro Location");
//        public LocationRouteController ()
//        {
//            this.RouteWriteRead = new RouteWriteRead();
//            this.LocationWriteRead = new LocationWriteRead();
//            this.rLocationRouteWriteRead = new rLocationRouteWriteRead();
//            this.rTypeInstrumentWriteRead = new rTypeInstrumentWriteRead();
//        }

//        // GET: Locationr
//        public ActionResult Index()
//        {
//            var ieList = this.rLocationRouteWriteRead.All().ToList();
//            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
//            for (int i = 0; i < ieList.Count(); i++)
//            {
//                SelectedInt obj = new SelectedInt();
//                obj.id = ieList.ToList()[i].Id;
//                obj.isSelected = false;
//                ieSelecteds.Add(obj);
//            }
//            LocationRouteViewModel model = new LocationRouteViewModel { rLocationRoutes = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

//            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
//            return View("Index", model);
//        }

//        [HttpPost]
//        public ActionResult Index(LocationRouteViewModel model)
//        {
//            try
//            {
//                if (model.Id != 0)
//                {
//                    this.rLocationRouteWriteRead.Delete(model.Id);
//                    log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);                    
//                    Session["Message"] =  DateTime.Now.ToString() + " - " + "Deletado cadastro do registro " + model.Id.ToString();
//                    return RedirectToAction("Index");
//                }
//                else
//                {
//                    bool none = true;
//                    for (int i = 0; i < model.Selecteds.Count(); i++)
//                    {
//                        if (model.Selecteds[i].isSelected == true)
//                        {
//                            this.rLocationRouteWriteRead.Delete(model.Selecteds[i].id);
//                            log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
//                            none = false;
//                        }                        
//                    }
//                    if (!none)
//                    {
//                        Session["Message"] = DateTime.Now.ToString() + " - " + "O(s) registros foram deletados.";
//                        return RedirectToAction("Index");
//                    }
//                    else
//                    {
//                        Session["Message"] = DateTime.Now.ToString() + " - " + "Não houveram registros selecionados.";
//                        return RedirectToAction("Index");
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Session["Message"] = DateTime.Now.ToString() + " - " + "Provavel erro de dependência de dados.";
//                return RedirectToAction("Index");
//            }
//        }

//        // GET: Asset/Create
//        public ActionResult Create()
//        {
//            var ieList_1 = (from Route in this.RouteWriteRead.All()
//                         select Route);
//            var ieList_2 = (from Location in this.LocationWriteRead.All()
//                            join tpd in this.rTypeInstrumentWriteRead.All() on Location.TypeId equals tpd.Id
//                            select Location);
//            return PartialView(new LocationRouteViewModel { Routes = ieList_1, Locations = ieList_2 });
//        }

//        [HttpPost]
//        public ActionResult Create(LocationRouteViewModel model)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    this.rLocationRouteWriteRead.Save(new rLocationRoute { LocationId = model.LocationId,
//                        RouteId = model.RouteId });
//                    log.Info("Cadastro de novo registro realizado pelo usuário " + User.Identity.Name);                    
//                    Session["Message"] =  DateTime.Now.ToString() + " - " + "Cadastro de novo registro realizado pelo usuário ";
//                    return RedirectToAction("Index");
//                }
//                // model is not valid
//                return Json(new
//                {
//                    status = "failure",
//                    formErrors = ModelState.Select(kvp => new { key = kvp.Key, errors = kvp.Value.Errors.Select(e => e.ErrorMessage) })
//                });            
//            }
//            catch (Exception ex)
//            {                
//                Session["Message"] =  DateTime.Now.ToString() + " - " + "Erro: " + ex.Message;
//                return RedirectToAction("Index");
//            }
//        }

//    }
//}
