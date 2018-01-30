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
    public class PhaseController : BaseController
    {
        private ProjectEntities db = new ProjectEntities();

        [HttpPost]
        public JsonResult GetProjectDates           (int SelectedProject)   
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from c in db.C004_PROJECT
                         where (c.IsActive.Equals(true)) && (c.ProjectId == SelectedProject)
                         select new { c.StartDate, c.EndDate}).FirstOrDefault();

                return Json(new { data = q });
            }
        }

        [HttpPost]
        public JsonResult GetPhaseDates             (int PhaseId)           
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from c in db.C005_PHASE
                         where (c.IsActive.Equals(true)) && (c.PhaseId == PhaseId)
                         select new { c.StartDate, c.EndDate }).FirstOrDefault();
                return Json(new { data = q });
            }
        }
        
        [HttpPost]
        public JsonResult GetProjectPhases          (int ProjectId)         
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from p in db.C005_PHASE
                         where (p.ProjectId == ProjectId)
                         select new { p.PhaseId, p.PhaseName}).ToList();
                return Json(new { data = q });
            }
        }
        
        [HttpPost]
        public JsonResult GetCompanyByLocation      (int LocId)             {
            using (ProjectEntities db = new ProjectEntities()) {
                var q = (from c in db.C011_COMPANY
                         where (c.LocationId == LocId) && (c.IsActive == true)
                         select new { c.CompanyId, c.CompanyName}).ToList();
                return Json(new { data = q });
            }
        }
        
        [HttpPost]
        public JsonResult GetAreaByDivision         (int DivId)             {
            using (ProjectEntities db = new ProjectEntities()) {
                var q = (from a in db.C002_AREA
                         where (a.DivisionId == DivId) && (a.isActive == true)
                         select new { a.AreaId, a.AreaName}).ToList();
                return Json(new { data = q });
            }
        }

        [HttpPost]
        public JsonResult GetPhaseBySubArea         (int Area, int SubArea) 
        {
            using (ProjectEntities db = new ProjectEntities()) {
                var q = (from p in db.C005_PHASE
                         join v in db.vw_PhasebyLCDASAP  on p.PhaseId equals v.PhaseId
                         join po in db.C004_PROJECT on v.ProjectId equals po.ProjectId
                         join u in db.EndUsers on p.GeneratedBy equals u.UID
                         where (v.AreaId == Area) && (v.SubAreaId == SubArea)
                         select new { p.PhaseId, po.ProjectName, p.PhaseName, p.StartDate, p.EndDate , p.IsActive, u.UserName, p.GeneratedDate}).ToList();

                var k = (from j in db.C004_PROJECT
                         where (j.AreaId == Area) && (j.SubAreaId == SubArea)
                         select new { j.ProjectId, j.ProjectName }).ToList();

                return Json(new { data = k, list = q });

            }
        }

        [HttpPost]
        public JsonResult GetPhaseByProject         (int ProjectId)         
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from p in db.C005_PHASE
                         join v in db.vw_PhasebyLCDASAP on p.PhaseId equals v.PhaseId
                         join po in db.C004_PROJECT on v.ProjectId equals po.ProjectId
                         join u in db.EndUsers on p.GeneratedBy equals u.UID
                         where (v.ProjectId == ProjectId) 
                         select new { p.PhaseId, po.ProjectName, p.PhaseName, p.StartDate, p.EndDate, p.IsActive, u.UserName, p.GeneratedDate }).ToList();
                
                return Json(new { data = q});

            }
        }

        [HttpPost]
        public JsonResult GetPhaseByCompany         (int CompanyId)         
        {
            using (ProjectEntities db = new ProjectEntities()) {

                var q = (from p  in db.C005_PHASE
                         join v  in db.vw_PhasebyLCDASAP    on p.PhaseId equals v.PhaseId
                         join po in db.C004_PROJECT         on v.ProjectId equals po.ProjectId
                         join u  in db.EndUsers             on p.GeneratedBy equals u.UID
                         where (v.CompanyId == CompanyId)
                         select new { p.PhaseId, po.ProjectName, p.PhaseName, p.StartDate, p.EndDate, p.IsActive, u.UserName, p.GeneratedDate }).ToList();

                return Json(new { data = q });

            }
        }

        // GET: Phase
        public ActionResult Index()
        {
            ViewBag.LocationId  = new SelectList(db.C010_LOCATION.Where(l => l.IsActive == true).OrderBy(l => l.LocationName), "LocationId", "LocationName");
            ViewBag.CompanyId   = new SelectList(db.C011_COMPANY.Take(0),                       "CompanyId" , "CompanyName");

            ViewBag.DivisionId  = new SelectList(db.C001_DIVISION.Where(d => d.IsActive == true).OrderBy(d => d.DivisionName), "DivisionId", "DivisionName");
            ViewBag.AreaId      = new SelectList(db.C002_AREA.      Take(0), "AreaId"   , "AreaName");
            ViewBag.SubAreaId   = new SelectList(db.C003_SUB_AREA.  Take(0), "SubAreaId", "SubAreaName");
            ViewBag.ProjectId   = new SelectList(db.C004_PROJECT.   Take(0), "ProjectId", "ProjectName");            

            var c005_PHASE = db.C005_PHASE.Include(c => c.EndUser).OrderBy(x => x.PhaseName).Take(0);
            return View(c005_PHASE.ToList());
        }

        // GET: Phase/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C005_PHASE c005_PHASE = db.C005_PHASE.Find(id);
            if (c005_PHASE == null)
            {
                return HttpNotFound();
            }
            return View(c005_PHASE);
        }

        // GET: Phase/Create
        public ActionResult Create(){
            
            #region Project
                int lid = (string.IsNullOrEmpty(Request.QueryString["prj"]) == false) ? Convert.ToInt32(Request.QueryString["prj"].ToString()) : 0;            
                if (lid > 0 ) {
                    ViewBag.ProjectId = new SelectList(db.C004_PROJECT.Where(p => p.IsActive == true), "ProjectId", "ProjectName", lid);
                }
                else {
                    ViewBag.ProjectId = new SelectList(db.C004_PROJECT.Where(p => p.IsActive == true), "ProjectId", "ProjectName");
                }
            #endregion
                            
            return View();
        }

        // POST: Phase/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,PhaseName,StartDate,EndDate")] C005_PHASE c005_PHASE)
        {
            if (ModelState.IsValid)
            {
                c005_PHASE.GeneratedBy      = UserIdentity.UserId;
                c005_PHASE.GeneratedDate    = DateTime.Now.AddHours(4);
                c005_PHASE.IsActive         = true;

                db.C005_PHASE.Add(c005_PHASE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.C004_PROJECT.Where(p => p.IsActive == true), "ProjectId", "ProjectName");
            return View(c005_PHASE);
        }

        // GET: Phase/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            C005_PHASE c005_PHASE = db.C005_PHASE.Find(id);
            if (c005_PHASE == null) {
                return HttpNotFound();
            }

            ViewBag.EndDate     = "";
            ViewBag.StartDate   = "";
            if (id.HasValue)
            {
                var q = (from c in db.C004_PROJECT
                         where (c.IsActive.Equals(true)) && (c.ProjectId == id.Value)
                         select new { c.StartDate, c.EndDate }).FirstOrDefault();
                ViewBag.EndDate     = q.EndDate.ToString("dd-MMM-yyyy");
                ViewBag.StartDate   = q.StartDate.ToString("dd-MMM-yyyy");
            }
            ViewBag.ProjectId = new SelectList(db.C004_PROJECT.Where(p => p.IsActive == true), "ProjectId", "ProjectName", c005_PHASE.PhaseId);
            return View(c005_PHASE);
        }

        // POST: Phase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhaseId,ProjectId,PhaseName,StartDate,EndDate")] C005_PHASE c005_PHASE)
        {
            if (ModelState.IsValid)
            {
                c005_PHASE.GeneratedBy = UserIdentity.UserId;
                c005_PHASE.GeneratedDate = DateTime.Now.AddHours(4);
                c005_PHASE.IsActive = true;

                db.Entry(c005_PHASE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var q = (from c in db.C004_PROJECT
                        where (c.IsActive.Equals(true)) && (c.ProjectId == c005_PHASE.ProjectId)
                        select new { c.StartDate, c.EndDate }).FirstOrDefault();
            ViewBag.EndDate = q.EndDate.ToString("dd-MMM-yyyy");
            ViewBag.StartDate = q.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.ProjectId = new SelectList(db.C004_PROJECT.Where(p => p.IsActive == true), "ProjectId", "ProjectName", c005_PHASE.PhaseId);
            return View(c005_PHASE);
        }

        // GET: Phase/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C005_PHASE c005_PHASE = db.C005_PHASE.Find(id);
            if (c005_PHASE == null)
            {
                return HttpNotFound();
            }

            Dictionary<int, string[]> sbphase = new Dictionary<int, string[]>();
            Dictionary<int, string[]> dbucket = new Dictionary<int, string[]>();

            var sphase = (from sp in db.C006_SubPhase
                            join e in db.EndUsers on sp.GeneratedBy equals e.UID
                         where (sp.PhaseId == id)
                         select new { ID = sp.SubPhaseId, IName = sp.SubPhaseName, Owner = e.UserName, Generated = sp.GeneratedDate }).ToList();
            foreach (var item in sphase){
                sbphase.Add(item.ID, new string[] { item.IName, item.Owner, item.Generated.ToString("dd-MMM-yyyy") });
            }
            ViewBag.sbphase=sbphase;

            var bucket = (from b in db.C007_BUCKET
                          join e in db.EndUsers on b.GeneratedBy equals e.UID
                          where ((b.PhaseId == id) && (b.IsActive == true))
                          select new { ID = b.BucketId, IName = b.Name, Owner = e.UserName, Generated = b.GenerationDate }).ToList();
            foreach (var item in bucket) {
                dbucket.Add(item.ID, new string[] { item.IName, item.Owner, item.Generated.ToString("dd-MMM-yyyy") });
            }
            ViewBag.Bucket = dbucket;


            return View(c005_PHASE);
        }

        // POST: Phase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            C005_PHASE c005_PHASE       = db.C005_PHASE.Find(id);
            c005_PHASE.GeneratedBy      = UserIdentity.UserId;
            c005_PHASE.GeneratedDate    = DateTime.Now.AddHours(4);
            c005_PHASE.IsActive         = false;
            db.Entry(c005_PHASE).State  = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Phase");

            // db.C005_PHASE.Remove(c005_PHASE);
            // db.SaveChanges();
            // return RedirectToAction("Index");
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
