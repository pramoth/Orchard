using Orchard.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions.Models;

namespace Pluralsight.Movies
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission LookupMovie = new Permission()
        {
            Description = "Lookup movie thought the TMDb API",
            Name = "LookupMovie"
        };

        public Feature Feature { get; set; }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype()
                {
                    Name = "Administrator",
                    Permissions = new []{LookupMovie}
                }
            };
        }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] { LookupMovie };
        }
    }
}