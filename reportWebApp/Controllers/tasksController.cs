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
    public class tasksController : Controller
    {
        private WrokDBEntities db = new WrokDBEntities();

        // GET: tasks
        public ActionResult Index()
        {
            var tasks = db.tasks.Include(t => t.project);
            return View(tasks.ToList());
        }

        public ActionResult projectTasks(int? id)
        {
            project pr = db.projects.Find(id);
            if (pr == null)
            {
                return HttpNotFound();
            }
            projectTasks PT = new projectTasks();
            PT.Pr = new project { projectId = pr.projectId, Name = pr.Name, createdAt = pr.createdAt , ClientId= pr.ClientId};
            

            var tasks = db.spGetTasksByProjectId(id);
            PT.Ts = new List<task>();

            foreach (var r in tasks)
            {

                PT.Ts.Add(new task { projectId = r.projectId, Info = r.Info, Name = r.Name, taskId = r.taskId });

            }

            return View(PT);
        }

        // GET: tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: tasks/Create
        public ActionResult Create(int? id)
        {
            var list = new List<project>();
            var project = db.projects.Find(id);
            list.Add(new project { projectId = project.projectId, createdAt = project.createdAt, Name = project.Name});
            ViewBag.projectId = new SelectList(list, "projectId", "Name");
            return View();
        }

        // POST: tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "taskId,projectId,Name,Info")] task task)
        {
            if (ModelState.IsValid)
            {
                db.tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("projectTasks", new { id = task.projectId });
            }

            ViewBag.projectId = new SelectList(db.projects, "projectId", "Name", task.projectId);
            return View(task);
        }

        // GET: tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.projectId = new SelectList(db.projects, "projectId", "Name", task.projectId);
            return View(task);
        }

        // POST: tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "taskId,projectId,Name,Info")] task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("projectTasks", new { id = task.projectId });
            }
            ViewBag.projectId = new SelectList(db.projects, "projectId", "Name", task.projectId);
            return View(task);
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
