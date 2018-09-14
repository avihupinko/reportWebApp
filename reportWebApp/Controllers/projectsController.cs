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
    public class projectsController : Controller
    {
        private WrokDBEntities db = new WrokDBEntities();

        // GET: projects
        public ActionResult Index()
        {
            var projects = db.projects.Include(p => p.Client);
            return View(projects.ToList());
        }

        public ActionResult clientProjects(int? id)
        {
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            clientProjects CP = new clientProjects();
            CP.Cl = new Client();
            CP.Cl.clientId = client.clientId;
            CP.Cl.Name = client.Name;

            var projects = db.spGetProjectsByClient(id);
            CP.Pr = new List<project>();
            
            foreach (var r in projects)
            {

                CP.Pr.Add(new project { projectId = r.projectId, ClientId = r.ClientId, createdAt = r.createdAt , Name = r.Name });

            }
           
            return View(CP);
        }

        // GET: projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }
        // id is the client id
        // GET: projects/Create/3
        public ActionResult Create(int? id)
        {
            var list = new List<Client>();
            var cl = db.Clients.Find(id);
            list.Add(new Client { clientId = cl.clientId, Name = cl.Name });

            ViewBag.ClientId = new SelectList(list, "clientId", "Name");
            return View();
        }

        // POST: projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "projectId,ClientId,Name,createdAt")] project project)
        {
            if (ModelState.IsValid)
            {
                db.projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("clientProjects", new { id = project.ClientId });
            }

            ViewBag.ClientId = new SelectList(db.Clients, "clientId", "Name", project.ClientId);
            return View(project);
        }

        // GET: projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "clientId", "Name", project.ClientId);
            return View(project);
        }

        // POST: projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "projectId,ClientId,Name,createdAt")] project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("clientProjects", new { id = project.ClientId });
            }
            ViewBag.ClientId = new SelectList(db.Clients, "clientId", "Name", project.ClientId);
            return View(project);
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
