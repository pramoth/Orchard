using Orchard.UI.Resources;

namespace CodeSanook.FacebookConnect
{
    public class ResourceRegistration : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            //It will automatic lookup in Style folder 
            //The latest version will get picked first 
            manifest.DefineStyle("FontAwesome").SetUrl("font-awesome.css", "font-awesome.min.css")
                .SetVersion("4.7.0");

            manifest.DefineStyle("CommentStyle").SetUrl("comment-style.css");
        }
    }
}