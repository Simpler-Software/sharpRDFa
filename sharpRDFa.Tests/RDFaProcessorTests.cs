using System.Collections.Generic;
using NUnit.Framework;

namespace sharpRDFa.Tests
{
    [TestFixture]
    public class RDFaProcessorTests
    {
        private IRDFaProcessor _processor;
        private Dictionary<string, string> _uriMappings;

        [SetUp]
        public void TestSetUp()
        {
            _processor = new RDFaProcessor();
            _uriMappings = new Dictionary<string, string>
                               {
                                   {"foaf", "http://xmlns.com/foaf/0.1/"},
                                   {"dc", "http://purl.org/dc/elements/1.1/"},
                                   {"xsd","http://www.w3.org/2001/XMLSchema#"}
                               };
        }

        [Test]
        public void IsNameSpace_ValidInput_ReturnsExpectedResults()
        {
            // Arrange

            // Act
            var result = _processor.IsNameSpace("xmlns:foaf");
            
            // Assert
            Assert.AreEqual("xmlns", result.Prefix);
            Assert.AreEqual("foaf", result.NCName);
        }

        [Test]
        public void IsNameSpace_InvalidInput_ReturnsNull()
        {
            // Arrange

            // Act
            var result = _processor.IsNameSpace("xmlns");
            
            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void IsCURIE_ValidInput_ReturnsExpectedResults()
        {
            // Arrange
            
            // Act
            var result = _processor.IsCURIE("dc:creator", _uriMappings);

            // Assert
            Assert.AreEqual("dc:creator", result.Curie);
            Assert.AreEqual("dc", result.Prefix);
            Assert.AreEqual("creator", result.Reference);
        }

        [Test]
        public void IsSafeCURIE_ValidInput_ReturnsExpectedResults()
        {
            // Arrange

            // Act
            var result = _processor.IsSafeCURIE("[dc:creator]", _uriMappings);

            // Assert
            Assert.AreEqual("dc:creator", result.Curie);
            Assert.AreEqual("dc", result.Prefix);
            Assert.AreEqual("creator", result.Reference);
        }

        [Test]
        public void IsURI_ValidInput_ReturnsExpectedResults()
        {
            // Arrange

            // Act
            var result1 = _processor.IsURI("http://example.org/john-d/", "href");
            var result2 = _processor.IsURI("[dc:creator]", "property");

            // Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual("http://example.org/john-d/", result1);
            Assert.IsNull(result2);
        }

        [Test]
        public void IsUriOrSafeCurie_ValidInput_ReturnsExpectedResults()
        {
            // Arrange

            // Act
            var result1 = _processor.IsUriOrSafeCurie("[dc:creator]", _uriMappings, "property");
            var result2 = _processor.IsUriOrSafeCurie("http://example.org/john-d/", _uriMappings, "href");

            // Assert
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [Test]
        public void GetURIParsed_ValidInput_ReturnsExpectedResults()
        {
            // Arrange

            // Act
            var result1 = _processor.GetURIParsed("http://example.org/john-d/?q=test");
            var result2 = _processor.GetURIParsed("[dc:creator]");

            // Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual("http", result1.Scheme);
            Assert.AreEqual("example.org", result1.Authority);
            Assert.AreEqual("/john-d/", result1.Path);
            Assert.AreEqual("q=test", result1.Query);
            Assert.IsNull(result2);
        }

        [Test]
        public void IsReservedWord_ValidInput_ReturnsExpectedResults()
        {
            // Arrange

            // Act
            var result1 = _processor.IsReservedWord("meta");
            var result2 = _processor.IsReservedWord("not_reserved_word");
            
            // Assert
            Assert.IsNotNull(result1);
            Assert.IsNull(result2);
        }

        [Test]
        public void IsReservedWordOrCurie_ValidInput_ReturnsExpectedResults()
        {
            // Arrange

            // Act
            var result1 = _processor.IsReservedWordOrCurie("meta", _uriMappings);
            var result2 = _processor.IsReservedWordOrCurie("dc:creator", _uriMappings);
            var result3 = _processor.IsReservedWordOrCurie("not_reserved_word", _uriMappings);
            
            // Assert
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNull(result3);
        }

        [Test]
        public void GetCURIEs_ValidInput_ReturnsExpectedResults()
        {
            // Arrange
            
            // Act
            IList<CURIE> result = _processor.GetCURIEs("dc:creator", _uriMappings);

            // Assert
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("dc:creator", result[0].Curie);
        }

        [Test]
        public void CURIEtoURI_ValidInput_ReturnsExpectedResults()
        {
            // Arrange
            
            // Act
            string result = _processor.CURIEtoURI("dc:creator", _uriMappings);
            
            // Assert
            Assert.AreEqual("http://purl.org/dc/elements/1.1/creator", result);
        }

        [Test]
        public void SafeCURIEtoURI_ValidInput_ReturnsExpectedResults()
        {
            // Arrange

            // Act
            string result = _processor.SafeCURIEtoURI("[dc:creator]", _uriMappings);

            // Assert
            Assert.AreEqual("http://purl.org/dc/elements/1.1/creator", result);
        }

        [Test]
        public void ResolveCURIE_ValidInput_ReturnsExpectedResults()
        {
            // Arrange

            // Act
            string result = _processor.ResolveCURIE("dc:creator", "http://example.org/john-d/", _uriMappings);
            
            // Assert
            Assert.AreEqual("http://purl.org/dc/elements/1.1/creator", result);
        }

        [Test]
        public void ResolveSafeCURIE_ValidInput_ReturnsExpectedResults()
        {
            // Arrange

            // Act
            string result = _processor.ResolveSafeCURIE("[dc:creator]", "http://example.org/john-d/", _uriMappings);

            // Assert
            Assert.AreEqual("http://purl.org/dc/elements/1.1/creator", result);
        } 
    }
}
