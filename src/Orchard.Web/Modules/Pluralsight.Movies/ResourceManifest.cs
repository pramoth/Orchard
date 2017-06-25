using Orchard.UI.Resources;

namespace Pluralsight.Movies {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            //It will search in module's Style folder
            manifest.DefineStyle("Movies").SetUrl("Pluralsight-Movies.css");
        }
    }
}