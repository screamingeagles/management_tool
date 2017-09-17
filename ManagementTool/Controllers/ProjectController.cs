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

        [HttpPost]
        public JsonResult GetSubAreaByArea(int SelectedArea)
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from c in db.C003_SUB_AREA
                         where (c.AreaActive.Equals(true)) && (c.AreaId == SelectedArea)
                         select new { c.SubAreaId, c.SubAreaName }).ToList();
                return Json(new { data = q });
            }
        }

        [HttpPost]
        public JsonResult GetAreaByDivision(int SelectedDivision)
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from c in db.C002_AREA
                         where (c.isActive.Equals(true)) && (c.DivisionId == SelectedDivision)
                         select new { c.AreaId, c.AreaName }).ToList();
                return Json(new { data = q });
            }
        }


        // GET: Project
        public ActionResult Index()
        {
            var c004_PROJECT = db.C004_PROJECT.Include(c => c.C001_DIVISION).Include(c => c.C002_AREA).Include(c => c.EndUser).Include(c => c.C013_PROJECT_TYPE);
            return View(c004_PROJECT.ToList());
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

            ViewBag.SubAreaId = Bhai.getSubArea(c004_PROJECT.SubAreaId);
            return View(c004_PROJECT);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            UserIdentity.UserId = 1020;
            UserIdentity.UserName = "Arsalan (RTT)";

            ViewBag.AreaId          = new SelectList(db.C002_AREA            , "AreaId", "AreaName");
            ViewBag.SubAreaId       = new SelectList(db.C003_SUB_AREA.Take(5), "SubAreaId", "SubAreaName");
            ViewBag.DivisionId      = new SelectList(db.C001_DIVISION        , "DivisionId", "DivisionName");
            ViewBag.ProjectType     = new SelectList(db.C013_PROJECT_TYPE    , "ProjectTypeId", "ProjectType");

            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DivisionId,AreaId,SubAreaId,ProjectName,ProjectType,StartDate,EndDate")] C004_PROJECT c004_PROJECT)
        {
            if ((c004_PROJECT.DivisionId > 0) && (c004_PROJECT.AreaId > 0) && (c004_PROJECT.ProjectName != "")) //(ModelState.IsValid)
            {
                c004_PROJECT.GeneratedBy    = UserIdentity.UserId;
                c004_PROJECT.GeneratedDate  = DateTime.Now.AddHours(4);
                c004_PROJECT.IsActive       = true;

                db.C004_PROJECT.Add(c004_PROJECT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreaId      = new SelectList(db.C002_AREA               , "AreaId", "AreaName");
            ViewBag.SubAreaId   = new SelectList(db.C003_SUB_AREA.Take(5)   , "SubAreaId", "SubAreaName");
            ViewBag.DivisionId  = new SelectList(db.C001_DIVISION           , "DivisionId", "DivisionName");
            ViewBag.ProjectType = new SelectList(db.C013_PROJECT_TYPE       , "ProjectTypeId", "ProjectType");
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

            ViewBag.DivisionId  = new SelectList(db.C001_DIVISION, "DivisionId", "DivisionName", c004_PROJECT.DivisionId);
            ViewBag.AreaId      = new SelectList(db.C002_AREA.Where(c => c.DivisionId == c004_PROJECT.DivisionId), "AreaId", "AreaName", c004_PROJECT.AreaId);
            ViewBag.SubAreaId = new SelectList(db.C003_SUB_AREA.Where(c => c.AreaId   == c004_PROJECT.AreaId)   , "SubAreaId", "SubAreaName", c004_PROJECT.SubAreaId);
            ViewBag.ProjectType = new SelectList(db.C013_PROJECT_TYPE, "ProjectTypeId", "ProjectType", c004_PROJECT.ProjectType);
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
                c004_PROJECT.GeneratedBy    = UserIdentity.UserId;
                c004_PROJECT.GeneratedDate  = DateTime.Now.AddHours(4);
                c004_PROJECT.IsActive       = true;

                db.Entry(c004_PROJECT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DivisionId  = new SelectList(db.C001_DIVISION, "DivisionId", "DivisionName", c004_PROJECT.DivisionId);
            ViewBag.AreaId      = new SelectList(db.C002_AREA.Where(c => c.DivisionId == c004_PROJECT.DivisionId), "AreaId", "AreaName", c004_PROJECT.AreaId);
            ViewBag.SubAreaId   = new SelectList(db.C003_SUB_AREA.Where(c => c.AreaId == c004_PROJECT.AreaId), "SubAreaId", "SubAreaName", c004_PROJECT.SubAreaId);
            ViewBag.ProjectType = new SelectList(db.C013_PROJECT_TYPE, "ProjectTypeId", "ProjectType", c004_PROJECT.ProjectType);
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
            ViewBag.SubAreaId = Bhai.getSubArea(c004_PROJECT.SubAreaId);
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
