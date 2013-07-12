using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace sharpRDFa.Tests
{
    [TestFixture]
    public class IRDFaProcessorTests
    {
        [Test]
        public void IsNameSpace_Test1()
        {
            // Arrange
            IRDFaProcessor processor = new RDFaProcessor();

            // Act
            var result = processor.IsNameSpace("xmlns:foaf");
            // Assert

            Assert.AreEqual("xmlns", result.Prefix);
            Assert.AreEqual("foaf", result.NCName);
        }

        [Test]
        public void IsNameSpace_Test2()
        {
            // Arrange
            IRDFaProcessor processor = new RDFaProcessor();

            // Act
            var result = processor.IsNameSpace("xmlns");
            // Assert

            Assert.IsNull(result);
        }

        [Test]
        public void IsCURIE_Test1()
        {
            // Arrange
            IRDFaProcessor processor = new RDFaProcessor();
            var uriMappings = new Dictionary<string, string>
                                  {
                                      {"foaf", "http://xmlns.com/foaf/0.1/"},
                                      {"dc", "http://purl.org/dc/elements/1.1/"}
                                  };
            // Act
            var result = processor.IsCURIE("dc:creator", uriMappings);

            // Assert
            Assert.AreEqual("dc:creator", result.Curie);
            Assert.AreEqual("dc", result.Prefix);
            Assert.AreEqual("creator", result.Reference);
        }

        [Test]
        public void IsSafeCURIE_Test1()
        {
            // Arrange
            IRDFaProcessor processor = new RDFaProcessor();
            var uriMappings = new Dictionary<string, string>
                                  {
                                      {"foaf", "http://xmlns.com/foaf/0.1/"},
                                      {"dc", "http://purl.org/dc/elements/1.1/"}
                                  };
            // Act
            var result = processor.IsSafeCURIE("[dc:creator]", uriMappings);

            // Assert
            Assert.AreEqual("dc:creator", result.Curie);
            Assert.AreEqual("dc", result.Prefix);
            Assert.AreEqual("creator", result.Reference);
        }

        [Test]
        public void IsURI_Test1()
        {
            // Arrange
            IRDFaProcessor processor = new RDFaProcessor();

            // Act
            var result1 = processor.IsURI("http://example.org/john-d/", "href");
            var result2 = processor.IsURI("[dc:creator]", "property");

            // Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual("http://example.org/john-d/", result1);
            Assert.IsNull(result2);
        }

        [Test]
        public void IsUriOrSafeCurie_Test1()
        {
            // Arrange
            IRDFaProcessor processor = new RDFaProcessor();
            var uriMappings = new Dictionary<string, string>
                                  {
                                      {"foaf", "http://xmlns.com/foaf/0.1/"},
                                      {"dc", "http://purl.org/dc/elements/1.1/"}
                                  };


            // Act
            var result1 = processor.IsUriOrSafeCurie("[dc:creator]", uriMappings, "property");
            var result2 = processor.IsUriOrSafeCurie("http://example.org/john-d/", uriMappings, "href");

            // Assert
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);

        }

        [Test]
        public void GetURIParsed_Test1()
        {
            // Arrange
            IRDFaProcessor processor = new RDFaProcessor();

            // Act
            var result1 = processor.GetURIParsed("http://example.org/john-d/?q=test");
            var result2 = processor.GetURIParsed("[dc:creator]");

            // Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual("http", result1.Scheme);
            Assert.AreEqual("example.org", result1.Authority);
            Assert.AreEqual("/john-d/", result1.Path);
            Assert.AreEqual("q=test", result1.Query);
            Assert.IsNull(result2);
        }

        [Test]
        public void IsReservedWord_Test1()
        {
            // Arrange
            IRDFaProcessor processor = new RDFaProcessor();

            // Act
            var result1 = processor.IsReservedWord("meta");
            var result2 = processor.IsReservedWord("not_reserved_word");
            // Assert

            Assert.IsNotNull(result1);
            Assert.IsNull(result2);
        }

        [Test]
        public void IsReservedWordOrCurie_Test1()
        {
            // Arrange
            IRDFaProcessor processor = new RDFaProcessor();
            var uriMappings = new Dictionary<string, string>
                                  {
                                      {"foaf", "http://xmlns.com/foaf/0.1/"},
                                      {"dc", "http://purl.org/dc/elements/1.1/"}
                                  };
            // Act
            var result1 = processor.IsReservedWordOrCurie("meta", uriMappings);
            var result2 = processor.IsReservedWordOrCurie("dc:creator", uriMappings);
            var result3 = processor.IsReservedWordOrCurie("not_reserved_word", uriMappings);
            // Assert

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNull(result3);

        }

        [Test]
        public void GetCURIEs_Test1()
        {
            // Arrange
            IRDFaProcessor processor = new RDFaProcessor();
            var uriMappings = new Dictionary<string, string>
                                  {
                                      {"foaf", "http://xmlns.com/foaf/0.1/"},
                                      {"dc", "http://purl.org/dc/elements/1.1/"}
                                  };
            // Act
            //var result = processor.GetCURIEs("dc:creator", uriMappings);

            // Assert

        }
    }
}
