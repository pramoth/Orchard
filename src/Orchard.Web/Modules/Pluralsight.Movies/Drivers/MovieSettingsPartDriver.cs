using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Pluralsight.Movies.Models;

namespace Pluralsight.Movies.Drivers {
    public class MovieSettingsPartDriver : ContentPartDriver<MovieSettingsPart> {
         protected override string Prefix {
            get { return "MovieSettings"; }
        }

        protected override DriverResult Editor(MovieSettingsPart part, dynamic shapeHelper) {
            return ContentShape("Parts_Movies_SiteSettings",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/MovieSettings",
                    Model: part,
                    Prefix: Prefix)).OnGroup("Movies");
        }

        protected override DriverResult Editor(MovieSettingsPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}