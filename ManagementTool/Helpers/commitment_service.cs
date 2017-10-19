using System;
using System.Web;
using System.Linq;
using ManagementTool.Models;
using System.Collections.Generic;


namespace ManagementTool.Helpers
{
    public class commitment_service
    {
        public int CommitmentId { get; set; }
        public string CommitmentHeading { get; set; }
        public List<commitment_detail> lst { get; set; }


        public static List<commitment_service> GetUserCommitment(int UserId){
            commitment_detail cd;
            commitment_service obj;
            List<commitment_detail> dtl;            
            List <commitment_service> cs = new List<commitment_service>();
            try {                

                using (ProjectEntities db = new ProjectEntities()) {

                 

                    var mo = (from m in db.C020_CommitmentMaster
                             where (m.UserId == UserId) && (m.IsActive == true)
                             select new { m.CommitmentId, m.CommitmentHeader}).ToList();

                    foreach (var item in mo) {
                        obj                     = new commitment_service();
                        obj.CommitmentId        = item.CommitmentId;
                        obj.CommitmentHeading   = item.CommitmentHeader;

                        #region Adding D e t a i l;
                        dtl = new List<commitment_detail>();
                        var dt = (from d in db.C021_CommimentDetails
                                  join u in db.EndUsers on d.GeneratedBy equals u.UID
                                  where (d.CommitmentId == item.CommitmentId) && (d.IsActive == true)
                                  select new { d.DetailId, d.CommimentName, d.CDescription, d.CRemarks, d.GeneratedDate, u.UserName }).ToList();

                        foreach (var detail in dt) {                           
                            cd = new commitment_detail();
                            cd.Detaild          = detail.DetailId;
                            cd.Commitment       = detail.CommimentName;
                            cd.Description      = detail.CDescription;
                            cd.Remarks          = detail.CRemarks;
                            cd.GeneratedBy      = detail.UserName;
                            cd.Generateddate    = detail.GeneratedDate;
                            dtl.Add(cd);
                        }
                        obj.lst = dtl;
                        #endregion

                        cs.Add(obj);

                        dtl = null;
                        obj = null;
                    }                   
                }
                return cs;
            }
            catch(Exception ex) {
                string s = ex.Message;
            }
            return null;
        }
    }

    public class commitment_detail {
        public int MateridId { get; set; }
        public int Detaild { get; set; }

        public int ServiceId { get; set; }
        public string ServiceName { get; set; }

        public string Commitment { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string GeneratedBy { get; set; }
        public DateTime Generateddate { get; set; }
    }
}