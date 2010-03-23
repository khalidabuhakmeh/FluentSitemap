// Copyright 2010 Aqua Bird Consulting (http://www.aquabirdconsulting.com)
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://github.com/khalidabuhakmeh/FluentSitemap
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;

namespace FluentSitemap.Core
{
    public class Sitemap : ISitemap
    {
        public IList<ISitemapNode> SiteMapNodes { get; private set; }
        private readonly string _baseUrl;
        private readonly UrlHelper _urlHelper;
        private readonly HttpContextBase _context;

        public Sitemap(HttpContextBase httpContextBase)
        {
            if (httpContextBase == null) throw new ArgumentNullException("httpContextBase");

            SiteMapNodes = new List<ISitemapNode>();
            _context = httpContextBase;
            _baseUrl = string.Format("{0}://{1}{2}", _context.Request.Url.Scheme, _context.Request.Url.Authority, _context.Request.ApplicationPath.TrimEnd('/'));
            _urlHelper = new UrlHelper(_context.Request.RequestContext);
        }

        public ISitemap Add(ISitemapNode node)
        {
            SitemapNodeValidator.Validate(node);

            SiteMapNodes.Add(node);
            return this;
        }

        public ISitemap Add(string controller, string action)
        {
            Create(controller, action);
            return this;
        }

        public ISitemap Add(string controller, string action, object parameters)
        {
            Create(controller, action, parameters);
            return this;
        }

        public ISitemap Add(string controller, string action, Func<IEnumerable<object>> listParameters)
        {
            throw new NotImplementedException();
        }

        public ISitemap AddFromRoute(string routeName, object routeValues)
        {
            CreateFromRoute(routeName, routeValues);
            return this;
        }

        public ISitemap AddFromRoute(string routeName, RouteValueDictionary routeValues)
        {
            CreateFromRoute(routeName, routeValues);
            return this;
        }

        public ISitemap AddFromRoute(string routeName, Func<IEnumerable<object>> listRouteValues)
        {
            CreateFromRoute(routeName, listRouteValues, null);
            return this;
        }

        public ISitemapNode Create()
        {
            var node = new SitemapNode(this);
            SiteMapNodes.Add(node);

            return node;
        }

        public ISitemapNode Create(string controller, string action)
        {
            var node = new SitemapNode(this).WithLocation(Url(_urlHelper.Action(action, controller)));
            SiteMapNodes.Add(node);

            return node;
        }

        public ISitemapNode Create(string controller, string action, object parameters)
        {
            var node = new SitemapNode(this).WithLocation(Url(_urlHelper.Action(action, controller, parameters)));
            SiteMapNodes.Add(node);

            return node;
        }

        public ISitemap Create(string controller, string action, Func<IEnumerable<object>> listParameters, Action<ISitemapNode> settingsPerNode)
        {
            foreach (var parameters in listParameters.Invoke())
            {
                var node = Create(controller, action, parameters);

                if (settingsPerNode != null)
                    settingsPerNode.Invoke(node);
            }

            return this;
        }

        public ISitemapNode CreateFromRoute(string routeName, object routeValues)
        {
            var node = new SitemapNode(this).WithLocation(Url(_urlHelper.RouteUrl(routeName, routeValues)));
            SiteMapNodes.Add(node);

            return node;
        }

        public ISitemapNode CreateFromRoute(string routeName, RouteValueDictionary routeValues)
        {
            var node = new SitemapNode(this).WithLocation(Url(_urlHelper.RouteUrl(routeName, routeValues)));
            SiteMapNodes.Add(node);

            return node;
        }

        public ISitemap CreateFromRoute(string routeName, Func<IEnumerable<object>> listRouteValues, Action<ISitemapNode> settingsPerNode)
        {
            foreach (var node in listRouteValues.Invoke()
                     .Select(values => CreateFromRoute(routeName, values)))
            {
                if (settingsPerNode != null)
                    settingsPerNode.Invoke(node);
            }

            return this;
        }

        public XDocument Xml()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var nodes = new List<XElement>();

            foreach (var sitemapNode in SiteMapNodes)
            {
                SitemapNodeValidator.Validate(sitemapNode);

                var elements = new List<XElement>();

                // every node has a location
                elements.Add(new XElement(ns + "loc", sitemapNode.Location));

                if (sitemapNode.LastModified.HasValue)
                    elements.Add(new XElement(ns + "lastmod", sitemapNode.LastModified.Value.ToString("yyyy-MM-dd")));

                if (sitemapNode.ChangeFrequency.HasValue)
                    elements.Add( new XElement(ns + "changefreq", sitemapNode.ChangeFrequency.Value.ToString().ToLower() ));

                if (sitemapNode.Priority.HasValue)
                    elements.Add(new XElement(ns + "priority", sitemapNode.Priority.Value.ToString()));

                nodes.Add(new XElement(ns + "url", elements));
            }

            var root = new XElement(ns + "urlset",  nodes);

            var document = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), root);

            return document;
        }

        public void Save(string path)
        {
            var newPath = path;

            if (path == null) throw new ArgumentNullException("path");
            
            if (path.StartsWith("~/"))
                newPath = _context.Server.MapPath(path);

            Xml().Save(newPath);
        }

        private string Url(string routedUrl)
        {
            return string.Format("{0}{1}", _baseUrl, routedUrl);
        }
    }
}