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
    public class TagGroupController : Controller
    {
        private IrTagGroupWriteRead rTagGroupWriteRead;
        private ITagWriteRead TagWriteRead;
        ILog log = LogManager.GetLogger("Admin Cadastro rTagGroup");
        public TagGroupController ()
        {
            this.rTagGroupWriteRead = new rTagGroupWriteRead();
            this.TagWriteRead = new TagWriteRead();
        }

        // GET: rTagGroupr
        public ActionResult Index()
        {
            var ieList = this.rTagGroupWriteRead.All();
            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            TagGroupViewModel model = new TagGroupViewModel { rTagGroups = ieList, Selecteds = ieSelecteds, Message = DateTime.Now.ToString() + " - " + "Lista acessada.", lines = ieList.Count() };

            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(TagGroupViewModel model)
        {
            try
            {
                if (model.Id != 0)
                {
                    this.rTagGroupWriteRead.Delete(model.Id); 
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
                            this.rTagGroupWriteRead.Delete(model.Selecteds[i].id);
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
            var tags = from tag in this.TagWriteRead.All()
                       select new Tag
                       {
                           Id = tag.Id,
                           Name = "[" + tag.Plc.Location.Name + "]" + tag.Name
                       };
            var ieList = this.rTagGroupWriteRead.All().Where(p=>p.ParentId == null);
            return PartialView(new TagGroupViewModel { rTagGroups = ieList.ToList(), Tags = tags });
        }


        [HttpPost]
        public ActionResult Create(TagGroupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rTagGroupWriteRead.Save(new Tops.Domain.rTagGroup { WindowsService = model.WindowsService, Rate = model.Rate, ParentId = model.ParentId, TagId = model.TagId, OpcServer = model.OpcServer,
                        AddrOpcServer = model.AddrOpcServer, Url_Read = model.Url_Read, Url_Write = model.Url_Write });
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
            var tags = from tag in this.TagWriteRead.All()
                       select new Tag
                       {
                           Id = tag.Id,
                           Name = "[" + tag.Plc.Location.Name + "]" + tag.Name
                       };
            var ieList = this.rTagGroupWriteRead.All().Where(p => p.ParentId == null);
            var _rTagGroup = (from rTagGroup in this.rTagGroupWriteRead.All().Where(p => p.Id.Equals(id))
                          select rTagGroup).First();
            TagGroupViewModel model = new TagGroupViewModel { Tags = tags, rTagGroups = ieList, WindowsService = _rTagGroup.WindowsService,
                Rate = _rTagGroup.Rate, Id = _rTagGroup.Id, TagId = _rTagGroup.TagId, ParentId = _rTagGroup.ParentId, OpcServer = _rTagGroup.OpcServer,
                AddrOpcServer = _rTagGroup.AddrOpcServer, Url_Read = _rTagGroup.Url_Read, Url_Write = _rTagGroup.Url_Write };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Edit(TagGroupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.rTagGroupWriteRead.Edit(new Tops.Domain.rTagGroup { WindowsService = model.WindowsService, Rate = model.Rate, Id = model.Id, ParentId = model.ParentId, TagId = model.TagId,
                        OpcServer = model.OpcServer,
                        AddrOpcServer = model.AddrOpcServer,
                        Url_Read = model.Url_Read,
                        Url_Write = model.Url_Write
                    });
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
