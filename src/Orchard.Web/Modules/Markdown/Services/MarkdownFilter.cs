using System;
using Orchard.Services;
using Markdig;

namespace Markdown.Services
{
    public class MarkdownFilter : IHtmlFilter
    {
        public string ProcessContent(string text, string flavor)
        {
            return String.Equals(flavor, "markdown", StringComparison.OrdinalIgnoreCase) ? MarkdownReplace(text) : text;
        }

        //https://github.com/lunet-io/markdig/blob/master/src/Markdig.Tests/Specs/Specs.cs
        //https://github.com/lunet-io/markdig/blob/master/src/Markdig.Benchmarks/spec.md
        private static string MarkdownReplace(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var pipeline = new MarkdownPipelineBuilder()
                .Use<ResponsiveImage>().Build();
            return Markdig.Markdown.ToHtml(text, pipeline);
        }
    }
}