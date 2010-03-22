using System;
using System.Text;
using System.Web.Mvc;
using FluentSitemap.Core;

namespace FluentSitemap.Sample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // You can pass in a HttpContext from anywhere
            // in you application
            ISitemap sitemap = new Sitemap(HttpContext);

                    // Add From a controller and action
            sitemap.Add("Home", "Index")
                   .Add("Home", "About")
                    // Add From a Route
                   .AddFromRoute("Default", new {id = "2"})
                    // Create Allows you to create a node
                    // and then edit the values
                   .CreateFromRoute("Default", new {controller = "home", action = "index"})
                        .WithPriority(0.3)
                        .WithChangeFrequency(ChangeFrequencyType.Never)
                        .Set() // <-- Call set to get back to SiteMap
                    // Create From a Route allows you to
                    // pass in a list of route values for dynamic routes
                   .CreateFromRoute("Default", () => new[] {new {id = 1}, new {id = 2}, new {id = 3}}
                                             , sn => sn.WithLastModified(DateTime.Now))
                    // Create allows you to pass in a list of parameters
                    // and modify each node
                   .Create("Home", "Index", () => new[] {new {id = 1}, new {id = 2}, new {id = 3}}
                                          , sn => sn.WithLastModified(DateTime.Now))
                    // Add a sitemap node the way I want to
                    .Add(new SitemapNode {Location = "http://localhost/mycustomnode/"})
                    // save the site map to the directory of the site
                   .Save("~/sitemap.xml");
                   
            // You can also get the Xml Document
            return Content(sitemap.Xml().ToString(), "text/xml", Encoding.UTF8);
        }
    }
}
