using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using ManagementTool.Common;
using System.Collections.Generic;

namespace ManagementTool.Controllers
{
    public class ImportController : BaseController
    {
        // GET: Import
        public ActionResult Index(int? id)
        {
            if (id.HasValue) {
                ViewBag.Message = "Your Files Successfully Saved";
            }else {
                ViewBag.Message = "";
            }

            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(){


            int      _uId   =  0;
            int     Index   =  1;
            string  hold    = "";
            STATS_TABLE_LOCAL t   = null;
            List<STATS_TABLE_LOCAL> dl = new List<STATS_TABLE_LOCAL>();

            _uId = UserIdentity.UserId;

            if (Request != null) {
                HttpPostedFileBase file = Request.Files["attachment"];
                if ((file != null) && (file.ContentLength != 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            hold = "";
                            t = new STATS_TABLE_LOCAL();

                            if (workSheet.Cells[rowIterator, 1].Value != null) { hold = workSheet.Cells[rowIterator, 1].Value.ToString(); }
                            t.Server = hold;

                            hold = "";
                            if (workSheet.Cells[rowIterator, 2].Value != null) { hold = workSheet.Cells[rowIterator, 2].Value.ToString(); }
                            t.TCode = hold;

                            hold = "";
                            if (workSheet.Cells[rowIterator, 3].Value != null) { hold = workSheet.Cells[rowIterator, 3].Value.ToString(); }
                            t.StartDate = hold;

                            hold = "";
                            if (workSheet.Cells[rowIterator, 4].Value != null) { hold = workSheet.Cells[rowIterator, 4].Value.ToString(); }
                            t.StartTime = hold;

                            hold = "";
                            if (workSheet.Cells[rowIterator, 5].Value != null) { hold = workSheet.Cells[rowIterator, 5].Value.ToString(); }
                            t.UserName = hold;

                            hold = "";
                            if (workSheet.Cells[rowIterator, 6].Value != null) { hold = workSheet.Cells[rowIterator, 6].Value.ToString(); }
                            t.FirstName = hold;

                            hold = "";
                            if (workSheet.Cells[rowIterator, 7].Value != null) { hold = workSheet.Cells[rowIterator, 7].Value.ToString(); }
                            t.LastName = hold;

                            hold = "";
                            if (workSheet.Cells[rowIterator, 8].Value != null) { hold = workSheet.Cells[rowIterator, 8].Value.ToString(); }
                            t.Department = hold;

                            hold = "";
                            if (workSheet.Cells[rowIterator, 9].Value != null) { hold = workSheet.Cells[rowIterator, 9].Value.ToString(); }
                            t.CompanyCode = hold;

                            hold = "";
                            if (workSheet.Cells[rowIterator, 10].Value != null) { hold = workSheet.Cells[rowIterator, 10].Value.ToString(); }
                            t.CompanyName = hold;

                            t.CreatedBy = _uId;
                            t.CreatedDate = DateTime.Now.AddHours(4);
                            dl.Add(t);

                            Index = Index + 1;
                            //for (int colIterator = 2; colIterator <= noOfCol; colIterator++){
                            //    hold = hold + "," + (workSheet.Cells[rowIterator, colIterator].Value == null ? "" : workSheet.Cells[rowIterator, colIterator].Value.ToString());
                            //}
                            //str = str + hold + "<br />";
                        }
                        int i = Bhai.SaveStatsImport(dl);
                        hold = "successdfully saved";
                    } // end if using
                } // end of file if
            } // end of request

            ViewBag.message = hold;


            return RedirectToAction("Index", new { id = 1});
        }
    }
}