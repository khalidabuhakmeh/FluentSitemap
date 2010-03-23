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
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentSitemap.Core
{
    public static class SitemapHelper
    {
        public static string GetControllerName<T>()
        {
            return GetControllerName(typeof (T));
        }

        public static string GetControllerName(Type type)
        {
            string controllerName = type.Name;

            if (controllerName.EndsWith("Controller", StringComparison.InvariantCultureIgnoreCase))
                controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);

            return controllerName;
        }

        public static string GetActionName<T>(Expression<Func<T, object>> actionExpression) where T : Controller
        {
            var methodCallExpression = actionExpression.Body as MethodCallExpression;
            if (methodCallExpression == null)
                throw new NotSupportedException("Expression signature is not yet supported");

            return GetActionName(methodCallExpression.Method);
        }

        public static string GetActionName(MethodInfo action)
        {
            string actionName = action.Name;

            var actionNameAttributes =
                (ActionNameAttribute[]) action.GetCustomAttributes(typeof (ActionNameAttribute), false);

            if ((actionNameAttributes != null) && (actionNameAttributes.Length > 0))
                actionName = actionNameAttributes[0].Name;

            return actionName;
        }

        public static RouteValueDictionary GetParameters(MethodCallExpression method)
        {
            ParameterInfo[] parameters = method.Method.GetParameters();
            var values = new RouteValueDictionary();

            for (int i = 0; i < parameters.Length; i++)
            {
                object value = ((ConstantExpression) method.Arguments[i]).Value;
                string name = parameters[i].Name;

                values.Add(name, value);
            }

            return values;
        }
    }
}