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
    public class timeSpentPerTasksController : Controller
    {
        private WrokDBEntities db = new WrokDBEntities();

        // GET: timeSpentPerTasks
        public ActionResult Index()
        {
            return View(db.timeSpentPerTasks.ToList());
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
