using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;

namespace Markdown.Services
{
    public class ResponsiveImage : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            var htmlRenderer = renderer as HtmlRenderer;
            if (htmlRenderer != null)
            {
                // Extend the rendering here.
                var inline = renderer.ObjectRenderers.FindExact<LinkInlineRenderer>();
                if (inline != null)
                {
                    renderer.ObjectRenderers.Remove(inline);
                }
                renderer.ObjectRenderers.Add(new StylableLinkInlineRenderer());
            }
        }
    }
}


