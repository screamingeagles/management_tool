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
    public class SubAreaController : BaseController
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: SubArea
        public ActionResult Index()
        {
            
            int area = 0;
            int division = 0;
            List<C003_SUB_AREA> sa;
            string div = (Request.Form["DivisionId"]== null) ? "" : Request.Form["DivisionId"]  .ToString();
            string ara = (Request.Form["AreaId"]    == null) ? "" : Request.Form["AreaId"]      .ToString();

            if (string.IsNullOrEmpty(div)) {
                ViewBag.DivisionId = new SelectList(db.C001_DIVISION.Where(d => d.IsActive == true).OrderBy(d => d.DivisionName), "DivisionId", "DivisionName");
            }
            else {
                division           = Convert.ToInt32(div);
                ViewBag.DivisionId = new SelectList(db.C001_DIVISION.Where(d => d.IsActive == true).OrderBy(d => d.DivisionName), "DivisionId", "DivisionName", Convert.ToInt32(div));
            }

            if (string.IsNullOrEmpty(ara)) {
                ViewBag.AreaId  = new SelectList(db.C002_AREA.Where(a => a.isActive == true).Take(0), "AreaId", "AreaName");
                sa              = db.C003_SUB_AREA.Include(c => c.C002_AREA).Include(c => c.EndUser).ToList();
            }
            else {
                area            = Convert.ToInt32(ara);                
                ViewBag.AreaId  = new SelectList(db.vw_SubAreaListByDivision.Where(a => a.DivisionId == division), "AreaId", "AreaName", area);
                sa              = db.C003_SUB_AREA.Include(c => c.C002_AREA).Include(c => c.EndUser).Where(s => s.AreaId == area) .ToList();
            }
            ViewBag.DivId       = div;            
            return View(sa);
        }

        // GET: SubArea/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C003_SUB_AREA c003_SUB_AREA = db.C003_SUB_AREA.Find(id);
            if (c003_SUB_AREA == null)
            {
                return HttpNotFound();
            }
            return View(c003_SUB_AREA);
        }

        // GET: SubArea/Create
        public ActionResult Create(int? id)
        {
            if (id.HasValue) {
                ViewBag.AreaId = new SelectList(db.C002_AREA, "AreaId", "AreaName", id.Value);
            } else {
                ViewBag.AreaId = new SelectList(db.C002_AREA, "AreaId", "AreaName");
            }            
            return View();
        }

        // POST: SubArea/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AreaId,SubAreaName")] C003_SUB_AREA c003_SUB_AREA)
        {
            if (ModelState.IsValid)
            {
                c003_SUB_AREA.GeneratedBy   = UserIdentity.UserId;
                c003_SUB_AREA.GeneratedDate = DateTime.Now.AddHours(4);
                c003_SUB_AREA.AreaActive    = true;
                db.C003_SUB_AREA.Add(c003_SUB_AREA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreaId = new SelectList(db.C002_AREA, "AreaId", "AreaName", c003_SUB_AREA.AreaId);
            return View(c003_SUB_AREA);
        }

        // GET: SubArea/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C003_SUB_AREA c003_SUB_AREA = db.C003_SUB_AREA.Find(id);
            if (c003_SUB_AREA == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaId = new SelectList(db.C002_AREA, "AreaId", "AreaName", c003_SUB_AREA.AreaId);
            return View(c003_SUB_AREA);
        }

        // POST: SubArea/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubAreaId,AreaId,SubAreaName")] C003_SUB_AREA c003_SUB_AREA)
        {
            if (ModelState.IsValid)
            {
                c003_SUB_AREA.GeneratedBy   = UserIdentity.UserId;
                c003_SUB_AREA.GeneratedDate = DateTime.Now.AddHours(4);
                c003_SUB_AREA.AreaActive    = true;

                db.Entry(c003_SUB_AREA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreaId = new SelectList(db.C002_AREA, "AreaId", "AreaName", c003_SUB_AREA.AreaId);
            return View(c003_SUB_AREA);
        }

        // GET: SubArea/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C003_SUB_AREA c003_SUB_AREA = db.C003_SUB_AREA.Find(id);
            if (c003_SUB_AREA == null)
            {
                return HttpNotFound();
            }

            #region  Project
            Dictionary<int, string[]> projects = new Dictionary<int, string[]>();
            var pj = (from p in db.C004_PROJECT
                     join e in db.EndUsers on p.GeneratedBy equals e.UID
                     where (p.SubAreaId == id)
                     select new { ID = p.ProjectId, IName = p.ProjectName, Owner = e.UserName, Generated = p.GeneratedDate }).ToList();
            foreach (var item in pj) {
                projects.Add(item.ID, new string[] { item.IName, item.Owner, item.Generated.ToString("dd-MMM-yyyy") });
            }
            ViewBag.Project = projects;
            #endregion  


            return View(c003_SUB_AREA);
        }

        // POST: SubArea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C003_SUB_AREA c003_SUB_AREA     = db.C003_SUB_AREA.Find(id);
            c003_SUB_AREA.GeneratedBy       = UserIdentity.UserId;
            c003_SUB_AREA.GeneratedDate     = DateTime.Now.AddHours(4);
            c003_SUB_AREA.AreaActive        = false;
            db.Entry(c003_SUB_AREA).State   = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Bucket");

            //db.C003_SUB_AREA.Remove(c003_SUB_AREA);
            //db.SaveChanges();
            //return RedirectToAction("Index");
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
