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
using System.Web;
using System.Xml.Linq;

namespace FluentSitemap.Core
{
    public class Sitemap : ISitemap
    {
        private readonly HttpContextBase _context;
        private readonly IEnumerable<ISitemapNode> _nodes;

        internal Sitemap(IEnumerable<ISitemapNode> nodes, HttpContextBase context)
        {
            _nodes = nodes;
            _context = context;
        }

        #region ISitemap Members

        /// <summary>
        /// Return a xml document of the SiteMapConfigurator
        /// </summary>
        /// <returns></returns>
        public XDocument Xml()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var nodes = new List<XElement>();

            foreach (ISitemapNode sitemapNode in _nodes)
            {
                SitemapNodeValidator.Validate(sitemapNode);

                var elements = new List<XElement> {new XElement(ns + "loc", sitemapNode.Location)};

                // every node has a location

                if (sitemapNode.LastModified.HasValue)
                    elements.Add(new XElement(ns + "lastmod", sitemapNode.LastModified.Value.ToString("yyyy-MM-dd")));

                if (sitemapNode.ChangeFrequency.HasValue)
                    elements.Add(new XElement(ns + "changefreq", sitemapNode.ChangeFrequency.Value.ToString().ToLower()));

                if (sitemapNode.Priority.HasValue)
                    elements.Add(new XElement(ns + "priority", sitemapNode.Priority.Value.ToString()));

                nodes.Add(new XElement(ns + "url", elements));
            }

            var root = new XElement(ns + "urlset", nodes);

            var document = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), root);

            return document;
        }

        /// <summary>
        /// Save the SiteMapConfigurator to the filesystem
        /// </summary>
        /// <param name="path">The file path, can be relative or absolute</param>
        public void Save(string path)
        {
            string newPath = path;

            if (path == null) throw new ArgumentNullException("path");

            if (path.StartsWith("~/"))
                newPath = _context.Server.MapPath(path);

            Xml().Save(newPath);
        }

        #endregion
    }
}