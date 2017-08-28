using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagementTool.Models;
using System.Collections.Generic;

namespace ManagementTool.Common
{
    public class projectmodel
    {
        public int ProjetId { get; set; }

        public string ProjetName { get; set; }

        public List<SelectListItem> Location { get; set; }
        public List<SelectListItem> Company { get; set; }
        public List<SelectListItem> Division { get; set; }
        public List<SelectListItem> ProjectType { get; set; }


        public int CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedById { get; set; }
        public DateTime UpdatedDate { get; set; }

        public projectmodel() {
        }
        public projectmodel(int projectId)
        {
            List<SelectListItem> lst;
            SelectListItem _SelectListItem;

            this.ProjetName = "";
            if (projectId > 0) { // return project
            }
            else { // add new 
                #region Add new

                using (ProjectEntities _db = new ProjectEntities())
                {
                    lst = new List<SelectListItem>();
                    #region  // :- L o c a t i o n  :. 
                    var query = (from l in _db.C004_LOCATION
                                 orderby l.LocationName
                                 where (l.IsActive == true)
                                 select new { l.LocationId, l.LocationName }).ToList();
                    foreach (var litem in query)
                    {
                        _SelectListItem = new SelectListItem();
                        _SelectListItem.Value = litem.LocationId.ToString();
                        _SelectListItem.Text = litem.LocationName;
                        lst.Add(_SelectListItem);
                    }
                    Location = lst;
                    #endregion


                    lst = null;
                    lst = new List<SelectListItem>();
                    #region  // :- c o m p a n y  :.
                    var queryc = (from c in _db.C005_COMPANY
                                  orderby c.CompanyName
                                  where (c.IsActive == true)
                                  select new { c.CompanyId, c.CompanyName }).ToList();
                    foreach (var litem in queryc)
                    {
                        _SelectListItem = new SelectListItem();
                        _SelectListItem.Value = litem.CompanyId.ToString();
                        _SelectListItem.Text = litem.CompanyName;
                        lst.Add(_SelectListItem);
                    }
                    Company = lst;
                    #endregion


                    lst = null;
                    lst = new List<SelectListItem>();
                    #region  // :- D i v i s i o n :.
                    var queryd = (from c in _db.C006_DIVISION
                                  orderby c.DivisionName
                                  where (c.IsActive == true)
                                  select new { c.DivisionId, c.DivisionName }).ToList();
                    foreach (var litem in queryd)
                    {
                        _SelectListItem = new SelectListItem();
                        _SelectListItem.Value = litem.DivisionId.ToString();
                        _SelectListItem.Text = litem.DivisionName;
                        lst.Add(_SelectListItem);
                    }
                    Division = lst;
                    #endregion


                    lst = null;
                    lst = new List<SelectListItem>();
                    #region  // :- T y p e  :.
                    var queryt = (from ty in _db.C009_TYPE
                                  where (ty.IsActive == true)
                                  select new { ty.TypeId, ty.TypeName }).ToList();
                    foreach (var litem in queryt)
                    {
                        _SelectListItem = new SelectListItem();
                        _SelectListItem.Value = litem.TypeId.ToString();
                        _SelectListItem.Text = litem.TypeName;
                        lst.Add(_SelectListItem);
                    }
                    ProjectType = lst;
                    #endregion

                }

                #endregion
            }
        }

    }
}