using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using Vale.Tops.Application.Presentation.AssetManager.Models;
using Vale.Tops.Domain;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Class;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Interface;

namespace Vale.Tops.Application.Presentation.AssetManager.Controllers
{   
    public class HomeController : Controller
    {
        private RuleWriteRead RuleWriteRead;
        private IvwFTACChangeReadOnly vwFTACChangeReadOnly;
        public HomeController ()
        {
            this.RuleWriteRead = new RuleWriteRead();
            this.vwFTACChangeReadOnly = new vwFTACChangeReadOnly();
        }


        [AllowAnonymous]
        public ActionResult Portal()
        {
            return View();
        }

        public PartialViewResult Identity()
        {
            return PartialView();
        }

    }
}