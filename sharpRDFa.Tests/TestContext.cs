using System.Collections.Generic;
using System.IO;
using HtmlAgilityPack;

namespace sharpRDFa.Tests
{
    public class TestContext
    {
        public HtmlNode GetElement(string filePath, string xPath)
        {
            var document = new HtmlDocument();
            document.LoadHtml(File.ReadAllText(filePath));
            return document.DocumentNode.SelectSingleNode(xPath);
        }

        public HtmlDocument GetHtmlDocument(string path)
        {
            var document = new HtmlDocument();
            document.LoadHtml(File.ReadAllText(path));
            return document;
        }

        public HtmlNode GetRootElement(string path)
        {
            var document = new HtmlDocument();
            document.LoadHtml(File.ReadAllText(path));
            return document.DocumentNode.SelectSingleNode("//html");
        }

        public ParserContext GetParserContext()
        {
            var context = new ParserContext { UriMappings = new Dictionary<string, string>() };
            return context;
        }
    }
}
