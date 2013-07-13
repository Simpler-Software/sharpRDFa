using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using sharpRDFa.RDFTriple;

namespace sharpRDFa.Tests
{
    [TestFixture]
    class RDFTripleBuilderTests
    {
        [Test]
        public void CreateSubject_ValidSubjectValue_SetTriplesSubject()
        {
            // Arrange
            IRDFTripleBuilder builder = new RDFTripleBuilder();
            var uriMappings = new Dictionary<string, string>
                               {
                                   {"foaf", "http://xmlns.com/foaf/0.1/"},
                                   {"dc", "http://purl.org/dc/elements/1.1/"},
                                   {"xsd","http://www.w3.org/2001/XMLSchema#"}
                               };
            // Act
            builder.CreateSubject("http://example.org/john-d/","http://example.org/john-d/", uriMappings);

            var result = builder.GetTriple();

            // Assert
            Assert.AreEqual("http://example.org/john-d/", result.Subject);
        }

        [Test]
        public void CreatePredicate_ValidPredicateValue_SetTriplesPredicate()
        {
            // Arrange
            IRDFTripleBuilder builder = new RDFTripleBuilder();
            var uriMappings = new Dictionary<string, string>
                               {
                                   {"foaf", "http://xmlns.com/foaf/0.1/"},
                                   {"dc", "http://purl.org/dc/elements/1.1/"},
                                   {"xsd","http://www.w3.org/2001/XMLSchema#"}
                               };
            // Act
            builder.CreatePredicate("[dc:creator]","http://example.org/john-d/", uriMappings);

            var result = builder.GetTriple();

            // Assert
            Assert.AreEqual("http://purl.org/dc/elements/1.1/creator", result.Predicate);
        }

        [Test]
        public void CreateObject_ValidObjectLiteralValue_SetTriplesObject()
        {
            // Arrange
            IRDFTripleBuilder builder = new RDFTripleBuilder();
            var uriMappings = new Dictionary<string, string>
                               {
                                   {"foaf", "http://xmlns.com/foaf/0.1/"},
                                   {"dc", "http://purl.org/dc/elements/1.1/"},
                                   {"xsd","http://www.w3.org/2001/XMLSchema#"}
                               };
            // Act
            builder.CreateObject("Jonathan Doe", "en", null, TripleObjectType.Literal, "http://example.org/john-d/", uriMappings);

            var result = builder.GetTriple();

            // Assert
            Assert.IsNotNull(result.Object);
            Assert.AreEqual("Jonathan Doe", result.Object.Literal);
            Assert.AreEqual("en", result.Object.Language);
        }

        [Test]
        public void CreateObject_ValidObjectURIorSafeCURIEValue_SetTriplesObject()
        {
            // Arrange
            IRDFTripleBuilder builder = new RDFTripleBuilder();
            var uriMappings = new Dictionary<string, string>
                               {
                                   {"foaf", "http://xmlns.com/foaf/0.1/"},
                                   {"dc", "http://purl.org/dc/elements/1.1/"},
                                   {"xsd","http://www.w3.org/2001/XMLSchema#"}
                               };
            // Act
            builder.CreateObject("http://example.org/john-d/#me", null, null, TripleObjectType.URIorSafeCURIE, "http://example.org/john-d/", uriMappings);

            var result = builder.GetTriple();

            // Assert
            Assert.IsNotNull(result.Object);
            Assert.AreEqual("http://example.org/john-d/#me", result.Object.Uri);
        }
    }
}
