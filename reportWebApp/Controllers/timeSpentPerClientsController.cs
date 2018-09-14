using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using reportWebApp.Models;

namespace reportWebApp.Controllers
{
    public class timeSpentPerClientsController : Controller
    {
        private WrokDBEntities db = new WrokDBEntities();

        // GET: timeSpentPerClients
        public ActionResult Index()
        {
            return View(db.timeSpentPerClients.ToList());
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
