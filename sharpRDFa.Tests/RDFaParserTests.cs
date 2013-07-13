using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using sharpRDFa.RDF;

namespace sharpRDFa.Tests
{
    [TestFixture]
    public class RDFaParserTests
    {
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

    }
}
