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
        public JsonResult GetBucketListbySubPhase   (int SubPhaseId)        
        {
            using (ProjectEntities _db = new ProjectEntities())
            {
                var q = (from b in _db.C007_BUCKET
                         where (b.IsActive.Equals(true)) && (b.SubPhaseId == SubPhaseId)
                         select new { b.BucketId, b.Name }).ToList();
                return Json(new { data = q });
            }
        }

        [HttpPost]
        public JsonResult GetCompaniesByLocation    (int SelectedLocation)  
        {
            using (ProjectEntities _db = new ProjectEntities()) {
                var q = (from c in _db.C011_COMPANY
                         where (c.IsActive.Equals(true)) && (c.LocationId == SelectedLocation)
                         select new { c.CompanyId, c.CompanyName }).ToList();
                return Json(new { data = q });
            }
        }

        [HttpPost]
        public JsonResult GetBucketDetails          (int SelectedBucket)    
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

                #region Commented bucket    
                /*
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
                    */
                #endregion

                var q = (from b in _db.C007_BUCKET
                         join sp in _db.C006_SubPhase on b.SubPhaseId equals sp.SubPhaseId
                         where (b.IsActive.Equals(true)) && (b.BucketId == SelectedBucket)
                         select new { sp.StartDate, sp.EndDate }).FirstOrDefault();

                return Json(new { StartDate = q.StartDate, EndDate = q.EndDate });
            }
        }


        [HttpPost]
        public JsonResult GetSubAreaByArea          (int SelectedArea)      
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



        [HttpPost]
        public JsonResult GetProjectsByAreaSubArea(int Area, int SubArea)
        {
            using (ProjectEntities _db = new ProjectEntities()) {
                if (SubArea == 0) {
                    var q = (from p in _db.C004_PROJECT
                             where (p.IsActive.Equals(true)) && (p.AreaId == Area)
                             select new { p.ProjectId, p.ProjectName }).ToList();
                    return Json(new { data = q });
                }
                else {
                    var q = (from s in _db.C004_PROJECT
                             where (s.IsActive.Equals(true)) && (s.AreaId == Area) && (s.SubAreaId == SubArea)
                             select new { s.ProjectId, s.ProjectName }).ToList();
                    return Json(new { data = q });
                }
            }
        }



        // GET: Tasks
        public ActionResult         Index       ()              {
            //UserIdentity.UserId = 1008;

            ViewBag.isAddAllowed    = false;
            ViewBag.isDeleteAllowed = false;

            int uid = UserIdentity.UserId;
            List<Service> s = Service.GetServiceList(uid,0);

            #region authorizatoin check
            string[] inparam = { "Create", "Delete" };
            var qry = (from r in db.ROLE_DETAIL
                       join e in db.EndUsers on r.RoleId equals e.UserType
                       where (e.UID == UserIdentity.UserId) && (r.Controller == "Tasks") && (inparam.Contains(r.Action))
                       select new { r.Action, r.Allowed }).ToList();
            foreach (var item in qry) {
                     if (item.Action == "Create") { ViewBag.isAddAllowed    = (item.Allowed) ? true : false; }
                else if (item.Action == "Delete") { ViewBag.isDeleteAllowed = (item.Allowed) ? true : false; }
            }
            #endregion

            
            return View(s);

            //var c008_TASK_DATA = db.C008_TASK_DATA.Include(c => c.C007_BUCKET).Include(c => c.C011_COMPANY).Include(c => c.C010_LOCATION).Include(c => c.EndUser).Include(c => c.EndUser1).Include(c => c.C015_STATUS).Include(c => c.C014_TASK_TYPE);
            //return View(c008_TASK_DATA.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult         Details     (int? id)       
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
            //c008_TASK_DATA.isAddAllowed = true;
            return View(c008_TASK_DATA);
        }

        // GET: Tasks/Create
        public ActionResult         Create      (int? PhaseId)  
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

            ViewBag.LocationId  = new SelectList(db.C010_LOCATION.Where(l => l.IsActive == true).OrderBy(l => l.LocationName), "LocationId", "LocationName");
            ViewBag.CompanyId   = new SelectList(db.C011_COMPANY    .Take(0), "CompanyId", "CompanyName");

            ViewBag.DivisionId  = new SelectList(db.C001_DIVISION.Where(d => d.IsActive == true).OrderBy(d => d.DivisionName), "DivisionId", "DivisionName");
            ViewBag.AreaId      = new SelectList(db.C002_AREA.      Take(0), "AreaId"    , "AreaName"    );
            ViewBag.SubAreaId   = new SelectList(db.C003_SUB_AREA.  Take(0), "SubAreaId" , "SubAreaName" );
            ViewBag.ProjectId   = new SelectList(db.C004_PROJECT.   Take(0), "ProjectId" , "ProjectName" );
            ViewBag.PhaseId     = new SelectList(db.C005_PHASE.     Take(0), "PhaseId"   , "PhaseName"   );
            ViewBag.SubPhaseId  = new SelectList(db.C006_SubPhase.  Take(0), "SubPhaseId", "SubPhaseName");



            ViewBag.BucketId    = new SelectList(db.C007_BUCKET.Where(x => x.PhaseId == ((PhaseId.HasValue)?PhaseId.Value:0)).OrderBy(x => x.Name),    "BucketId",     "Name");
            ViewBag.OwnerId     = new SelectList(db.EndUsers,       "UID",          "UserName");
            ViewBag.StatusId    = new SelectList(db.C015_STATUS,    "StatusId",     "TaskStatus");
            ViewBag.TaskTypeId  = new SelectList(db.C014_TASK_TYPE.Where(t => t.TypeName != "Meeting"), "TypeId",       "TypeName");

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
            ViewBag.TaskTypeId  = new SelectList(db.C014_TASK_TYPE.Where(t => t.TypeName != "Meeting"), "TypeId"      , "TypeName"    , c008_TASK_DATA.TaskTypeId);

            ViewBag.UserId      = UserIdentity.UserId;
            ViewBag.UserName    = UserIdentity.UserName;
            return View(c008_TASK_DATA);
        }


        public ActionResult Meeting(int? PhaseId)
        {           

            ViewBag.ProjectId = new SelectList(db.C004_PROJECT.Where(p => p.IsActive == true).OrderBy(p => p.ProjectName), "ProjectId", "ProjectName");
            if (PhaseId.HasValue) {
                ViewBag.PhaseId = new SelectList(db.C005_PHASE.Where(p => p.IsActive == true).Take(5), "PhaseId", "PhaseName", PhaseId.Value);
            } else {
                ViewBag.PhaseId = new SelectList(db.C005_PHASE.Where(p => p.IsActive == true).Take(5), "PhaseId", "PhaseName");
            }

            ViewBag.BucketId = new SelectList(db.C007_BUCKET.Where(x => x.PhaseId == ((PhaseId.HasValue) ? PhaseId.Value : 0)).OrderBy(x => x.Name), "BucketId", "Name");
            ViewBag.OwnerId = new SelectList(db.EndUsers, "UID", "UserName");
            ViewBag.StatusId = new SelectList(db.C015_STATUS, "StatusId", "TaskStatus");

            ViewBag.UserId = UserIdentity.UserId;
            ViewBag.UserName = UserIdentity.UserName;
            return View();
        }

        // POST: Tasks/Meeting                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Meeting([Bind(Include = "BucketId,SName,Description,StartDate,Deadline,ManDays,OwnerId,DocsLink,TaskTypeId,StatusId")] C008_TASK_DATA c008_TASK_DATA)
        {
            if (c008_TASK_DATA.SName != "") //(ModelState.IsValid)
            {
                c008_TASK_DATA.GeneratedBy = UserIdentity.UserId;
                c008_TASK_DATA.GeneratedDate = DateTime.Now.AddHours(4);
                c008_TASK_DATA.IsActive = true;

                db.C008_TASK_DATA.Add(c008_TASK_DATA);
                db.SaveChanges();
                int ServiceId = c008_TASK_DATA.TaskId;
                return RedirectToAction("Meetings", "Tasks", new { id = ServiceId });
            }

            ViewBag.ProjectId = new SelectList(db.C004_PROJECT.Where(p => p.IsActive == true).OrderBy(p => p.ProjectName), "ProjectId", "ProjectName");
            ViewBag.PhaseId = new SelectList(db.C005_PHASE.Where(p => p.IsActive == true).Take(5), "PhaseId", "PhaseName");

            ViewBag.BucketId = new SelectList(db.C007_BUCKET, "BucketId", "Name", c008_TASK_DATA.BucketId);
            ViewBag.OwnerId = new SelectList(db.EndUsers, "UID", "UserName", c008_TASK_DATA.OwnerId);
            ViewBag.StatusId = new SelectList(db.C015_STATUS, "StatusId", "TaskStatus", c008_TASK_DATA.StatusId);

            ViewBag.UserId = UserIdentity.UserId;
            ViewBag.UserName = UserIdentity.UserName;
            return View(c008_TASK_DATA);
        }


        // GET: Tasks/Edit/5
        public ActionResult Meetings(int? id)
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

            task_base tb = (from t in db.C008_TASK_DATA
                            join b in db.C007_BUCKET on t.BucketId equals b.BucketId
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

                            where (t.TaskId == id)
                            select new task_base
                            {
                                ServiceId = t.TaskId,
                                ProjectName = p.ProjectName,
                                PhaseName = ph.PhaseName,
                                SubPhaseName = (x.SubPhaseName == null) ? "" : x.SubPhaseName,

                                LocationName = l.LocationName,
                                CompanyName = c.CompanyName,
                                DivisionName = d.DivisionName,
                                AreaName = a.AreaName,
                                SubAreaName = (z.SubAreaName == null) ? "No Sub Area" : z.SubAreaName
                            }).FirstOrDefault();

            ViewBag.TaskBase = tb;
            ViewBag.CoOwners = Bhai.GetParticipantsList(id.Value);
            ViewBag.BucketId = new SelectList(db.C007_BUCKET, "BucketId", "Name", c008_TASK_DATA.BucketId);
            ViewBag.OwnerId = new SelectList(db.EndUsers, "UID", "UserName", c008_TASK_DATA.OwnerId);
            ViewBag.StatusId = new SelectList(db.C015_STATUS, "StatusId", "TaskStatus", c008_TASK_DATA.StatusId);
            ViewBag.TaskTypeId = new SelectList(db.C014_TASK_TYPE, "TypeId", "TypeName", c008_TASK_DATA.TaskTypeId);

            ViewBag.BucketDetial = Bhai.GetBucketDetail(c008_TASK_DATA.BucketId);
            ViewBag.UserName = UserIdentity.UserName;
            return View(c008_TASK_DATA);
        }


        // POST: Tasks/Meetings/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Meetings([Bind(Include = "TaskId,BucketId,SName,Description,OrderId,ManDays,OwnerId,DocsLink,TaskTypeId,StatusId")] C008_TASK_DATA c008_TASK_DATA)
        {
            if (ModelState.IsValid)
            {
                c008_TASK_DATA.GeneratedBy = UserIdentity.UserId;
                c008_TASK_DATA.GeneratedDate = DateTime.Now.AddHours(4);
                c008_TASK_DATA.IsActive = true;

                db.Entry(c008_TASK_DATA).State = EntityState.Modified;
                db.Entry(c008_TASK_DATA).Property(x => x.StartDate).IsModified = false;
                db.Entry(c008_TASK_DATA).Property(x => x.Deadline).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Meetings", "Tasks", new { id = c008_TASK_DATA.TaskId });
                //return RedirectToAction("Index");
            }

            task_base tb = (from t in db.C008_TASK_DATA
                            join b in db.C007_BUCKET on t.BucketId equals b.BucketId
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

                            where (t.TaskId == c008_TASK_DATA.TaskId)
                            select new task_base
                            {
                                ServiceId = t.TaskId,
                                ProjectName = p.ProjectName,
                                PhaseName = ph.PhaseName,
                                SubPhaseName = (x.SubPhaseName == null) ? "" : x.SubPhaseName,

                                LocationName = l.LocationName,
                                CompanyName = c.CompanyName,
                                DivisionName = d.DivisionName,
                                AreaName = a.AreaName,
                                SubAreaName = (z.SubAreaName == null) ? "No Sub Area" : z.SubAreaName
                            }).FirstOrDefault();

            ViewBag.TaskBase = tb;
            ViewBag.BucketId = new SelectList(db.C007_BUCKET, "BucketId", "Name", c008_TASK_DATA.BucketId);
            ViewBag.OwnerId = new SelectList(db.EndUsers, "UID", "UserName", c008_TASK_DATA.OwnerId);
            ViewBag.StatusId = new SelectList(db.C015_STATUS, "StatusId", "TaskStatus", c008_TASK_DATA.StatusId);
            ViewBag.TaskTypeId = new SelectList(db.C014_TASK_TYPE, "TypeId", "TypeName", c008_TASK_DATA.TaskTypeId);

            ViewBag.BucketDetial = Bhai.GetBucketDetail(c008_TASK_DATA.BucketId);
            ViewBag.UserName = UserIdentity.UserName;
            return View(c008_TASK_DATA);
        }


        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            C008_TASK_DATA c008_TASK_DATA = db.C008_TASK_DATA.Find(id);
            if (c008_TASK_DATA == null) { return HttpNotFound(); }
            if (c008_TASK_DATA.TaskTypeId == 5) { return RedirectToAction("Meetings", "Tasks", new { id = id}); }

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

                                                    ProjectId    = p.ProjectId,    
                                                    ProjectName = p.ProjectName,
                                                    
                                                    PhaseId     = ph.PhaseId,        
                                                    PhaseName   = ph.PhaseName,

                                                    SubPhaseId  =  x.SubPhaseId,
                                                    SubPhaseName= (x.SubPhaseName == null)? "": x.SubPhaseName,

                                                    LocationId   = l.LocationId,
                                                    LocationName = l.LocationName,

                                                    CompanyId    = c.CompanyId,
                                                    CompanyName  = c.CompanyName,

                                                    DivisionId   = d.DivisionId,
                                                    DivisionName = d.DivisionName,

                                                    AreaId       =  a.AreaId,
                                                    AreaName     = a.AreaName,   

                                                    SubAreaId    = p.SubAreaId,
                                                    SubAreaName  = (z.SubAreaName == null)? "No Sub Area": z.SubAreaName
                            }).FirstOrDefault();


            #region authorizatoin check
            string[] inparam = { "Create", "Delete" }; 
            var qry = (from r in db.ROLE_DETAIL
                       join e in db.EndUsers on r.RoleId equals e.UserType
                       where (e.UID == UserIdentity.UserId) && (r.Controller == "Tasks") && (inparam.Contains(r.Action))
                       select new { r.Action, r.Allowed }).ToList();
            foreach (var item in qry) {
                     if (item.Action == "Create") { tb.isAddAllowed     = (item.Allowed) ? true : false; }
                else if(item.Action  == "Delete") { tb.isDeleteAllowed  = (item.Allowed) ? true : false; }                
            }
            #endregion

            ViewBag.TaskBase    = tb;
            ViewBag.LocationId  = new SelectList(db.C010_LOCATION   .Where(l => l.IsActive == true).                                        OrderBy(l => l.LocationName),   "LocationId", "LocationName", tb.LocationId);
            ViewBag.CompanyId   = new SelectList(db.C011_COMPANY    .Where(c => c.LocationId == tb.LocationId && c.IsActive == true).       OrderBy(c => c.CompanyName),    "CompanyId", "CompanyName"  , tb.CompanyId);

            ViewBag.DivisionId  = new SelectList(db.C001_DIVISION   .Where(d => d.IsActive == true).                                        OrderBy(d => d.DivisionName),   "DivisionId", "DivisionName", tb.DivisionId);
            ViewBag.AreaId      = new SelectList(db.C002_AREA       .Where(a => a.DivisionId == tb.DivisionId   && a.isActive == true).     OrderBy(a => a.AreaName),       "AreaId"    , "AreaName"    , tb.AreaId);
            ViewBag.SubAreaId   = new SelectList(db.C003_SUB_AREA   .Where(sa => sa.AreaId == tb.AreaId         && sa.AreaActive == true).  OrderBy(sa => sa.SubAreaName),  "SubAreaId" , "SubAreaName" , tb.SubAreaId);

            ViewBag.ProjectId   = new SelectList(db.C004_PROJECT    .Where(p => p.IsActive      == true).                                   OrderBy(p => p.ProjectName),    "ProjectId", "ProjectName",   tb.ProjectId);
            ViewBag.PhaseId     = new SelectList(db.C005_PHASE      .Where(ph => ph.ProjectId   == tb.ProjectId && ph.IsActive == true).    OrderBy(ph => ph.PhaseName),    "PhaseId",   "PhaseName",     tb.PhaseId);
            ViewBag.SubPhaseId  = new SelectList(db.C006_SubPhase   .Where(sp => sp.PhaseId     == tb.PhaseId).                             OrderBy(sp => sp.SubPhaseName), "SubPhaseId", "SubPhaseName", tb.SubPhaseId);


            ViewBag.BucketId    = new SelectList(db.C007_BUCKET.Where(b => b.ProjectId == tb.ProjectId && b.PhaseId == tb.PhaseId && b.IsActive ==true).OrderBy(b => b.Name)     , "BucketId", "Name", c008_TASK_DATA.BucketId);
            ViewBag.OwnerId     = new SelectList(db.EndUsers,       "UID",      "UserName",     c008_TASK_DATA.OwnerId);
            ViewBag.StatusId    = new SelectList(db.C015_STATUS,    "StatusId", "TaskStatus",   c008_TASK_DATA.StatusId);
            ViewBag.TaskTypeId  = new SelectList(db.C014_TASK_TYPE, "TypeId",   "TypeName",     c008_TASK_DATA.TaskTypeId);
            ViewBag.UserId = UserIdentity.UserId;
            ViewBag.UserName = UserIdentity.UserName;
            
            
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
            if (c008_TASK_DATA == null) {
                return HttpNotFound();
            }


            if (c008_TASK_DATA.TaskTypeId == 5)
            {
                return RedirectToAction("Meetings", "Tasks", new { id = id });
            }

            task_base tb = (from t in db.C008_TASK_DATA
                            join b in db.C007_BUCKET on t.BucketId equals b.BucketId
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

                            where (t.TaskId == id)
                            select new task_base
                            {
                                ServiceId = t.TaskId,
                                ProjectName = p.ProjectName,
                                PhaseName = ph.PhaseName,
                                SubPhaseName = (x.SubPhaseName == null) ? "" : x.SubPhaseName,

                                LocationName = l.LocationName,
                                CompanyName = c.CompanyName,
                                DivisionName = d.DivisionName,
                                AreaName = a.AreaName,
                                SubAreaName = (z.SubAreaName == null) ? "No Sub Area" : z.SubAreaName
                            }).FirstOrDefault();

            ViewBag.TaskBase    = tb;
            ViewBag.BucketId    = new SelectList(db.C007_BUCKET, "BucketId", "Name", c008_TASK_DATA.BucketId);
            ViewBag.OwnerId     = new SelectList(db.EndUsers, "UID", "UserName", c008_TASK_DATA.OwnerId);
            ViewBag.StatusId    = new SelectList(db.C015_STATUS, "StatusId", "TaskStatus", c008_TASK_DATA.StatusId);
            ViewBag.TaskTypeId  = new SelectList(db.C014_TASK_TYPE, "TypeId", "TypeName", c008_TASK_DATA.TaskTypeId);

            ViewBag.BucketDetial= Bhai.GetBucketDetail(c008_TASK_DATA.BucketId);
            ViewBag.UserName    = UserIdentity.UserName;
            return View(c008_TASK_DATA);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C008_TASK_DATA c008_TASK_DATA = db.C008_TASK_DATA.Find(id);

            c008_TASK_DATA.GeneratedBy  = UserIdentity.UserId;
            c008_TASK_DATA.GeneratedDate= DateTime.Now.AddHours(4);
            c008_TASK_DATA.IsActive     = false;

            db.Entry(c008_TASK_DATA).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Tasks");

            //db.C008_TASK_DATA.Remove(c008_TASK_DATA);
            //db.SaveChanges();
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


        /* <!--  Add new Participant --> */
        [HttpPost]
        public JsonResult AddNewParticipant(int TaskId, string PName, string PDesc)
        {
            if (TaskId > 0)
            {
                var qry = (from e in db.EndUsers
                           where (e.UserName == PName)
                           select new { e.UID, e.UserName }).FirstOrDefault();

                if (qry != null)
                {
                    C024_participants co = new C024_participants();
                    co.TaskId = TaskId;
                    co.UserId = qry.UID;
                    co.PDesc  = PDesc;
                    co.CreatedBy = UserIdentity.UserId;
                    co.CreatedDate = DateTime.Now.AddHours(4);
                    db.C024_participants.Add(co);
                    db.SaveChanges();
                }
            }

            var q = (from o in db.C024_participants
                     join u in db.EndUsers on o.UserId equals u.UID
                     where (o.TaskId == TaskId)
                     select new { o.PId, u.UserName, o.PDesc }).ToList();
            return Json(new { data = q });

        }

        /* <!--  Remove User --> */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ParticipantRemove(int id)
        {
            int ProjectId = 0;
            C024_participants co = db.C024_participants.Find(id);
            ProjectId = co.TaskId;
            db.C024_participants.Remove(co);
            db.SaveChanges();
            return RedirectToAction("Meetings", "Tasks", new { id = ProjectId });
        }

    }
}
