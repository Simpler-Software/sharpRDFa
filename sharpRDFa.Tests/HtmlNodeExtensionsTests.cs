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
    }
}
