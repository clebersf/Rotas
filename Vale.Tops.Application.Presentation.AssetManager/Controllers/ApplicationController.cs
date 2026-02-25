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
    public class ApplicationController : Controller
    {
        private IApplicationWriteRead ApplicationWriteRead;
        ILog log = LogManager.GetLogger("Admin Cadastro Application");
        public ApplicationController()
        {
            this.ApplicationWriteRead = new ApplicationWriteRead();
        }

        // GET: Applicationr
        public ActionResult Index()
        {
            var ieList = (from Application in this.ApplicationWriteRead.All()
                          select new Tops.Domain.Application
                          {
                              Description = Application.Description,
                              Id = Application.Id,
                              Name = Application.Name
                          });
            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            ApplicationViewModel model = new ApplicationViewModel { Applications = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(ApplicationViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.ApplicationWriteRead.Delete(model.Id); 
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
                            this.ApplicationWriteRead.Delete(model.Selecteds[i].id);
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
            return PartialView();
        }

        public ActionResult Copy(int id)
        {
            var _Application = (from Application in this.ApplicationWriteRead.All().Where(p => p.Id.Equals(id))
                                select new Tops.Domain.Application
                                {
                                    Description = Application.Description,
                                    Id = Application.Id,
                                    Name = Application.Name
                                }).First();
            ApplicationViewModel model = new ApplicationViewModel { Description = _Application.Description, Id = _Application.Id, Name = _Application.Name };
            return PartialView("Create",model);
        }

        [HttpPost]
        public ActionResult Create(ApplicationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.ApplicationWriteRead.Save(new Tops.Domain.Application { Description = model.Description, Name = model.Name });
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
            var _Application = (from Application in this.ApplicationWriteRead.All().Where(p => p.Id.Equals(id))
                          select new Tops.Domain.Application
                          {
                              Description = Application.Description,
                              Id = Application.Id,
                              Name = Application.Name
                          }).First();
            ApplicationViewModel model = new ApplicationViewModel { Description = _Application.Description, Id = _Application.Id, Name = _Application.Name};
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Edit(ApplicationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.ApplicationWriteRead.Edit(new Domain.Application { Description = model.Description, Id = model.Id, Name = model.Name});
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
