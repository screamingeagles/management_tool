using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ManagementTool.Common;
using ManagementTool.Models;
using System.Collections.Generic;

namespace ManagementTool.Controllers
{
    public class ProjectsController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Projects
        public ActionResult Index()
        {
            //var q = (from p in db.C001_PROJECT
            //         join e in db.EndUsers on p.CreatedBy equals e.UID
            //         where p.IsActive == true
            //         select new {
            //             ProjetId       = p.ProjectId,
            //             ProjectName    = p.ProjectName,
            //             CreatedDate    = p.CreatedDate,
            //             CreatedName    = e.UserName
            //         }).ToList();

            //return View(q);
            return View(db.C001_PROJECT.ToList());
        }

        public ActionResult Add()
        {
            projectmodel pm = new projectmodel(0);
            return View(pm);
        }

        [HttpPost]
        public ActionResult Save(FormCollection frm)
        {
            if (ModelState.IsValid) {
                string pname = "";
                // pname = "arsalan";
                // c001_PROJECT.CreatedBy = 1020;
                // db.C001_PROJECT.Add(c001_PROJECT);
                // db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetCompaniesbyLocation(int SelectedLocation)
        {
            using (ProjectEntities _db = new ProjectEntities())
            {
                var q = (from c in _db.C005_COMPANY
                         where (c.IsActive.Equals(true)) && (c.LocationId == SelectedLocation)
                         select new { c.CompanyId, c.CompanyName }).ToList();
                return Json(new { data = q });
            }
        }



        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C001_PROJECT c001_PROJECT = db.C001_PROJECT.Find(id);
            if (c001_PROJECT == null)
            {
                return HttpNotFound();
            }
            return View(c001_PROJECT);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,ProjectName,CreatedBy,CreatedDate,IsActive")] C001_PROJECT c001_PROJECT)
        {
            if (ModelState.IsValid)
            {
                db.C001_PROJECT.Add(c001_PROJECT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c001_PROJECT);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C001_PROJECT c001_PROJECT = db.C001_PROJECT.Find(id);
            if (c001_PROJECT == null)
            {
                return HttpNotFound();
            }
            return View(c001_PROJECT);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,ProjectName,CreatedBy,CreatedDate,IsActive")] C001_PROJECT c001_PROJECT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c001_PROJECT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c001_PROJECT);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C001_PROJECT c001_PROJECT = db.C001_PROJECT.Find(id);
            if (c001_PROJECT == null)
            {
                return HttpNotFound();
            }
            return View(c001_PROJECT);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C001_PROJECT c001_PROJECT = db.C001_PROJECT.Find(id);
            db.C001_PROJECT.Remove(c001_PROJECT);
            db.SaveChanges();
            return RedirectToAction("Index");
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
