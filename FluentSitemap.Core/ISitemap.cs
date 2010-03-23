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
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;

namespace FluentSitemap.Core
{
    public interface ISitemap
    {
        /// <summary>
        /// Add a user created sitemap node
        /// </summary>
        /// <param name="node">the user sitemap node</param>
        /// <returns></returns>
        ISitemap Add(ISitemapNode node);
        /// <summary>
        /// Add a sitemap node by using a controller and action
        /// </summary>
        /// <param name="controller">the controller name</param>
        /// <param name="action">the action name</param>
        /// <returns>the sitemap</returns>
        ISitemap Add(string controller, string action);
        /// <summary>
        /// Creates a sitemap node attached to the existing sitemap
        /// </summary>
        /// <param name="controller">controller name</param>
        /// <param name="action">action name</param>
        /// <returns>the sitemap node</returns>
        ISitemap Add<TController>(Expression<Func<TController, ActionResult>> action)
            where TController : Controller;
        /// <summary>
        /// Add a sitemap node using a controller, action, and parameters
        /// </summary>
        /// <param name="controller">the controller name</param>
        /// <param name="action">the action name</param>
        /// <param name="parameters">the parameters</param>
        /// <returns>the sitemap</returns>
        ISitemap Add(string controller, string action, object parameters);
        /// <summary>
        /// Add many nodes at once using controller, action, and a function that returns a collection of parameters
        /// </summary>
        /// <param name="controller">the controller name</param>
        /// <param name="action">the action name</param>
        /// <param name="listParameters">a list of parameters</param>
        /// <returns>the sitemap</returns>
        ISitemap Add(string controller, string action, Func<IEnumerable<object>> listParameters);
        /// <summary>
        /// Use a route to add a node
        /// </summary>
        /// <param name="routeName">the route name</param>
        /// <param name="routeValues">the route values i.e. controller, action, parameters</param>
        /// <returns></returns>
        ISitemap AddFromRoute(string routeName, object routeValues);
        /// <summary>
        /// Use a route to add a node
        /// </summary>
        /// <param name="routeName">the route name</param>
        /// <param name="routeValues">the route values i.e. controller, action, parameters</param>
        /// <returns></returns>
        ISitemap AddFromRoute(string routeName, RouteValueDictionary routeValues);
        /// <summary>
        /// Use a route to add a node
        /// </summary>
        /// <param name="routeName">the route name</param>
        /// <param name="listRouteValues">the list of route values i.e. controller, action, parameters</param>
        /// <returns></returns>
        ISitemap AddFromRoute(string routeName, Func<IEnumerable<object>> listRouteValues);

        /// <summary>
        /// Creates a sitemap node attached to the existing sitemap
        /// </summary>
        /// <returns>the sitemap node</returns>
        ISitemapNode Create();
        /// <summary>
        /// Creates a sitemap node attached to the existing sitemap
        /// </summary>
        /// <param name="controller">controller name</param>
        /// <param name="action">action name</param>
        /// <returns>the sitemap node</returns>
        ISitemapNode Create(string controller, string action);
        /// <summary>
        /// Creates a sitemap node attached to the existing sitemap
        /// </summary>
        /// <param name="controller">controller name</param>
        /// <param name="action">action name</param>
        /// <param name="parameters">the parameters</param>
        /// <returns>the sitmap node</returns>
        ISitemapNode Create(string controller, string action, object parameters);
        /// <summary>
        /// Creates many sitemap nodes attached to the existing sitemap
        /// </summary>
        /// <param name="controller">controller name</param>
        /// <param name="action">action name</param>
        /// <param name="listParameters">list of parameters</param>
        /// <param name="settingsPerNode">an action to be executed on each new node</param>
        /// <returns></returns>
        ISitemap Create(string controller, string action, Func<IEnumerable<object>> listParameters, Action<ISitemapNode> settingsPerNode);
        /// <summary>
        /// Creates a sitemap node attached to the existing sitemap
        /// </summary>
        /// <param name="routeName">route name</param>
        /// <param name="routeValues">the route values i.e. controller, action, parameters</param>
        /// <returns>the sitemap node</returns>
        ISitemapNode CreateFromRoute(string routeName, object routeValues);

        /// <summary>
        /// Creates a sitemap node attached to the existing sitemap
        /// </summary>
        /// <param name="routeName">the route name</param>
        /// <param name="routeValues">the route values i.e. controller, action, parameters</param>
        /// <returns></returns>
        ISitemapNode CreateFromRoute(string routeName, RouteValueDictionary routeValues);
        /// <summary>
        /// Creates many sitemap nodes attached to the existing sitemap
        /// </summary>
        /// <param name="routeName">the route name</param>
        /// <param name="listRouteValues">the list of route values</param>
        /// <param name="settingsPerNode">the action to be executed on each node</param>
        /// <returns></returns>
        ISitemap CreateFromRoute(string routeName, Func<IEnumerable<object>> listRouteValues, Action<ISitemapNode> settingsPerNode);

        /// <summary>
        /// Return a xml document of the sitemap
        /// </summary>
        /// <returns></returns>
        XDocument Xml();
        /// <summary>
        /// Save the sitemap to the filesystem
        /// </summary>
        /// <param name="path">The file path, can be relative or absolute</param>
        void Save(string path);
        
    }
}
