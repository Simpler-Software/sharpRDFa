using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace sharpRDFa.Tests
{
    [TestFixture]
    public class VocabularyTests
    {
        [Test]
        public void GetCommonPrefixes_WithEmbeddedJsonFile_ReturnsPrefixesDictionary()
        {
            // Arrange

            // Act
            var result = Vocabulary.GetCommonPrefixes();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Dictionary<string, string>>(result);
            Assert.AreEqual(5105, result.Count);
            Assert.IsTrue(result.ContainsKey("dc"));
            Assert.AreEqual("http://purl.org/dc/elements/1.1/", result["dc"]);
        }
    }
}
