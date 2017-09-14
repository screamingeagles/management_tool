using System;
using System.Web;
using System.Net;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using ManagementTool.Common;
using ManagementTool.Models;
using System.Collections.Generic;


namespace ManagementTool.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Project
        public ActionResult Index()
        {
            return View(db.C004_PROJECT.ToList());
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C004_PROJECT c004_PROJECT = db.C004_PROJECT.Find(id);
            if (c004_PROJECT == null)
            {
                return HttpNotFound();
            }
            return View(c004_PROJECT);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            //UserIdentity.UserId = 1020;
            //UserIdentity.UserName = "Arslan";
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,DivisionId,AreaId,SubAreaId,ProjectName,ProjectType,StartDate,EndDate,GeneratedBy,GeneratedDate,IsActive")] C004_PROJECT c004_PROJECT)
        {
            if (ModelState.IsValid)
            {
                db.C004_PROJECT.Add(c004_PROJECT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c004_PROJECT);
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C004_PROJECT c004_PROJECT = db.C004_PROJECT.Find(id);
            if (c004_PROJECT == null)
            {
                return HttpNotFound();
            }
            return View(c004_PROJECT);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,DivisionId,AreaId,SubAreaId,ProjectName,ProjectType,StartDate,EndDate,GeneratedBy,GeneratedDate,IsActive")] C004_PROJECT c004_PROJECT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c004_PROJECT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c004_PROJECT);
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C004_PROJECT c004_PROJECT = db.C004_PROJECT.Find(id);
            if (c004_PROJECT == null)
            {
                return HttpNotFound();
            }
            return View(c004_PROJECT);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C004_PROJECT c004_PROJECT = db.C004_PROJECT.Find(id);
            db.C004_PROJECT.Remove(c004_PROJECT);
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
