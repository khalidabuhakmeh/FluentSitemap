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
