using System.Text.RegularExpressions;

namespace WebApplicationExample.Routing
{
    public class CustomConstrains : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(routeKey))
            {
                return false;
            }

            Regex regex = new Regex($"^(apr|jul|oct|jan)$");
            string? montheValue = Convert.ToString(values[routeKey]);

            if (!regex.IsMatch(montheValue))
            {
                return false;
            }

            return true;
        }
    }
}
