using FluentSitemap.Core;
using FluentSitemap.Sample.Controllers;

namespace FluentSitemap.Sample.Models
{
    public class HomeControllerSitemapMetadata : ISitemapMetadata
    {
        private const string Home = "Home";

        public void Create(ISitemap sitemap)
        {
            sitemap.Add(Home, "Index")
                   .Add(Home, "Scanner")
                   .Add<HomeController>(c => c.Metadata());
        }
    }

    public class OtherControllerSitemapMetadata : ISitemapMetadata
    {
        private const string Other = "Other";

        public void Create(ISitemap sitemap)
        {
            sitemap.Add(Other, "Index")
                   .Add<OtherController>(c => c.Test(1, "dude"));

        }
    }
}