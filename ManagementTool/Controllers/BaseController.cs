using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ManagementTool.Common;
using System.Collections.Generic;


namespace ManagementTool.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int     _sid        = 0;
            bool    _isValid    = true;
            string  _action     = "";
            string  _controller = "";

            _sid                = (HttpContext.Session["SessionId"] == null) ? 0 : Convert.ToInt32(HttpContext.Session["SessionId"].ToString());
            _action             = filterContext.RouteData.Values["action"].ToString();
            _controller         = filterContext.RouteData.Values["controller"].ToString();

            if (_sid== 0) {
                // no session go to login
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Index" } });
                return;
            }
            else {
                // user logged in get his authentication
                _isValid = AuthorizedSignatory.GetUserAuthorization(_controller, _action, _sid);
            }
            

            if (_isValid == false) {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Welcome" } });
                return;
            }
            base.OnActionExecuting(filterContext); // route to normal location
        }
    }
}