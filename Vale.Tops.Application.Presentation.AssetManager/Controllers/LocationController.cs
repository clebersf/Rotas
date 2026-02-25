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
using System.Web.Script.Serialization;
using MoreLinq;

namespace Vale.Tops.Application.Presentation.AssetManager.Controllers
{
    public class LocationController : Controller
    {
        private ILocationWriteRead LocationWriteRead;
        private ITypeWriteRead TypeWriteRead;
        ILog log = LogManager.GetLogger("Admin Cadastro Location");

        public LocationController ()
        {
            this.LocationWriteRead = new LocationWriteRead();
            this.TypeWriteRead = new TypeWriteRead();
        }

        public ActionResult Index(LocationViewModel model)
        {
            //string filter = model.Filter == null? "" : model.Filter;
            //string negfilter = model.NegFilter == null | model.NegFilter == "" ? "##$$%%&&" : model.NegFilter;
            var ieList = this.LocationWriteRead.All();
                //.Where(p => (p.Description.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 || p.Name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0))
                //.Where(p => (p.Description.IndexOf(negfilter, StringComparison.OrdinalIgnoreCase) < 0 && p.Name.IndexOf(negfilter, StringComparison.OrdinalIgnoreCase) < 0));//.Take(500);

            List<SelectedInt> ieSelecteds = new List<SelectedInt>();
            for (int i = 0; i < ieList.Count(); i++)
            {
                SelectedInt obj = new SelectedInt();
                obj.id = ieList.ToList()[i].Id;
                obj.isSelected = false;
                ieSelecteds.Add(obj);
            }
            if (model.Id != 0)
            {
                this.LocationWriteRead.Delete(model.Id);
                log.Info("Deletado cadastro do registro " + model.Id.ToString() + " realizado pelo usuário " + User.Identity.Name);
                Session["Message"] = DateTime.Now.ToString() + " - " + "Deletado cadastro do registro " + model.Id.ToString();
            }
            else
            {
                model.Message = DateTime.Now.ToString() + " - " + "Lista acessada.";
            }
            model.Selecteds = ieSelecteds;
            model.Locations = ieList;
            
            model.lines = ieList.Count();
            ViewBag.Message = HttpContext.Session.Count > 0 ? HttpContext.Session["Message"].ToString() : "";
            return View("Index", model);
        }


        public ActionResult Create()
        {
            // alimenta dropdown
            var _types = (from type in this.TypeWriteRead.All()
                             select type);
            // alimenta dropdown
            var _parents = (from location in this.LocationWriteRead.All()//.Where(p => p.ParentId.Equals(null))
                            select location);
            LocationViewModel model = new LocationViewModel
            {
                Types = _types, Parents = _parents
            };
            return PartialView(model);
        }

        // GET: Asset/Edit/5
        public ActionResult Copy(int id)
        {
            var _Location = (from Location in this.LocationWriteRead.All().Where(p => p.Id.Equals(id))
                             select Location).First();
            var ieType = (from Type in this.TypeWriteRead.All()
                          select Type);

            // alimenta dropdown
            var _parents = (from location in this.LocationWriteRead.All()//.Where(p => p.ParentId.Equals(null))
                            select location);
            LocationViewModel model = new LocationViewModel
            {
                Id = _Location.Id,
                Description = _Location.Description,
                TypeId = _Location.TypeId,
                Name = _Location.Name,
                ParentId = _Location.ParentId,
                Types = ieType,
                Parents = _parents,
                Alias = _Location.Alias
            };
            return PartialView("Create",model);
        }

