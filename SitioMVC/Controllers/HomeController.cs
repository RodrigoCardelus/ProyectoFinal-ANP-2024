using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EC;

namespace SitioMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["EmpLogueado"] = null;
            return View();
        }
    }
}