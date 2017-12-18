using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using ManagementTool.Common;
using ManagementTool.Models;
using System.Collections.Generic;


namespace ManagementTool.Controllers
{
    public class SearchController : Controller
    {
        public JsonResult GetSearchOptions(string query) {

            string[] names = null;
            using (ProjectEntities db = new ProjectEntities()) {
                
                if (String.IsNullOrEmpty(query) == false) {
                    names = (from i in db.vw_SearchSource
                             where i.Names.Contains(query)
                             select i.Names).ToArray();
                }             
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }


        // GET: Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(){
            List<SearchResultDTO> qry = new List<SearchResultDTO>();

            string param = (Request.Form["top-search"] == null) ? "" : Request.Form["top-search"].ToString();

            if (string.IsNullOrEmpty(param) == false) {

                #region Filtering Input
                if (param.IndexOf('>') > 0) {
                    // sending selected text
                    int index = param.LastIndexOf('>');
                    param = (index > 0) ? param.Substring(index + 1).Trim() : param.Trim();
                }
                #endregion

                using (ProjectEntities db = new ProjectEntities()) {
                    qry = (from i in db.vw_SearchSource
                                where i.Names.Contains(param)
                                    select new SearchResultDTO { id = i.ID, Name = i.Names, Table = i.TableCol }).ToList();                    
                }

            }
            return View(qry);
        }
    }
}