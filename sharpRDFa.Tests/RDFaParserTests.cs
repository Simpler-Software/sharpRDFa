using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;

namespace sharpRDFa.Tests
{
    [TestFixture]
    public class RDFaParserTests
    {
        private TestContext _testContext;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _testContext = new TestContext();
        }

        [Test]
        public void AcceptanceTest_XHTML_RDFa_1_0()
        {
            var parser = new RDFaParser();
            var triples = parser.GetRDFTriplesFromFile("Resource\\XHTML+RDFa 1.0.html");

            Assert.IsNotNull(triples);
            Assert.AreEqual(7, triples.Count);
            Assert.AreEqual("http://example.org/john-d/", triples[0].Subject);
            Assert.AreEqual("http://purl.org/dc/elements/1.1/creator", triples[0].Predicate);
            Assert.AreEqual("Jonathan Doe", triples[0].Object.Literal);

            Assert.AreEqual("http://example.org/john-d/", triples[1].Subject);
            Assert.AreEqual("http://xmlns.com/foaf/0.1/primaryTopic", triples[1].Predicate);
            Assert.AreEqual("http://example.org/john-d/#me", triples[1].Object.Uri);

            Assert.AreEqual("http://example.org/john-d/#me", triples[2].Subject);
            Assert.AreEqual("http://xmlns.com/foaf/0.1/nick", triples[2].Predicate);
            Assert.AreEqual("John D", triples[2].Object.Literal);

            Assert.AreEqual("http://example.org/john-d/#me", triples[3].Subject);
            Assert.AreEqual("http://xmlns.com/foaf/0.1/interest", triples[3].Predicate);
            Assert.AreEqual("http://www.neubauten.org/", triples[3].Object.Uri);

            Assert.AreEqual("http://example.org/john-d/#me", triples[4].Subject);
            Assert.AreEqual("http://xmlns.com/foaf/0.1/interest", triples[4].Predicate);
            Assert.AreEqual("urn://ISBN:0752820907", triples[4].Object.Uri);

            Assert.AreEqual("urn://ISBN:0752820907", triples[5].Subject);
            Assert.AreEqual("http://purl.org/dc/elements/1.1/title", triples[5].Predicate);
            Assert.AreEqual("Weaving the Web", triples[5].Object.Literal);

            Assert.AreEqual("urn://ISBN:0752820907", triples[6].Subject);
            Assert.AreEqual("http://purl.org/dc/elements/1.1/creator", triples[6].Predicate);
            Assert.AreEqual("Tim Berners-Lee", triples[6].Object.Literal);

        }

        //[Test]
        public void AcceptanceTest_IMDB_The_Rock()
        {
            var parser = new RDFaParser();
            var triples = parser.GetRDFTriplesFromFile("Resource\\IMDB_The_Rock.html").Where(x => x.Subject != null && x.Predicate != null).ToList();
        }

        //[Test]
        public void AcceptanceTest_CharlesRobertDarwin()
        {
            var parser = new RDFaParser();
            var triples = parser.GetRDFTriplesFromFile("Resource\\CharlesRobertDarwin.html");
        }
        
        [Test]
        public void AcceptanceTest_Alice_Example()
        {
            var parser = new RDFaParser();
            var triples = parser.GetRDFTriplesFromFile("Resource\\alice-example.html");


        }

        [Test]
        public void UpdateUriMappings_WithValidElementNode_RetrunsExpectedMappings()
        {
            // Arrange
            var parser = new RDFaParser();
            var context = _testContext.GetParserContext();
            var rootElement = _testContext.GetRootElement("Resource\\XHTML+RDFa 1.0.html");
            
            // Act
            var response = parser.UpdateUriMappings(context, rootElement);
            
            // Assert
            Assert.AreEqual(context.UriMappings, response);
            Assert.AreEqual(2, response.Count);
            Assert.AreEqual("http://xmlns.com/foaf/0.1/", response["foaf"]);
            Assert.AreEqual("http://purl.org/dc/elements/1.1/", response["dc"]);
        }

        [Test]
        public void UpdateUriMappings_WithValidElementNodeAndExistingMapping_RetrunsUpdatedMappings()
        {
            // Arrange
            var parser = new RDFaParser();
            var context = _testContext.GetParserContext();
            context.UriMappings.Add("dc", "http://purl.org/dc/elements/1.0/");
            var rootElement = _testContext.GetRootElement("Resource\\XHTML+RDFa 1.0.html");

            // Act
            var response = parser.UpdateUriMappings(context, rootElement);

            // Assert
            Assert.AreEqual(context.UriMappings, response);
            Assert.AreEqual(2, response.Count);
            Assert.AreEqual("http://xmlns.com/foaf/0.1/", response["foaf"]);
            Assert.AreEqual("http://purl.org/dc/elements/1.1/", response["dc"]);
            Assert.AreNotEqual("http://purl.org/dc/elements/1.0/", response["dc"]);
        }

        [Test]
        public void UpdateUriMappings_WithValidElementNode_RetrunsExpectedMappings_2()
        {
            // Arrange
            var parser = new RDFaParser();
            var context = _testContext.GetParserContext();
            var element = _testContext.GetElement("Resource\\alice-example.html", "//head");

            // Act
            var response = parser.UpdateUriMappings(context, element);

            // Assert
            Assert.AreEqual(context.UriMappings, response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual("http://ogp.me/ns#", response["og"]);
        }

        [Test]
        public void GetBaseURI_WithHtmlNode_ReturnsExpectedBaseURI()
        {
            // Arrange
            var parser = new RDFaParser();
            var element = _testContext.GetHtmlDocument("Resource\\alice-example.html");

            // Act
            var result = parser.GetBaseURI(element);

            // Assert
            Assert.AreEqual("http://example.com/", result);
        }

        

        

        
    }
}
