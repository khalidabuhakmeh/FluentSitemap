using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentSitemap.Core
{
    public class SitemapConfigurator : ISitemapConfigurator
    {
        private readonly string _baseUrl;
        private readonly HttpContextBase _context;
        private readonly UrlHelper _urlHelper;

        /// <summary>
        /// Create a sitemap
        /// </summary>
        /// <param name="httpContextBase">The http context</param>
        public SitemapConfigurator(HttpContextBase httpContextBase)
        {
            if (httpContextBase == null) throw new ArgumentNullException("httpContextBase");

            SiteMapNodes = new List<ISitemapNode>();
            _context = httpContextBase;
            _baseUrl = string.Format("{0}://{1}{2}", _context.Request.Url.Scheme, _context.Request.Url.Authority,
                                     _context.Request.ApplicationPath.TrimEnd('/'));
            _urlHelper = new UrlHelper(_context.Request.RequestContext);
        }

        private IList<ISitemapNode> SiteMapNodes { get; set; }

        #region ISitemapConfigurator Members

        /// <summary>
        /// Add a user created sitemap node
        /// </summary>
        /// <param name="node">the user sitemap node</param>
        /// <returns></returns>
        public ISitemapConfigurator Add(ISitemapNode node)
        {
            SitemapNodeValidator.Validate(node);

            SiteMapNodes.Add(node);
            return this;
        }

        /// <summary>
        /// Add a sitemap node by using a controller and action
        /// </summary>
        /// <param name="controller">the controller name</param>
        /// <param name="action">the action name</param>
        /// <returns>the sitemap</returns>
        public ISitemapConfigurator Add(string controller, string action)
        {
            Create(controller, action);
            return this;
        }

        /// <summary>
        /// Creates a sitemap node attached to the existing sitemap
        /// </summary>        
        /// <param name="action">action name</param>
        /// <returns>the sitemap node</returns>
        public ISitemapConfigurator Add<TController>(Expression<Func<TController, ActionResult>> action)
            where TController : Controller
        {
            Create(action);
            return this;
        }

        /// <summary>
        /// Add a sitemap node using a controller, action, and parameters
        /// </summary>
        /// <param name="controller">the controller name</param>
        /// <param name="action">the action name</param>
        /// <param name="parameters">the parameters</param>
        /// <returns>the sitemap</returns>
        public ISitemapConfigurator Add(string controller, string action, object parameters)
        {
            Create(controller, action, parameters);
            return this;
        }

        /// <summary>
        /// Add many nodes at once using controller, action, and a function that returns a collection of parameters
        /// </summary>
        /// <param name="controller">the controller name</param>
        /// <param name="action">the action name</param>
        /// <param name="listParameters">a list of parameters</param>
        /// <returns>the sitemap</returns>
        public ISitemapConfigurator Add(string controller, string action, Func<IEnumerable<object>> listParameters)
        {
            return Create(controller, action, listParameters, null);
        }

        public ISitemapConfigurator Add<TController>(Expression<Func<TController, object>> actionExpression,
                                                     Func<IEnumerable<object>> listParameters)
            where TController : Controller
        {
            string controller = SitemapHelper.GetControllerName<TController>();
            return Create(controller, SitemapHelper.GetActionName(actionExpression), listParameters, null);
        }

        /// <summary>
        /// Use a route to add a node
        /// </summary>
        /// <param name="routeName">the route name</param>
        /// <param name="routeValues">the route values i.e. controller, action, parameters</param>
        /// <returns></returns>
        public ISitemapConfigurator AddFromRoute(string routeName, object routeValues)
        {
            CreateFromRoute(routeName, routeValues);
            return this;
        }

        /// <summary>
        /// Use a route to add a node
        /// </summary>
        /// <param name="routeName">the route name</param>
        /// <param name="routeValues">the route values i.e. controller, action, parameters</param>
        /// <returns></returns>
        public ISitemapConfigurator AddFromRoute(string routeName, RouteValueDictionary routeValues)
        {
            CreateFromRoute(routeName, routeValues);
            return this;
        }

        /// <summary>
        /// Use a route to add a node
        /// </summary>
        /// <param name="routeName">the route name</param>
        /// <param name="listRouteValues">the list of route values i.e. controller, action, parameters</param>
        /// <returns></returns>
        public ISitemapConfigurator AddFromRoute(string routeName, Func<IEnumerable<object>> listRouteValues)
        {
            CreateFromRoute(routeName, listRouteValues, null);
            return this;
        }

        /// <summary>
        /// Creates a sitemap node attached to the existing sitemap
        /// </summary>
        /// <returns>the sitemap node</returns>
        public ISitemapNode Create()
        {
            var node = new SitemapNode(this);
            SiteMapNodes.Add(node);

            return node;
        }

        /// <summary>
        /// Creates a sitemap node attached to the existing sitemap
        /// </summary>
        /// <param name="controller">controller name</param>
        /// <param name="action">action name</param>
        /// <returns>the sitemap node</returns>
        public ISitemapNode Create(string controller, string action)
        {
            ISitemapNode node = new SitemapNode(this).WithLocation(Url(_urlHelper.Action(action, controller)));
            SiteMapNodes.Add(node);

            return node;
        }

        /// <summary>
        /// Creates a sitemap node attached to the existing sitemap
        /// </summary>
        /// <param name="controller">controller name</param>
        /// <param name="action">action name</param>
        /// <param name="parameters">the parameters</param>
        /// <returns>the sitmap node</returns>
        public ISitemapNode Create(string controller, string action, object parameters)
        {
            ISitemapNode node =
                new SitemapNode(this).WithLocation(Url(_urlHelper.Action(action, controller, parameters)));
            SiteMapNodes.Add(node);

            return node;
        }

        /// <summary>
        /// Creates many sitemap nodes attached to the existing sitemap
        /// </summary>
        /// <param name="controller">controller name</param>
        /// <param name="action">action name</param>
        /// <param name="listParameters">list of parameters</param>
        /// <param name="settingsPerNode">an action to be executed on each new node</param>
        /// <returns></returns>
        public ISitemapConfigurator Create(string controller, string action, Func<IEnumerable<object>> listParameters,
                                           Action<ISitemapNode> settingsPerNode)
        {
            foreach (object parameters in listParameters.Invoke())
            {
                ISitemapNode node = Create(controller, action, parameters);

                if (settingsPerNode != null)
                    settingsPerNode.Invoke(node);
            }

            return this;
        }

        /// <summary>
        /// Creates many sitemap nodes attached to the existing sitemap
        /// </summary>
        /// <param name="actionExpression">action</param>
        /// <param name="listParameters">list of parameters</param>
        /// <param name="settingsPerNode">an action to be executed on each new node</param>
        /// <returns></returns>
        public ISitemapConfigurator Create<TController>(Expression<Func<TController, object>> actionExpression,
                                                        Func<IEnumerable<object>> listParameters,
                                                        Action<ISitemapNode> settingsPerNode)
            where TController : Controller
        {
            string controller = SitemapHelper.GetControllerName<TController>();
            string action = SitemapHelper.GetActionName(actionExpression);

            foreach (object parameters in listParameters.Invoke())
            {
                ISitemapNode node = Create(controller, action, parameters);

                if (settingsPerNode != null)
                    settingsPerNode.Invoke(node);
            }

            return this;
        }

        /// <summary>
        /// Creates a sitemap node attached to the existing sitemap
        /// </summary>
        /// <param name="routeName">route name</param>
        /// <param name="routeValues">the route values i.e. controller, action, parameters</param>
        /// <returns>the sitemap node</returns>
        public ISitemapNode CreateFromRoute(string routeName, object routeValues)
        {
            ISitemapNode node = new SitemapNode(this).WithLocation(Url(_urlHelper.RouteUrl(routeName, routeValues)));
            SiteMapNodes.Add(node);

            return node;
        }

        /// <summary>
        /// Creates a sitemap node attached to the existing sitemap
        /// </summary>
        /// <param name="routeName">the route name</param>
        /// <param name="routeValues">the route values i.e. controller, action, parameters</param>
        /// <returns></returns>
        public ISitemapNode CreateFromRoute(string routeName, RouteValueDictionary routeValues)
        {
            ISitemapNode node = new SitemapNode(this).WithLocation(Url(_urlHelper.RouteUrl(routeName, routeValues)));
            SiteMapNodes.Add(node);

            return node;
        }

        /// <summary>
        /// Creates many sitemap nodes attached to the existing sitemap
        /// </summary>
        /// <param name="routeName">the route name</param>
        /// <param name="listRouteValues">the list of route values</param>
        /// <param name="settingsPerNode">the action to be executed on each node</param>
        /// <returns></returns>
        public ISitemapConfigurator CreateFromRoute(string routeName, Func<IEnumerable<object>> listRouteValues,
                                                    Action<ISitemapNode> settingsPerNode)
        {
            foreach (ISitemapNode node in listRouteValues.Invoke()
                .Select(values => CreateFromRoute(routeName, values)))
            {
                if (settingsPerNode != null)
                    settingsPerNode.Invoke(node);
            }

            return this;
        }

        /// <summary>
        /// Export the sitemap, call when done
        /// </summary>
        /// <returns></returns>
        public ISitemap Export()
        {
            return new Sitemap(SiteMapNodes, _context);
        }

        /// <summary>
        /// Finds all controllers in an assembly and adds all public actions in a controller
        /// </summary>
        /// <typeparam name="T">a type from the assembly you would like to scan</typeparam>
        /// <returns></returns>
        public ISitemapConfigurator FromAssemblyOf<T>()
        {
            FromAssembly(typeof (T).Assembly);
            return this;
        }

        /// <summary>
        /// Finds all controllers in all assemblies and adds all public actions in a controller
        /// </summary>
        /// <param name="assemblies">The assemblies</param>
        /// <returns></returns>
        public ISitemapConfigurator FromAssemblies(IEnumerable<Assembly> assemblies)
        {
            foreach (Assembly assembly in assemblies)
                FromAssembly(assembly);

            return this;
        }

        /// <summary>
        /// Load nodes from metadata classes (made for IoC)
        /// </summary>
        /// <param name="sitemapMetadatas"></param>
        /// <returns></returns>
        public ISitemapConfigurator FromMetaData(IEnumerable<ISitemapMetadata> sitemapMetadatas)
        {
            foreach (ISitemapMetadata sitemapMetadata in sitemapMetadatas)
                sitemapMetadata.Create(this);

            return this;
        }

        #endregion

        /// <summary>
        /// Creates a sitemap node attached to the existing sitemap
        /// </summary>        
        /// <param name="action">action expression</param>        
        /// <returns>the sitmap node</returns>
        public ISitemapNode Create<TController>(Expression<Func<TController, ActionResult>> action)
            where TController : Controller
        {
            string controllerName = SitemapHelper.GetControllerName<TController>();
            MethodInfo methodInfo = ((MethodCallExpression) action.Body).Method;
            string actionName = SitemapHelper.GetActionName(methodInfo);

            RouteValueDictionary parameters = SitemapHelper.GetParameters((MethodCallExpression) action.Body);

            // Create Location
            string location = Url(_urlHelper.Action(actionName, controllerName, parameters));

            ISitemapNode node = new SitemapNode(this).WithLocation(location);
            SiteMapNodes.Add(node);

            return node;
        }

        /// <summary>
        /// Create the fully qualified Url of the executing site
        /// </summary>
        /// <param name="routedUrl">The end of the Url</param>
        /// <returns>the full url</returns>
        private string Url(string routedUrl)
        {
            return string.Format("{0}{1}", _baseUrl, routedUrl);
        }

        private void FromAssembly(Assembly assembly)
        {
            var types = new List<Type>();

            types.AddRange(assembly.GetTypes().Where(t => (t.IsClass && t.BaseType == typeof (Controller))));

            foreach (Type type in types)
            {
                string controller = SitemapHelper.GetControllerName(type);

                MethodInfo[] methods = type.GetMethods();

                // Loop through all the methods picking out the controller actions.
                foreach (MethodInfo method in methods)
                {
                    // make sure the method is an action
                    if (!(method.IsPublic && method.ReturnType.IsAssignableFrom(typeof (ActionResult))))
                        continue;

                    string action = SitemapHelper.GetActionName(method);

                    // add the site node
                    Add(controller, action);
                }
            }
        }
    }
}