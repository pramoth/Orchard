using Orchard.UI.Resources;

namespace Orchard.Resources {
    public class AngularJs : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            const string angularVersion = "1.5.9";
            manifest.DefineScript("AngularJs").SetUrl("angular.min.js", "angular.js")
                .SetVersion(angularVersion);
            manifest.DefineScript("AngularJs_Sanitize").SetUrl("angular-sanitize.min.js", "angular-sanitize.js")
                .SetVersion(angularVersion).SetDependencies("AngularJs");
            manifest.DefineScript("AngularJs_Resource").SetUrl("angular-resource.min.js", "angular-resource.js")
                .SetVersion(angularVersion).SetDependencies("AngularJs");

            manifest.DefineScript("AngularJs_Sortable").SetUrl("angular-sortable.min.js", "angular-sortable.js")
                .SetDependencies("AngularJs", "jQueryUI_Sortable");

            manifest.DefineScript("AngularJs_Full")
                .SetDependencies("AngularJs", "AngularJs_Sanitize", "AngularJs_Resource", "AngularJs_Sortable");
        }
    }
}
