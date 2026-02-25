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
    public class TagController : Controller
    {
        private ITagWriteRead TagWriteRead;
        private IPlcWriteRead PlcWriteRead;
        ILog log = LogManager.GetLogger("Admin Cadastro Tag");
        public TagController ()
        {
            this.TagWriteRead = new TagWriteRead();
            this.PlcWriteRead = new PlcWriteRead();
        }

        // GET: Tagr
        public ActionResult Index()
        {
            var ieList = this.TagWriteRead.All();
            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            TagViewModel model = new TagViewModel { Tags = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(TagViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.TagWriteRead.Delete(model.Id); 
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
                            this.TagWriteRead.Delete(model.Selecteds[i].id);
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
            var plcs = PlcWriteRead.All();
            return PartialView(new TagViewModel { Plcs = plcs });
        }

        // GET: Asset/Edit/5
        public ActionResult Copy(int id)
        {
            var plcs = PlcWriteRead.All();
            var _Tag = (from Tag in this.TagWriteRead.All().Where(p => p.Id.Equals(id))
                         select new Tops.Domain.Tag
                         {
                             Description = Tag.Description,
                             Id = Tag.Id,
                             Name = Tag.Name
                         }).First();
            TagViewModel model = new TagViewModel { Plcs = plcs, Description = _Tag.Description, Id = _Tag.Id, Name = _Tag.Name };
            return PartialView("Create",model);
        }

        [HttpPost]
        public ActionResult Create(TagViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.TagWriteRead.Save(new Tops.Domain.Tag { Description = model.Description, Name = model.Name, PlcId = model.PlcId });
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
            var plcs = PlcWriteRead.All();
            var _Tag = (from Tag in this.TagWriteRead.All().Where(p => p.Id.Equals(id))
                          select Tag).First();
            TagViewModel model = new TagViewModel { Plcs = plcs, Description = _Tag.Description, Id = _Tag.Id, Name = _Tag.Name, PlcId = _Tag.PlcId };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Edit(TagViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.TagWriteRead.Edit(new Tops.Domain.Tag { Description = model.Description, Id = model.Id, Name = model.Name, PlcId = model.PlcId });
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
