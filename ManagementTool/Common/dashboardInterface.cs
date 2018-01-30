using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using ManagementTool.Models;
using System.Collections.Generic;

namespace ManagementTool.Common
{
    public class dashboardInterface
    {
        public List<SelectListItem> LoginCompanySelector{ get; set; }
        public List<SelectListItem> ActivityCompanySelector { get; set; }

        public List<LoginHistory_Interface> login_history { get; set; }

        public List<SAPUserActivity_interface> SAPUserActivity { get; set; }    
    }


    public class LoginHistory_Interface
    {
        public int TID { get; set; }
        public string UserName { get; set; }        
        public string Department { get; set; }
        public string Company { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int LoginSince { get; set; }
    }

    public partial class SAPUserActivity_interface
    {
        public int SID { get; set; }
        public string Server { get; set; }
        public string TCode { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}