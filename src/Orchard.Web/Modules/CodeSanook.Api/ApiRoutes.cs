using Orchard.Mvc.Routes;
using Orchard.WebApi.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CodeSanook.Api
{
    public class ApiRoutes : IHttpRouteProvider
    {
        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new RouteDescriptor[] {
                new HttpRouteDescriptor {
                    Name = "UserApi",
                    Priority = 0,
                    RouteTemplate = "api/user/{id}",
                    Defaults = new {
                        area = "CodeSanook.Api",
                        controller = "UserApi",
                        id = RouteParameter.Optional
                    }
                }
            };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (RouteDescriptor routeDescriptor in GetRoutes())
            {
                routes.Add(routeDescriptor);
            }
        }
    }
}