using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ManagementTool.Models;
using ManagementTool.Common;
using System.Collections.Generic;

namespace ManagementTool.Controllers
{
    public class TasksController : BaseController
    {
        private ProjectEntities db = new ProjectEntities();

        [HttpPost]
        public JsonResult GetCompaniesByLocation(int SelectedLocation)
        {
            using (ProjectEntities _db = new ProjectEntities()) {
                var q = (from c in _db.C011_COMPANY
                         where (c.IsActive.Equals(true)) && (c.LocationId == SelectedLocation)
                         select new { c.CompanyId, c.CompanyName }).ToList();
                return Json(new { data = q });
            }
        }

        [HttpPost]
        public JsonResult GetBucketDetails(int SelectedBucket)
        {
            using (ProjectEntities _db = new ProjectEntities())
            {
                //var q = (from b  in _db.C007_BUCKET 
                //    join p  in _db.C004_PROJECT  on b.ProjectId  equals p.ProjectId
                //    join ph in _db.C005_PHASE    on b.PhaseId    equals ph.PhaseId
                //    join sp in _db.C006_SubPhase on b.SubPhaseId equals sp.SubPhaseId
                //    into jointable  from z in jointable.DefaultIfEmpty()
                //    where (b.IsActive.Equals(true)) && (b.BucketId == SelectedBucket)
                //    select new {   ProjectName = p.ProjectName,
                //                ProjectId   = p.ProjectId,
                //                PhaseName   = ph.PhaseName,
                //                PhaseId     = ph.PhaseId,
                //                SubPhaseName = (z.SubPhaseName == null) ? "" : z.SubPhaseName,
                //                StartDate = ph.StartDate, EndDate = ph.EndDate}).FirstOrDefault();

                var q = (from b in db.C007_BUCKET
                         join p in db.C004_PROJECT on b.ProjectId equals p.ProjectId
                         join ph in db.C005_PHASE on b.PhaseId equals ph.PhaseId
                         join sp in db.C006_SubPhase on b.SubPhaseId equals sp.SubPhaseId
                         into joinx
                         from x in joinx.DefaultIfEmpty()

                         join l in db.C010_LOCATION on p.LocationId equals l.LocationId
                         join c in db.C011_COMPANY on p.CompanyId equals c.CompanyId
                         join d in db.C001_DIVISION on p.DivisionId equals d.DivisionId
                         join a in db.C002_AREA on p.AreaId equals a.AreaId
                         join sa in db.C003_SUB_AREA on p.SubAreaId equals sa.SubAreaId
                         into joiny
                         from z in joiny.DefaultIfEmpty()

                         where (b.BucketId == SelectedBucket)
                         select new {
                             ProjectName    = p.ProjectName,
                             PhaseName      = ph.PhaseName,
                             SubPhaseName   = (x.SubPhaseName == null) ? "" : x.SubPhaseName,

                             LocationName   = l.LocationName,
                             CompanyName    = c.CompanyName,
                             DivisionName   = d.DivisionName,
                             AreaName       = a.AreaName,
                             SubAreaName    = (z.SubAreaName== null) ? "No Sub Area"    : z.SubAreaName,
                             StartDate      = (x.StartDate  == null) ? ph.StartDate     : x.StartDate,
                             EndDate        = (x.EndDate    == null) ? ph.EndDate       : x.EndDate
                         }).FirstOrDefault();

                    return Json(new {   ProjectName  = q.ProjectName ,
                                        PhaseName    = q.PhaseName   ,
                                        SubPhaseName = q.SubPhaseName,
                                        LocationName = q.LocationName,
                                        CompanyName  = q.CompanyName ,
                                        DivisionName = q.DivisionName,
                                        AreaName     = q.AreaName    ,
                                        SubAreaName  = q.SubAreaName,
                                        StartDate    = q.StartDate,
                                        EndDate      = q.EndDate });
            }
        }

        [HttpPost]
        public JsonResult GetSubAreaByArea(int SelectedArea)
        {
            using (ProjectEntities _db = new ProjectEntities())
            {
                var q = (from c in _db.C003_SUB_AREA
                         where (c.AreaActive.Equals(true)) && (c.AreaId == SelectedArea)
                         select new { c.SubAreaId, c.SubAreaName }).ToList();

                var dta = (from a in _db.C002_AREA 
                         join d in _db.C001_DIVISION on a.DivisionId equals d.DivisionId
                         where (a.isActive.Equals(true)) && (a.AreaId == SelectedArea)
                         select new { d.DivisionId, d.DivisionName }).FirstOrDefault();

                return Json(new { data = q, DvDetail = dta.DivisionName + " (" + dta.DivisionId + ")" });
            }
        }



