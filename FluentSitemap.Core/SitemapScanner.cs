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
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace FluentSitemap.Core
{
    public class SitemapScanner
    {
        private readonly HttpContextBase _httpContextBase;

        public SitemapScanner(HttpContextBase httpContextBase)
        {
            _httpContextBase = httpContextBase;
        }

        public ISitemap Create()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            return GetSitemap(new [] {assembly});
        }

        public ISitemap Create(IEnumerable<Assembly> assemblies)
        {
            return GetSitemap(assemblies);
        }

        private ISitemap GetSitemap(IEnumerable<Assembly> assemblies)
        {
            ISitemap sitemap = new Sitemap(_httpContextBase);

            var types = new List<Type>();

            // Get all the controllers 
            // from all the asemblies passed in
            assemblies.ToList()
                      .ForEach(assembly => types.AddRange(assembly.GetTypes()
                                                                  .Where(t => (t.IsClass && t.BaseType == typeof(Controller)))
                                                          )
                              );

            foreach (var type in types)
            {
                var controller = type.Name.ToLower().Replace("controller", string.Empty); // replace controller

                MethodInfo[] methods = type.GetMethods();

                // Loop through all the methods picking out the controller actions.
                foreach (var method in methods)
                {
                    // make sure the method is an action
                    if (!(method.IsPublic && method.ReturnType.IsAssignableFrom(typeof(ActionResult))))
                        continue;

                    var action = method.Name;

                    if (Attribute.IsDefined(method, typeof(ActionNameAttribute)))
                    {
                        var attribute = Attribute.GetCustomAttribute(method, typeof(ActionNameAttribute)) as ActionNameAttribute;

                        if (attribute != null)
                            action = attribute.Name;
                    }

                    // add the site node
                    sitemap.Add(controller, action);
                }
            }
            return sitemap;
        }
    }
}
