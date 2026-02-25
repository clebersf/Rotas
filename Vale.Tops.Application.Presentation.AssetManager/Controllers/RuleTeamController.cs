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
    public class RuleTeamController : Controller
    {      
        private IApplicationWriteRead ApplicationWriteRead;
        private IRuleWriteRead RuleWriteRead;
        ILog log = LogManager.GetLogger("Gestão de equipes");

        public RuleTeamController ()
        {
            this.ApplicationWriteRead = new ApplicationWriteRead();
            this.RuleWriteRead = new RuleWriteRead();
        }

        public ActionResult Index()
        {
            var ieList = this.RuleWriteRead.All();

            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            RuleTeamViewModel model = new RuleTeamViewModel();
            model.Selecteds = ieSelecteds;
            model.Rules = ieList;

            model.Message = DateTime.Now.ToString() + " - " + "Lista acessada.";
            model.lines = ieList.Count();
            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }
        // GET: Ruler
        [HttpPost]
        public ActionResult Index(RuleTeamViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.RuleWriteRead.Delete(model.Id);
                    log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                    Session["Message"] = DateTime.Now.ToString() + " - " + "Deletado cadastro do registro " + model.Id.ToString();
                    return RedirectToAction("Index");
                }
                else
                {
                    bool none = true;
                    for (int i = 0; i < model.Selecteds.Count(); i++)
                    {
                        if (model.Selecteds[i].isSelected == true)
                        {                            
                            this.RuleWriteRead.Delete(model.Selecteds[i].id);
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
            // alimenta dropdown
            var _Applications = (from application in this.ApplicationWriteRead.All()
                             select new Tops.Domain.Application
                             {                                 
                                 Id = application.Id,
                                 Description = application.Description,
                                 Name = application.Name
                             });
            // alimenta dropdown
            var _rule = (from rule in this.RuleWriteRead.All()
                          select new Rule
                          {
                              Id = rule.Id,
                              Description = rule.Description,
                              GroupDns = rule.GroupDns
                          });
            RuleTeamViewModel model = new RuleTeamViewModel
            {
                Applications = _Applications, Rules = _rule
            };
            return PartialView(model);
        }

        // GET: Asset/Edit/5
        public ActionResult Copy(int id)
        {
            // alimenta dropdown
            var _applications = (from Application in this.ApplicationWriteRead.All()
                                 select new Tops.Domain.Application
                                 {
                                     Id = Application.Id,
                                     Description = Application.Description,
                                     Name = Application.Name
                                 });
            // alimenta dropdown
            var _rule = (from rule in this.RuleWriteRead.All()
                         select new Rule
                         {
                             Id = rule.Id,
                             Description = rule.Description,
                             GroupDns = rule.GroupDns
                         });
            // alimenta dropdown
            var _rrule = (from rrule in this.RuleWriteRead.All()
                          select new Rule
                          {
                              Id = rrule.Id,
                              ApplicationId = rrule.ApplicationId,
                          });
            RuleTeamViewModel model = new RuleTeamViewModel
            {
                Applications = _applications,
                Rules = _rule,
                ApplicationId = _rrule.Where(p => p.Id.Equals(id)).First().ApplicationId,
                RuleId = _rrule.Where(p => p.Id.Equals(id)).First().Id
            };
            return PartialView("Create",model);
        }

        [HttpPost]
        public ActionResult Create(RuleTeamViewModel model)
        {
            try
            {
                model.Id = model.RuleId;
                if (ModelState.IsValid)
                {
                    this.RuleWriteRead.Save(new Rule
                    {
                        Id = model.RuleId,
                        ApplicationId = model.ApplicationId
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

        // GET: Asset/Edit/5
        public ActionResult Edit(int id)
        {
            // alimenta dropdown
            var _applications = (from Application in this.ApplicationWriteRead.All()
                                 select new Tops.Domain.Application
                                 {
                                     Id = Application.Id,
                                     Description = Application.Description,
                                     Name = Application.Name
                                 });
            // alimenta dropdown
            var _rule = (from rule in this.RuleWriteRead.All()
                         select new Rule
                         {
                             Id = rule.Id,
                             Description = rule.Description,
                             GroupDns = rule.GroupDns
                         });
            // alimenta dropdown
            var _rrule = (from rrule in this.RuleWriteRead.All()
                         select new Rule
                         {
                             Id = rrule.Id,
                             ApplicationId = rrule.ApplicationId,                             
                         });
            RuleTeamViewModel model = new RuleTeamViewModel
            {
                Applications = _applications,
                Rules = _rule,
                ApplicationId = _rrule.Where(p => p.Id.Equals(id)).First().ApplicationId,
                RuleId = _rrule.Where(p => p.Id.Equals(id)).First().Id
            };           
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Edit(RuleTeamViewModel model)
        {
            try
            {
                model.Id = model.RuleId;
                if (ModelState.IsValid)
                {
                    this.RuleWriteRead.Edit(new Rule
                    {
                        Id = model.Id,
                        ApplicationId = model.ApplicationId
                    });
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
