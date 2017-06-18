using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Pluralsight.Movies.Models;

namespace Pluralsight.Movies.Drivers
{
    public class MoviePartDriver : ContentPartDriver<MoviePart>
    {
        protected override string Prefix => "Movie";

        protected override DriverResult Editor(MoviePart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Movie_Edit", () =>
            shapeHelper.EditorTemplate(TemplateName: "Parts/Movie", Model: part, Prefix: this.Prefix));

        }
    }
}