using FluentSitemap.Core;

namespace FluentSitemap.Sample.Models
{
    public class HomeControllerSitemapMetadata : ISitemapMetadata
    {
        private const string Home = "Home";

        public void Create(ISitemap sitemap)
        {
            sitemap.Add(Home, "Index")
                   .Add(Home, "Scanner")
                   .Add(Home, "SiteMetadata");
        }
    }

    public class OtherControllerSitemapMetadata : ISitemapMetadata
    {
        private const string Other = "Other";

        public void Create(ISitemap sitemap)
        {
            sitemap.Add(Other, "Index");
        }
    }
}