using System;
using System.Linq;
using System.Web;
using ManagementTool.Models;
using System.Collections.Generic;


namespace ManagementTool.Common
{
    public class AuthorizedSignatory
    {
        public static bool GetUserAuthorization(string controller, string action, int UserId) {

            int RoleId = 0;
            bool ret = false;            

            UserId = Bhai.GetUserIdFromSession(UserId);
            RoleId = AuthorizedSignatory.GetUserRole(UserId);
            using (ProjectEntities pe = new ProjectEntities()) {
                ret = (from r in pe.ROLE_DETAIL
                          where (r.RoleId == RoleId) && (r.Controller == controller) 
                             && (r.Action == action) && (r.IsActive == true)
                          select r.Allowed).FirstOrDefault();
            }
            return ret;
        }

        private static int GetUserRole(int userid) {
            int role = 103;
            try {
                using (ProjectEntities pe = new ProjectEntities()) {
                    int? q = (from r in pe.EndUsers where (r.UID == userid)
                            select  r.UserType).FirstOrDefault();

                    if (q.HasValue) {
                        role = q.Value;
                    }else{
                        role = 103;
                    }
                }
                return role;
            }
            catch {
                return role;
            }
        }
    }
}