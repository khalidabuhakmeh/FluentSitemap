using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Xml.Linq;

namespace FluentSitemap.Core
{
    public interface ISitemap
    {
        ISitemap Add(ISitemapNode node);
        ISitemap Add(string controller, string action);
        ISitemap Add(string controller, string action, object parameters);
        ISitemap Add(string controller, string action, Func<IEnumerable<object>> listParameters);
        ISitemap AddFromRoute(string routeName, object routeValues);
        ISitemap AddFromRoute(string routeName, RouteValueDictionary routeValues);
        ISitemap AddFromRoute(string routeName, Func<IEnumerable<object>> listRouteValues);

        ISitemapNode Create();
        ISitemapNode Create(string controller, string action);
        ISitemapNode Create(string controller, string action, object parameters);
        ISitemap Create(string controller, string action, Func<IEnumerable<object>> listParameters, Action<ISitemapNode> settingsPerNode);
        ISitemapNode CreateFromRoute(string routeName, object routeValues);
        ISitemapNode CreateFromRoute(string routeName, RouteValueDictionary routeValues);
        ISitemap CreateFromRoute(string routeName, Func<IEnumerable<object>> listRouteValues, Action<ISitemapNode> settingsPerNode);

        XDocument Xml();
        void Save(string path);
        
    }
}
