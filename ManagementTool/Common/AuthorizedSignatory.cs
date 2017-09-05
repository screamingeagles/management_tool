using System;
using System.Linq;
using System.Web;
using ManagementTool.Models;
using System.Collections.Generic;


namespace ManagementTool.Common
{
    public class AuthorizedSignatory {
        public static bool GetUserAuthorization(string controller, string action) {

            int RoleId = 0;
            bool ret = false;
            RoleId = UserIdentity.RoleId;
            
            using (ProjectEntities pe = new ProjectEntities()) {
                ret = (from r in pe.ROLE_DETAIL
                          where (r.RoleId == RoleId) && (r.Controller == controller) 
                             && (r.Action == action) && (r.IsActive == true)
                          select r.Allowed).FirstOrDefault();
            }
            return ret;
        }
        
    }
}