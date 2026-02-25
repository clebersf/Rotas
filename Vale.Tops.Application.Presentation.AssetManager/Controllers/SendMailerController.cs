using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Vale.Tops.Application.Presentation.AssetManager.Controllers
{
    public class SendMailerController : Controller
    {
        //  
        // GET: /SendMailer/   
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Index(Vale.Tops.Application.Presentation.AssetManager.Models.MailModel _objModelMail)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress(_objModelMail.From);
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "172.23.191.18";
                smtp.Port = 25;
                smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new System.Net.NetworkCredential(@"ta-vale\svc.assetman", "svc.assetman"); // Enter seders User name and password   
                smtp.EnableSsl = false;
                smtp.Send(mail);
                return View("Index", _objModelMail);
            }
            else
            {
                return View();
            }
        }
    }
}