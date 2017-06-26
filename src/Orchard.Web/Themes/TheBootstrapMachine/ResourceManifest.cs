using Orchard.UI.Resources;

namespace Orchard.Gallery
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();
            manifest.DefineScript("Bootstrap").SetUrl("bootstrap.js");
            manifest.DefineStyle("Bootstrap").SetUrl("bootstrap.css");

        }
    }
}
