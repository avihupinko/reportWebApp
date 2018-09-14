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
    public class reportsController : Controller
    {
        private WrokDBEntities db = new WrokDBEntities();

        // GET: reports
        public ActionResult Index()
        {
            var reports = db.reports.Include(r => r.task);
            return View(reports.ToList());
        }

        public ActionResult taskReports(int? id)
        {
            task T = db.tasks.Find(id);
            if (T == null)
            {
                return HttpNotFound();
            }
            taskReports TR = new taskReports { Ta = T};
            

            var rep = db.spGetReportsByTaskId(id);
            TR.Re = new List<report>();

            foreach (var r in rep)
            {

                TR.Re.Add(new report{Info = r.Info, taskId = r.taskId, date= r.date, time= r.time, reportId = r.reportId });
            }

            return View(TR);
        }

        // GET: reports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            report report = db.reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: reports/Create
        public ActionResult Create(int? id)
        {
            var list = new List<task>();
            var cl = db.tasks.Find(id);
            list.Add(cl);

            ViewBag.taskId = new SelectList(list, "taskId", "Name");
            return View();
        }

        // POST: reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "reportId,taskId,date,Info,time")] report report)
        {
            if (ModelState.IsValid)
            {
                db.reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("taskReports", new { id = report.taskId });
            }

            ViewBag.taskId = new SelectList(db.tasks, "taskId", "Name", report.taskId);
            return View(report);
        }

        // GET: reports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            report report = db.reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            ViewBag.taskId = new SelectList(db.tasks, "taskId", "Name", report.taskId);
            return View(report);
        }

        // POST: reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "reportId,taskId,date,Info,time")] report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("taskReports", new { id = report.taskId });
            }
            ViewBag.taskId = new SelectList(db.tasks, "taskId", "Name", report.taskId);
            return View(report);
        }

        // GET: reports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            report report = db.reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            report report = db.reports.Find(id);
            var taskId = report.taskId;
            db.reports.Remove(report);
            db.SaveChanges();
            return RedirectToAction("taskReports", new { id = taskId });
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
