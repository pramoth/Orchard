using Orchard.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Pluralsight.Movies
{
    public class Routes : IRouteProvider
    {
        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
	            new RouteDescriptor {
		            Route = new Route(
			            "Admin/Actors",
			            new RouteValueDictionary {
				            {"area", "Pluralsight.Movies"},
				            {"controller", "ActorsAdmin"},
				            {"action", "Index"}
			            },
			            new RouteValueDictionary(),
			            new RouteValueDictionary {
				            {"area", "Pluralsight.Movies"}
			            },
			            new MvcRouteHandler())
	            },
                new RouteDescriptor {
                    Route = new Route(
                        "Admin/Actors/{action}",
                        new RouteValueDictionary {
                            {"area", "Pluralsight.Movies"},
                            {"controller", "ActorsAdmin"},
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "Pluralsight.Movies"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Route = new Route(
                        "Actors/{actorId}",
                        new RouteValueDictionary {
                            {"area", "Pluralsight.Movies"},
                            {"controller", "Actor"},
                            {"action", "Details"}
                        },
                        new RouteValueDictionary(new { actorId = @"\d+" }),
                        new RouteValueDictionary {
                            {"area", "Pluralsight.Movies"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Route = new Route(
                        "Actors/{actorName}",
                        new RouteValueDictionary {
                            {"area", "Pluralsight.Movies"},
                            {"controller", "Actor"},
                            {"action", "DetailsByName"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "Pluralsight.Movies"}
                        },
                        new MvcRouteHandler())
                }
            };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var route in GetRoutes()) {
                routes.Add(route);
            }
        }
    }
}