        // GET: Tasks
        public ActionResult Index(){
            int uid = UserIdentity.UserId;
            List<Service> s = Service.GetServiceList(uid,0);
            return View(s);

            //var c008_TASK_DATA = db.C008_TASK_DATA.Include(c => c.C007_BUCKET).Include(c => c.C011_COMPANY).Include(c => c.C010_LOCATION).Include(c => c.EndUser).Include(c => c.EndUser1).Include(c => c.C015_STATUS).Include(c => c.C014_TASK_TYPE);
            //return View(c008_TASK_DATA.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C008_TASK_DATA c008_TASK_DATA = db.C008_TASK_DATA.Find(id);
            if (c008_TASK_DATA == null)
            {
                return HttpNotFound();
            }
            return View(c008_TASK_DATA);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            #region A R E A
            /* var area = (from a in db.C002_AREA join
                         d in db.C001_DIVISION on a.DivisionId equals d.DivisionId
                         where ((a.isActive == true) && (d.IsActive == true))
                         select new { a, d }).OrderBy(x => x.d.DivisionName).ThenBy(x => x.a.AreaName).AsEnumerable()
                         .Select(x => new {
                             AreaId = x.a.AreaId,
                             DivId = x.a.DivisionId,
                             AreaName = x.a.AreaName,
                             DivName  = x.d.DivisionName,
                             Description = string.Format("{0} -- {1}", x.d.DivisionName, x.a.AreaName)
                         });*/
            #endregion

            ViewBag.BucketId    = new SelectList(db.C007_BUCKET,    "BucketId",     "Name");
            ViewBag.OwnerId     = new SelectList(db.EndUsers,       "UID",          "UserName");
            ViewBag.StatusId    = new SelectList(db.C015_STATUS,    "StatusId",     "TaskStatus");
            ViewBag.TaskTypeId  = new SelectList(db.C014_TASK_TYPE, "TypeId",       "TypeName");

            ViewBag.UserId      = UserIdentity.UserId;
            ViewBag.UserName    = UserIdentity.UserName;
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BucketId,SName,Description,StartDate,Deadline,ManDays,OwnerId,DocsLink,TaskTypeId,StatusId")] C008_TASK_DATA c008_TASK_DATA)
        {
            if (c008_TASK_DATA.SName != "") //(ModelState.IsValid)
            {
                c008_TASK_DATA.GeneratedBy   = UserIdentity.UserId;
                c008_TASK_DATA.GeneratedDate = DateTime.Now.AddHours(4);
                c008_TASK_DATA.IsActive      = true;

                db.C008_TASK_DATA.Add(c008_TASK_DATA);
                db.SaveChanges();
                int ServiceId = c008_TASK_DATA.TaskId;
                return RedirectToAction("Edit","Tasks", new { id = ServiceId });
            }
       

            ViewBag.BucketId = new SelectList(db.C007_BUCKET, "BucketId", "Name", c008_TASK_DATA.BucketId);            
            ViewBag.OwnerId     = new SelectList(db.EndUsers        , "UID"         , "UserName"    , c008_TASK_DATA.OwnerId);
            ViewBag.StatusId    = new SelectList(db.C015_STATUS     , "StatusId"    , "TaskStatus"  , c008_TASK_DATA.StatusId);
            ViewBag.TaskTypeId  = new SelectList(db.C014_TASK_TYPE  , "TypeId"      , "TypeName"    , c008_TASK_DATA.TaskTypeId);

            ViewBag.UserId      = UserIdentity.UserId;
            ViewBag.UserName    = UserIdentity.UserName;
            return View(c008_TASK_DATA);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            C008_TASK_DATA c008_TASK_DATA = db.C008_TASK_DATA.Find(id);
            if (c008_TASK_DATA == null) {
                return HttpNotFound();
            }
            
            task_base tb = (from t  in db.C008_TASK_DATA
                            join b  in db.C007_BUCKET   on t.BucketId   equals b.BucketId
                            join p  in db.C004_PROJECT  on b.ProjectId  equals p.ProjectId
                            join ph in db.C005_PHASE    on b.PhaseId    equals ph.PhaseId
                            join sp in db.C006_SubPhase on b.SubPhaseId equals sp.SubPhaseId
                            into joinx from x in joinx.DefaultIfEmpty()

                            join l  in db.C010_LOCATION on p.LocationId equals l.LocationId
                            join c  in db.C011_COMPANY  on p.CompanyId  equals c.CompanyId
                            join d  in db.C001_DIVISION on p.DivisionId equals d.DivisionId
                            join a  in db.C002_AREA     on p.AreaId     equals a.AreaId
                            join sa in db.C003_SUB_AREA on p.SubAreaId  equals sa.SubAreaId
                            into joiny from z in joiny.DefaultIfEmpty()
                            
                            where (t.TaskId == id)
                            select new task_base { ServiceId    = t.TaskId,
                                                    ProjectName = p.ProjectName,
                                                    PhaseName   = ph.PhaseName,
                                                    SubPhaseName= (x.SubPhaseName == null)? "": x.SubPhaseName,

                                                    LocationName = l.LocationName,
                                                    CompanyName  = c.CompanyName,
                                                    DivisionName = d.DivisionName,
                                                    AreaName     = a.AreaName,   
                                                    SubAreaName  = (z.SubAreaName == null)? "No Sub Area": z.SubAreaName
                            }).FirstOrDefault();

            ViewBag.TaskBase    = tb;
            ViewBag.BucketId    = new SelectList(db.C007_BUCKET     , "BucketId"    , "Name"        , c008_TASK_DATA.BucketId);
            ViewBag.OwnerId     = new SelectList(db.EndUsers        , "UID"         , "UserName"    , c008_TASK_DATA.OwnerId);
            ViewBag.StatusId    = new SelectList(db.C015_STATUS     , "StatusId"    , "TaskStatus"  , c008_TASK_DATA.StatusId);
            ViewBag.TaskTypeId  = new SelectList(db.C014_TASK_TYPE  , "TypeId"      , "TypeName"    , c008_TASK_DATA.TaskTypeId);

            ViewBag.BucketDetial = Bhai.GetBucketDetail(c008_TASK_DATA.BucketId);
            ViewBag.UserName    = UserIdentity.UserName;
            return View(c008_TASK_DATA);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskId,BucketId,SName,Description,OrderId,ManDays,OwnerId,DocsLink,TaskTypeId,StatusId")] C008_TASK_DATA c008_TASK_DATA)
        {
            if (ModelState.IsValid)
            {
                c008_TASK_DATA.GeneratedBy      = UserIdentity.UserId;
                c008_TASK_DATA.GeneratedDate    = DateTime.Now.AddHours(4);
                c008_TASK_DATA.IsActive         = true;

                db.Entry(c008_TASK_DATA).State = EntityState.Modified;
                db.Entry(c008_TASK_DATA).Property(x => x.StartDate) .IsModified = false;
                db.Entry(c008_TASK_DATA).Property(x => x.Deadline)  .IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Edit", "Tasks", new { id = c008_TASK_DATA.TaskId });
                //return RedirectToAction("Index");
            }

            ViewBag.BucketId    = new SelectList(db.C007_BUCKET     , "BucketId"    , "Name"        , c008_TASK_DATA.BucketId);
            ViewBag.OwnerId     = new SelectList(db.EndUsers        , "UID"         , "UserName"    , c008_TASK_DATA.OwnerId);
            ViewBag.StatusId    = new SelectList(db.C015_STATUS     , "StatusId"    , "TaskStatus"  , c008_TASK_DATA.StatusId);
            ViewBag.TaskTypeId  = new SelectList(db.C014_TASK_TYPE  , "TypeId"      , "TypeName"    , c008_TASK_DATA.TaskTypeId);

            ViewBag.UserName = UserIdentity.UserName;
            return View(c008_TASK_DATA);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C008_TASK_DATA c008_TASK_DATA = db.C008_TASK_DATA.Find(id);
            if (c008_TASK_DATA == null)
            {
                return HttpNotFound();
            }
            return View(c008_TASK_DATA);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C008_TASK_DATA c008_TASK_DATA = db.C008_TASK_DATA.Find(id);
            db.C008_TASK_DATA.Remove(c008_TASK_DATA);
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