        [HttpPost]
        public ActionResult Create(LocationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.LocationWriteRead.Save(new Location
                    {
                        Description = model.Description,
                        TypeId = model.TypeId,
                        Name = model.Name,
                        ParentId = model.ParentId,
                        Alias = model.Alias
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
            var _Location = (from Location in this.LocationWriteRead.All().Where(p => p.Id.Equals(id))
                             select Location).First();
            var ieType = (from Type in this.TypeWriteRead.All()
                          select Type);
  
            // alimenta dropdown
            var _parents = (from location in this.LocationWriteRead.All()//.Where(p => p.ParentId.Equals(null))
                            select location);
            LocationViewModel model = new LocationViewModel
            {
                Id = _Location.Id,
                Description = _Location.Description,
                TypeId = _Location.TypeId,
                Name = _Location.Name,
                ParentId = _Location.ParentId,
                Types = ieType,
                Parents = _parents,
                Alias = _Location.Alias
            };
            return PartialView(model);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        public ActionResult Edit(LocationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.LocationWriteRead.Edit(new Location
                    {
                        Id = model.Id,
                        Description = model.Description,
                        TypeId = model.TypeId,
                        Name = model.Name,
                        ParentId = model.ParentId,
                        Alias = model.Alias
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
        [AllowAnonymous]
        public ActionResult Tree ()
        {
            List<TreeViewNode> nodes = new List<TreeViewNode>();
            var ieL1 = (from L1 in this.LocationWriteRead.All().Where(p => p.ParentId.Equals(null))
                          select new TreeViewNode
                          {
                              id = L1.Id.ToString(),
                              parent = "#",
                              text = L1.Name + " - " + L1.Id.ToString()
                          });

            nodes.AddRange(ieL1.DistinctBy(p=>p.id));

            var ieL2 = (from NL in this.LocationWriteRead.All()
                        join PL in ieL1 on NL.ParentId.ToString() equals PL.id
                        select new TreeViewNode
                        {
                            id = NL.Id.ToString(),
                            parent = NL.ParentId.ToString(),
                            text = NL.Name + " - " + NL.Id.ToString()
                        });

            nodes.AddRange(ieL2.DistinctBy(p => p.id));

            var ieL3 = (from NL in this.LocationWriteRead.All()
                        join PL in ieL2 on NL.ParentId.ToString() equals PL.id
                        select new TreeViewNode
                        {
                            id = NL.Id.ToString(),
                            parent = NL.ParentId.ToString(),
                            text = NL.Name + " - " + NL.Id.ToString()
                        });

            nodes.AddRange(ieL3.DistinctBy(p => p.id));

            var ieL4 = (from NL in this.LocationWriteRead.All()
                        join PL in ieL3 on NL.ParentId.ToString() equals PL.id
                        select new TreeViewNode
                        {
                            id = NL.Id.ToString(),
                            parent = NL.ParentId.ToString(),
                            text = NL.Name + " - " + NL.Id.ToString()
                        });

            nodes.AddRange(ieL4.DistinctBy(p => p.id));

            var ieL5 = (from NL in this.LocationWriteRead.All()
                        join PL in ieL4 on NL.ParentId.ToString() equals PL.id
                        select new TreeViewNode
                        {
                            id = NL.Id.ToString(),
                            parent = NL.ParentId.ToString(),
                            text = NL.Name + " - " + NL.Id.ToString()
                        });

            nodes.AddRange(ieL5.DistinctBy(p => p.id));

            var ieL6 = (from NL in this.LocationWriteRead.All()
                        join PL in ieL5 on NL.ParentId.ToString() equals PL.id
                        select new TreeViewNode
                        {
                            id = NL.Id.ToString(),
                            parent = NL.ParentId.ToString(),
                            text = NL.Name + " - " + NL.Id.ToString()
                        });

            nodes.AddRange(ieL6.DistinctBy(p => p.id));

            var ieL7 = (from NL in this.LocationWriteRead.All()
                        join PL in ieL6 on NL.ParentId.ToString() equals PL.id
                        select new TreeViewNode
                        {
                            id = NL.Id.ToString(),
                            parent = NL.ParentId.ToString(),
                            text = NL.Name + " - " + NL.Id.ToString()
                        });

            nodes.AddRange(ieL7.DistinctBy(p => p.id));
            //Serialize to JSON string.
            ViewBag.Json = (new JavaScriptSerializer()).Serialize(nodes);
            return View();
        }

    }
}
