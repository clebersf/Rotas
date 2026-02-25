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
    public class RouteTravelController : Controller
    {
        private IRouteWriteRead RouteWriteRead;
        private IrRouteTravelWriteRead rRouteTravelWriteRead;
        private OxDWriteRead OxDWriteRead;
        ILog log = LogManager.GetLogger("Admin Cadastro Route");
        public RouteTravelController ()
        {
            this.RouteWriteRead = new RouteWriteRead();
            this.rRouteTravelWriteRead = new rRouteTravelWriteRead();
            this.OxDWriteRead = new OxDWriteRead();
        }

        // GET: Router
        public ActionResult Index()
        {
            var ieList = this.RouteWriteRead.All();
            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            RouteTravelViewModel model = new RouteTravelViewModel { Routes = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(RouteTravelViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.rRouteTravelWriteRead.Delete(model.Id);
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
                            this.rRouteTravelWriteRead.Delete(model.Selecteds[i].id);
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
            var route = this.rRouteTravelWriteRead.All();
            return PartialView(new RouteTravelViewModel { RouteTravels = route });
        }


        [HttpPost]
        public ActionResult Create(RouteTravelViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rRouteTravelWriteRead.Save(new rRouteTravel { Id = model.Id, Constant = model.Constante, Variable = model.Variable, GoalI = model.GoalI, GoalF = model.GoalF });
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
            var _Route = (from Route in this.RouteWriteRead.All().Where(p => p.Id.Equals(id))
                          select Route).First();
            //double constante = _Route.rRouteTravel != null ? _Route.rRouteTravel.Constant : 0;
            //double variable = _Route.rRouteTravel != null ? _Route.rRouteTravel.Variable : 0;
            //double goali = _Route.rRouteTravel != null ? _Route.rRouteTravel.GoalI : 0;
            //double goalf = _Route.rRouteTravel != null ? _Route.rRouteTravel.GoalF : 0;
            //RouteTravelViewModel model = new RouteTravelViewModel { Id = _Route.Id, Constante = constante, Variable = variable, GoalI = goali, GoalF = goalf };
            return PartialView(null);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Edit(RouteTravelViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (rRouteTravelWriteRead.All().Where(p => p.Id == model.Id).Count() > 0)
                    {
                        this.rRouteTravelWriteRead.Edit(new rRouteTravel
                        {
                            Id = model.Id,
                            Constant = model.Constante,
                            Variable = model.Variable,
                            GoalI = model.GoalI,
                            GoalF = model.GoalF
                        });
                    }
                    else
                    {
                        this.rRouteTravelWriteRead.Save(new rRouteTravel
                        {
                            Id = model.Id,
                            Constant = model.Constante,
                            Variable = model.Variable,
                            GoalI = model.GoalI,
                            GoalF = model.GoalF
                        });
                    }
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

    }
}
