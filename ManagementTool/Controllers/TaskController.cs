using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementTool.Models;

namespace ManagementTool.Controllers
{
    public class TaskController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Task
        public ActionResult Index()
        {
            var c002_TASK_DATA = db.C002_TASK_DATA.Include(c => c.C001_PROJECT).Include(c => c.C004_LOCATION).Include(c => c.C010_STATUS).Include(c => c.C008_SUB_AREA).Include(c => c.C009_TYPE).Include(c => c.C007_AREA).Include(c => c.C005_COMPANY).Include(c => c.C006_DIVISION);
            return View(c002_TASK_DATA.ToList());
        }

        // GET: Task/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C002_TASK_DATA c002_TASK_DATA = db.C002_TASK_DATA.Find(id);
            if (c002_TASK_DATA == null)
            {
                return HttpNotFound();
            }
            return View(c002_TASK_DATA);
        }

        // GET: Task/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.C001_PROJECT, "ProjectId", "ProjectName");
            ViewBag.LocationId = new SelectList(db.C004_LOCATION, "LocationId", "LocationName");
            ViewBag.StatusId = new SelectList(db.C010_STATUS, "StatusId", "TaskStatus");
            ViewBag.SubAreaId = new SelectList(db.C008_SUB_AREA, "SubAreaId", "SubAreaName");
            ViewBag.TypeId = new SelectList(db.C009_TYPE, "TypeId", "TypeName");
            ViewBag.AreaId = new SelectList(db.C007_AREA, "AreaId", "AreaName");
            ViewBag.CompanyId = new SelectList(db.C005_COMPANY, "CompanyId", "CompanyName");
            ViewBag.DivisionId = new SelectList(db.C006_DIVISION, "DivisionId", "DivisionName");
            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaskId,ProjectId,LocationId,CompanyId,DivisionId,AreaId,SubAreaId,SName,Description,StartDate,Deadline,ManDays,ServiceOwnerId,CreatedById,CreateByName,CreatedDate,DocsLink,TypeId,StatusId,IsActive")] C002_TASK_DATA c002_TASK_DATA)
        {
            if (ModelState.IsValid)
            {
                db.C002_TASK_DATA.Add(c002_TASK_DATA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.C001_PROJECT, "ProjectId", "ProjectName", c002_TASK_DATA.ProjectId);
            ViewBag.LocationId = new SelectList(db.C004_LOCATION, "LocationId", "LocationName", c002_TASK_DATA.LocationId);
            ViewBag.StatusId = new SelectList(db.C010_STATUS, "StatusId", "TaskStatus", c002_TASK_DATA.StatusId);
            ViewBag.SubAreaId = new SelectList(db.C008_SUB_AREA, "SubAreaId", "SubAreaName", c002_TASK_DATA.SubAreaId);
            ViewBag.TypeId = new SelectList(db.C009_TYPE, "TypeId", "TypeName", c002_TASK_DATA.TypeId);
            ViewBag.AreaId = new SelectList(db.C007_AREA, "AreaId", "AreaName", c002_TASK_DATA.AreaId);
            ViewBag.CompanyId = new SelectList(db.C005_COMPANY, "CompanyId", "CompanyName", c002_TASK_DATA.CompanyId);
            ViewBag.DivisionId = new SelectList(db.C006_DIVISION, "DivisionId", "DivisionName", c002_TASK_DATA.DivisionId);
            return View(c002_TASK_DATA);
        }

        // GET: Task/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C002_TASK_DATA c002_TASK_DATA = db.C002_TASK_DATA.Find(id);
            if (c002_TASK_DATA == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.C001_PROJECT, "ProjectId", "ProjectName", c002_TASK_DATA.ProjectId);
            ViewBag.LocationId = new SelectList(db.C004_LOCATION, "LocationId", "LocationName", c002_TASK_DATA.LocationId);
            ViewBag.StatusId = new SelectList(db.C010_STATUS, "StatusId", "TaskStatus", c002_TASK_DATA.StatusId);
            ViewBag.SubAreaId = new SelectList(db.C008_SUB_AREA, "SubAreaId", "SubAreaName", c002_TASK_DATA.SubAreaId);
            ViewBag.TypeId = new SelectList(db.C009_TYPE, "TypeId", "TypeName", c002_TASK_DATA.TypeId);
            ViewBag.AreaId = new SelectList(db.C007_AREA, "AreaId", "AreaName", c002_TASK_DATA.AreaId);
            ViewBag.CompanyId = new SelectList(db.C005_COMPANY, "CompanyId", "CompanyName", c002_TASK_DATA.CompanyId);
            ViewBag.DivisionId = new SelectList(db.C006_DIVISION, "DivisionId", "DivisionName", c002_TASK_DATA.DivisionId);
            return View(c002_TASK_DATA);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskId,ProjectId,LocationId,CompanyId,DivisionId,AreaId,SubAreaId,SName,Description,StartDate,Deadline,ManDays,ServiceOwnerId,CreatedById,CreateByName,CreatedDate,DocsLink,TypeId,StatusId,IsActive")] C002_TASK_DATA c002_TASK_DATA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c002_TASK_DATA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.C001_PROJECT, "ProjectId", "ProjectName", c002_TASK_DATA.ProjectId);
            ViewBag.LocationId = new SelectList(db.C004_LOCATION, "LocationId", "LocationName", c002_TASK_DATA.LocationId);
            ViewBag.StatusId = new SelectList(db.C010_STATUS, "StatusId", "TaskStatus", c002_TASK_DATA.StatusId);
            ViewBag.SubAreaId = new SelectList(db.C008_SUB_AREA, "SubAreaId", "SubAreaName", c002_TASK_DATA.SubAreaId);
            ViewBag.TypeId = new SelectList(db.C009_TYPE, "TypeId", "TypeName", c002_TASK_DATA.TypeId);
            ViewBag.AreaId = new SelectList(db.C007_AREA, "AreaId", "AreaName", c002_TASK_DATA.AreaId);
            ViewBag.CompanyId = new SelectList(db.C005_COMPANY, "CompanyId", "CompanyName", c002_TASK_DATA.CompanyId);
            ViewBag.DivisionId = new SelectList(db.C006_DIVISION, "DivisionId", "DivisionName", c002_TASK_DATA.DivisionId);
            return View(c002_TASK_DATA);
        }

        // GET: Task/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C002_TASK_DATA c002_TASK_DATA = db.C002_TASK_DATA.Find(id);
            if (c002_TASK_DATA == null)
            {
                return HttpNotFound();
            }
            return View(c002_TASK_DATA);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C002_TASK_DATA c002_TASK_DATA = db.C002_TASK_DATA.Find(id);
            db.C002_TASK_DATA.Remove(c002_TASK_DATA);
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
