using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentSitemap.Core
{
    public static class SitemapHelper
    {
        public static string GetControllerName(Type type)
        {
            string controllerName = type.Name;

            if (controllerName.EndsWith("Controller", StringComparison.InvariantCultureIgnoreCase))
                controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);

            return controllerName;
        }

        public static string GetActionName(MethodInfo action)
        {
            string actionName = action.Name;

            var actionNameAttributes = (ActionNameAttribute[])action.GetCustomAttributes(typeof(ActionNameAttribute), false);

            if ((actionNameAttributes != null) && (actionNameAttributes.Length > 0))
                actionName = actionNameAttributes[0].Name;

            return actionName;
        }

        public static RouteValueDictionary GetParameters(MethodCallExpression method)
        {
            var parameters = method.Method.GetParameters();
            var values = new RouteValueDictionary();

            for (int i = 0; i < parameters.Length; i++ )
            {
                var value = ((ConstantExpression) method.Arguments[i]).Value;
                var name = parameters[i].Name;

                values.Add(name, value);
            }

            return values;
        }
    }
}
