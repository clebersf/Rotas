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
    public class GraphController : Controller
    {
        private IvwGraphJsonReadOnly vwGraphJsonReadOnly;
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool Store(string jsonObj)
        {
            //Graph graph = new Graph();
            //graph.setJsonData(jsonObj);
            //DatabaseConnector.saveData(Serializer.Serialize(graph));
            return true;
        }

        public String Restore()
        {
            vwGraphJsonReadOnly = new vwGraphJsonReadOnly();
            //Graph graph = new Graph();
            //graph.setJsonData(this.vwGraphJsonReadOnly.All().First().Json);
            try
            {
                return null;//graph.getJsonData();
            }
            catch (NullReferenceException e)
            {
                return null;
            }
        }
    }
}