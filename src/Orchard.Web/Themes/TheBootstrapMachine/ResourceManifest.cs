using Orchard.UI.Resources;

namespace Orchard.Gallery
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();
            manifest.DefineStyle("ThemeStyle").SetUrl("theme-style.js");
        }
    }
}
