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
    public class TaskController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Task
        public ActionResult Index()
        {
            var c005_TASK_DATA = db.C005_TASK_DATA.Where(c => c.OwnerId == UserIdentity.UserId).Include(c => c.C010_AREA).Include(c => c.EndUser).Include(c => c.EndUser1).Include(c => c.C015_STATUS).Include(c => c.C011_SUB_AREA).Include(c => c.C014_TASK_TYPE).Include(c => c.C004_BUCKET);
            return View(c005_TASK_DATA.ToList());
        }

        // GET: Task/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C005_TASK_DATA c005_TASK_DATA = db.C005_TASK_DATA.Find(id);
            if (c005_TASK_DATA == null)
            {
                return HttpNotFound();
            }
            return View(c005_TASK_DATA);
        }

        // GET: Task/Create
        public ActionResult Create()
        {
            ViewBag.AreaId = new SelectList(db.C010_AREA, "AreaId", "AreaName");
            ViewBag.GeneratedBy = new SelectList(db.EndUsers, "UID", "UserName");
            ViewBag.OwnerId = new SelectList(db.EndUsers, "UID", "UserName");
            ViewBag.StatusId = new SelectList(db.C015_STATUS, "StatusId", "TaskStatus");
            ViewBag.SubAreaId = new SelectList(db.C011_SUB_AREA, "SubAreaId", "SubAreaName");
            ViewBag.TypeId = new SelectList(db.C014_TASK_TYPE, "TypeId", "TypeName");
            ViewBag.BucketId = new SelectList(db.C004_BUCKET, "BucketId", "Name");
            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaskId,BucketId,AreaId,SubAreaId,SSubAreaId,SName,Description,StartDate,Deadline,OrderId,ManDays,OwnerId,GeneratedBy,GeneratedDate,DocsLink,TypeId,StatusId,IsActive")] C005_TASK_DATA c005_TASK_DATA)
        {
            if (ModelState.IsValid)
            {
                db.C005_TASK_DATA.Add(c005_TASK_DATA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreaId = new SelectList(db.C010_AREA, "AreaId", "AreaName", c005_TASK_DATA.AreaId);
            ViewBag.GeneratedBy = new SelectList(db.EndUsers, "UID", "UserName", c005_TASK_DATA.GeneratedBy);
            ViewBag.OwnerId = new SelectList(db.EndUsers, "UID", "UserName", c005_TASK_DATA.OwnerId);
            ViewBag.StatusId = new SelectList(db.C015_STATUS, "StatusId", "TaskStatus", c005_TASK_DATA.StatusId);
            ViewBag.SubAreaId = new SelectList(db.C011_SUB_AREA, "SubAreaId", "SubAreaName", c005_TASK_DATA.SubAreaId);
            ViewBag.TypeId = new SelectList(db.C014_TASK_TYPE, "TypeId", "TypeName", c005_TASK_DATA.TypeId);
            ViewBag.BucketId = new SelectList(db.C004_BUCKET, "BucketId", "Name", c005_TASK_DATA.BucketId);
            return View(c005_TASK_DATA);
        }

        // GET: Task/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C005_TASK_DATA c005_TASK_DATA = db.C005_TASK_DATA.Find(id);
            if (c005_TASK_DATA == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaId = new SelectList(db.C010_AREA, "AreaId", "AreaName", c005_TASK_DATA.AreaId);
            ViewBag.GeneratedBy = new SelectList(db.EndUsers, "UID", "UserName", c005_TASK_DATA.GeneratedBy);
            ViewBag.OwnerId = new SelectList(db.EndUsers, "UID", "UserName", c005_TASK_DATA.OwnerId);
            ViewBag.StatusId = new SelectList(db.C015_STATUS, "StatusId", "TaskStatus", c005_TASK_DATA.StatusId);
            ViewBag.SubAreaId = new SelectList(db.C011_SUB_AREA, "SubAreaId", "SubAreaName", c005_TASK_DATA.SubAreaId);
            ViewBag.TypeId = new SelectList(db.C014_TASK_TYPE, "TypeId", "TypeName", c005_TASK_DATA.TypeId);
            ViewBag.BucketId = new SelectList(db.C004_BUCKET, "BucketId", "Name", c005_TASK_DATA.BucketId);
            return View(c005_TASK_DATA);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskId,BucketId,AreaId,SubAreaId,SSubAreaId,SName,Description,StartDate,Deadline,OrderId,ManDays,OwnerId,GeneratedBy,GeneratedDate,DocsLink,TypeId,StatusId,IsActive")] C005_TASK_DATA c005_TASK_DATA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c005_TASK_DATA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreaId = new SelectList(db.C010_AREA, "AreaId", "AreaName", c005_TASK_DATA.AreaId);
            ViewBag.GeneratedBy = new SelectList(db.EndUsers, "UID", "UserName", c005_TASK_DATA.GeneratedBy);
            ViewBag.OwnerId = new SelectList(db.EndUsers, "UID", "UserName", c005_TASK_DATA.OwnerId);
            ViewBag.StatusId = new SelectList(db.C015_STATUS, "StatusId", "TaskStatus", c005_TASK_DATA.StatusId);
            ViewBag.SubAreaId = new SelectList(db.C011_SUB_AREA, "SubAreaId", "SubAreaName", c005_TASK_DATA.SubAreaId);
            ViewBag.TypeId = new SelectList(db.C014_TASK_TYPE, "TypeId", "TypeName", c005_TASK_DATA.TypeId);
            ViewBag.BucketId = new SelectList(db.C004_BUCKET, "BucketId", "Name", c005_TASK_DATA.BucketId);
            return View(c005_TASK_DATA);
        }

        // GET: Task/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C005_TASK_DATA c005_TASK_DATA = db.C005_TASK_DATA.Find(id);
            if (c005_TASK_DATA == null)
            {
                return HttpNotFound();
            }
            return View(c005_TASK_DATA);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C005_TASK_DATA c005_TASK_DATA = db.C005_TASK_DATA.Find(id);
            db.C005_TASK_DATA.Remove(c005_TASK_DATA);
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
