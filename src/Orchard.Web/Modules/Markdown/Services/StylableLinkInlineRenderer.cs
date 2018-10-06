using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax.Inlines;

namespace Markdown.Services
{
    public class StylableLinkInlineRenderer : HtmlObjectRenderer<LinkInline>
    {
        public bool AutoRelNoFollow { get; set; }

        protected override void Write(HtmlRenderer renderer, LinkInline link)
        {
            if (renderer.EnableHtmlForInline)
            {
                renderer.Write(link.IsImage ? "<img class=\"img-responsive\" src=\"" : "<a href=\"");
                renderer.WriteEscapeUrl(link.GetDynamicUrl != null ? link.GetDynamicUrl() ?? link.Url : link.Url);
                renderer.Write("\"");
                renderer.WriteAttributes(link);
            }
            if (link.IsImage)
            {
                if (renderer.EnableHtmlForInline)
                {
                    renderer.Write(" alt=\"");
                }
                var wasEnableHtmlForInline = renderer.EnableHtmlForInline;
                renderer.EnableHtmlForInline = false;
                renderer.WriteChildren(link);
                renderer.EnableHtmlForInline = wasEnableHtmlForInline;
                if (renderer.EnableHtmlForInline)
                {
                    renderer.Write("\"");
                }
            }

            if (renderer.EnableHtmlForInline && !string.IsNullOrEmpty(link.Title))
            {
                renderer.Write(" title=\"");
                renderer.WriteEscape(link.Title);
                renderer.Write("\"");
            }

            if (link.IsImage)
            {
                if (renderer.EnableHtmlForInline)
                {
                    renderer.Write(" />");
                }
            }
            else
            {
                if (renderer.EnableHtmlForInline)
                {
                    if (AutoRelNoFollow)
                    {
                        renderer.Write(" rel=\"nofollow\"");
                    }
                    renderer.Write(">");
                }
                renderer.WriteChildren(link);
                if (renderer.EnableHtmlForInline)
                {
                    renderer.Write("</a>");
                }
            }
        }
    }
}


