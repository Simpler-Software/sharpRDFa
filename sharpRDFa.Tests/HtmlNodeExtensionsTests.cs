using NUnit.Framework;
using sharpRDFa.Extension;

namespace sharpRDFa.Tests
{
    [TestFixture]
    public class HtmlNodeExtensionsTests
    {
        private TestContext _testContext;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _testContext = new TestContext();
        }

        [Test]
        public void GetXPATHAttributeValue_WithValidInput_ReturnsExpectedValue()
        {
            // Arrange
            var element = _testContext.GetRootElement("Resource\\alice-example.html");

            // Act
            var result = element.GetXPATHAttributeValue("//base", "href");

            // Assert
            Assert.AreEqual("http://example.com/", result);
        }

        [Test]
        public void GetAttributeValue_ElementWithNoMatchingAttribute_ReturnsNull()
        {
            // Arrange
            var element = _testContext.GetRootElement("Resource\\alice-example.html");

            // Act
            var result = element.GetAttributeValue(new[]{"not_existing_attrib_name"});

            // Assert
            Assert.IsNull(result);
        } 
    }
}
