using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FluentSitemap.Sample.Controllers
{
    public class OtherController : Controller
    {
        public ActionResult Index()
        {
            return Content("Hello, World");
        }

        public ActionResult Test(int id, string dude)
        {
            return Content(id.ToString());
        }

    }
}
