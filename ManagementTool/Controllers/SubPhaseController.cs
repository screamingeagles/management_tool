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
    public class SubPhaseController : BaseController
    {
        private ProjectEntities db = new ProjectEntities();

        [HttpPost]
        public JsonResult GetSubPhase                   (int PhaseId)           
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from c in db.C006_SubPhase
                         where (c.PhaseId == PhaseId)
                         select new { c.SubPhaseId, c.SubPhaseName}).ToList();
                return Json(new { data = q });
            }
        }

        [HttpPost]
        public JsonResult GetSubPhaseByCompany          (int CompanyId)         
        {
            using (ProjectEntities db = new ProjectEntities()) {
                var q = (from    sp     in db.vw_SubPhasebyLCDASAP 
                            join P in db.C005_PHASE         on sp.PhaseId       equals P.PhaseId
                            join po     in db.C004_PROJECT  on sp.ProjectId     equals po.ProjectId
                            join u      in db.EndUsers      on sp.GeneratedBy   equals u.UID
                         where (sp.CompanyId == CompanyId)
                         select new { sp.SubPhaseId, po.ProjectName, P.PhaseName, sp.SubPhaseName, sp.StartDate, sp.EndDate, u.UserName, sp.GeneratedDate }).ToList();

                return Json(new { data = q });

            }
        }

        [HttpPost]
        public JsonResult GetSubPhaseBySubArea          (int Area, int SubArea) 
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from sp    in db.vw_SubPhasebyLCDASAP
                         join P     in db.C005_PHASE            on sp.PhaseId       equals P.PhaseId
                         join po    in db.C004_PROJECT          on sp.ProjectId     equals po.ProjectId
                         join u     in db.EndUsers              on sp.GeneratedBy   equals u.UID
                         where (sp.AreaId == Area) && (sp.SubAreaId == SubArea)
                         select new { sp.SubPhaseId, po.ProjectName, P.PhaseName, sp.SubPhaseName, sp.StartDate, sp.EndDate, u.UserName, sp.GeneratedDate }).ToList();


                var k = (from j in db.C004_PROJECT
                         join v in db.vw_PhasebyLCDASAP on j.ProjectId equals v.ProjectId
                         where (v.AreaId == Area) && (v.SubAreaId == SubArea)
                         select new { j.ProjectId, j.ProjectName }).ToList();

                return Json(new { data = k, list = q });

            }
        }

        [HttpPost]
        public JsonResult GetSubPhaseByProject          (int ProjectId)         
        {
            using (ProjectEntities db = new ProjectEntities()) {
                var q = (from sp in db.vw_SubPhasebyLCDASAP
                         join P in db.C005_PHASE        on sp.PhaseId       equals P.PhaseId
                         join po in db.C004_PROJECT     on sp.ProjectId     equals po.ProjectId
                         join u in db.EndUsers          on sp.GeneratedBy   equals u.UID
                         where (sp.ProjectId == ProjectId)
                         select new { sp.SubPhaseId, po.ProjectName, P.PhaseName, sp.SubPhaseName, sp.StartDate, sp.EndDate, u.UserName, sp.GeneratedDate }).ToList();

                var pl = (from pr in db.C004_PROJECT
                          join ph in db.C005_PHASE on pr.ProjectId equals ph.ProjectId
                          where (pr.ProjectId == ProjectId)
                          select new {  ph.PhaseId, ph.PhaseName }).ToList();
                return Json(new { data = q, list = pl });
            }
        }

        [HttpPost]
        public JsonResult GetSubPhaseByPhase            (int PhaseId)           
        {
            using (ProjectEntities db = new ProjectEntities())
            {
                var q = (from sp in db.vw_SubPhasebyLCDASAP
                         join P in db.C005_PHASE on sp.PhaseId equals P.PhaseId
                         join po in db.C004_PROJECT on sp.ProjectId equals po.ProjectId
                         join u in db.EndUsers on sp.GeneratedBy equals u.UID
                         where (sp.PhaseId == PhaseId)
                         select new { sp.SubPhaseId, po.ProjectName, P.PhaseName, sp.SubPhaseName, sp.StartDate, sp.EndDate, u.UserName, sp.GeneratedDate }).ToList();

                return Json(new { data = q });
            }
        }



        // GET: SubPhase
        public ActionResult Index()
        {
            ViewBag.LocationId  = new SelectList(db.C010_LOCATION.OrderBy(l => l.LocationName), "LocationId", "LocationName");
            ViewBag.CompanyId   = new SelectList(db.C011_COMPANY.Take(0), "CompanyId", "CompanyName");

            ViewBag.DivisionId  = new SelectList(db.C001_DIVISION.Where(d => d.IsActive == true).OrderBy(d => d.DivisionName), "DivisionId", "DivisionName");
            ViewBag.AreaId      = new SelectList(db.C002_AREA.Take(0)       , "AreaId", "AreaName");
            ViewBag.SubAreaId   = new SelectList(db.C003_SUB_AREA.Take(0)   , "SubAreaId", "SubAreaName");
            ViewBag.ProjectId   = new SelectList(db.C004_PROJECT.Take(0)    , "ProjectId", "ProjectName");
            ViewBag.PhaseId     = new SelectList(db.C005_PHASE.Take(0)      , "PhaseId", "PhaseName");

            var c006_SubPhase = db.C006_SubPhase.Include(c => c.C005_PHASE).Include(c => c.EndUser).Take(0);
            return View(c006_SubPhase.ToList());
        }

        // GET: SubPhase/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C006_SubPhase c006_SubPhase = db.C006_SubPhase.Find(id);
            if (c006_SubPhase == null)
            {
                return HttpNotFound();
            }
            return View(c006_SubPhase);
        }

        // GET: SubPhase/Create
        public ActionResult Create()
        {
            #region Phase
            int pid = (string.IsNullOrEmpty(Request.QueryString["phase"]) == false) ? Convert.ToInt32(Request.QueryString["phase"].ToString()) : 0;
            if (pid > 0) {
                ViewBag.PhaseId = new SelectList(db.C005_PHASE.Where(p => p.IsActive == true), "PhaseId", "PhaseName", pid);
            }
            else {
                ViewBag.PhaseId = new SelectList(db.C005_PHASE.Where(p => p.IsActive == true), "PhaseId", "PhaseName");
            }
            #endregion

            
            return View();
        }

        // POST: SubPhase/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PhaseId,SubPhaseName,StartDate,EndDate")] C006_SubPhase c006_SubPhase)
        {
            if (ModelState.IsValid)
            {
                c006_SubPhase.GeneratedBy = UserIdentity.UserId;
                c006_SubPhase.GeneratedDate = DateTime.Now.AddHours(4);                

                db.C006_SubPhase.Add(c006_SubPhase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PhaseId = new SelectList(db.C005_PHASE.Where(p => p.IsActive == true), "PhaseId", "PhaseName", c006_SubPhase.PhaseId);
            return View(c006_SubPhase);
        }

        // GET: SubPhase/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C006_SubPhase c006_SubPhase = db.C006_SubPhase.Find(id);
            if (c006_SubPhase == null)
            {
                return HttpNotFound();
            }

            var q = (from c in db.C005_PHASE
                     where (c.IsActive.Equals(true)) && (c.PhaseId == c006_SubPhase.PhaseId)
                     select new { c.StartDate, c.EndDate }).FirstOrDefault();

            ViewBag.EndDate     = q.EndDate.ToString("dd-MMM-yyyy");
            ViewBag.StartDate   = q.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.PhaseId     = new SelectList(db.C005_PHASE.Where(p => p.IsActive == true), "PhaseId", "PhaseName", c006_SubPhase.PhaseId);
            return View(c006_SubPhase);
        }

        // POST: SubPhase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubPhaseId,PhaseId,SubPhaseName,StartDate,EndDate")] C006_SubPhase c006_SubPhase)
        {
            if (ModelState.IsValid)
            {
                c006_SubPhase.GeneratedBy   = UserIdentity.UserId;
                c006_SubPhase.GeneratedDate = DateTime.Now.AddHours(4);

                db.Entry(c006_SubPhase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PhaseId = new SelectList(db.C005_PHASE.Where(p => p.IsActive == true), "PhaseId", "PhaseName", c006_SubPhase.PhaseId);
            return View(c006_SubPhase);
        }

        // GET: SubPhase/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C006_SubPhase c006_SubPhase = db.C006_SubPhase.Find(id);
            if (c006_SubPhase == null)
            {
                return HttpNotFound();
            }

            Dictionary<int, string[]> dbucket = new Dictionary<int, string[]>();
            var bucket = (from b in db.C007_BUCKET
                          join e in db.EndUsers on b.GeneratedBy equals e.UID
                          where ((b.SubPhaseId == id) && (b.IsActive == true))
                          select new { ID = b.BucketId, IName = b.Name, Owner = e.UserName, Generated = b.GenerationDate }).ToList();
            foreach (var item in bucket)
            {
                dbucket.Add(item.ID, new string[] { item.IName, item.Owner, item.Generated.ToString("dd-MMM-yyyy") });
            }
            ViewBag.Bucket = dbucket;

            return View(c006_SubPhase);
        }

        // POST: SubPhase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C006_SubPhase c006_SubPhase = db.C006_SubPhase.Find(id);
            db.C006_SubPhase.Remove(c006_SubPhase);
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
