using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagementTool.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {            
            ViewData["SubTitle"] = "Welcome in ASP.NET MVC 5 INSPINIA SeedProject ";
            ViewData["Message"] = "It is an application skeleton for a typical MVC 5 project. You can use it to quickly bootstrap your webapp projects.";

            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }
    }
}