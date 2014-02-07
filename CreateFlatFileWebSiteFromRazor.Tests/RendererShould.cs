using CreateFlatFileWebsiteFromRazor.Logic;
using NUnit.Framework;
using Moq;

namespace CreateFlatFileWebSiteFromRazor.Tests
{
    [TestFixture]
    public class RendererShould
    {
        [Test]
        public void CombineSingleRazorTemplateWithJsonDataCorrectly()
        {
            // arrange
            const string plainContent = "<html><body>@Model.Description</body></html>";
            const string simpleProductData = "{\"Description\":\"Simple Product Data\"}";
            const string expectedResult = "<html><body>Simple Product Data</body></html>";

            var mockContentRepository = new Mock<IContentRepository>();
            mockContentRepository.Setup(c => c.GetContent(It.IsAny<string>())).Returns(plainContent);

            var mockDataRepository = new Mock<IDataRepository>();
            mockDataRepository.Setup(d => d.GetData(It.IsAny<string>())).Returns(simpleProductData);

            // act
            var renderer = new RenderHtmlPage(mockContentRepository.Object, mockDataRepository.Object);
            var result = renderer.BuildContentResult("product", "1");

            // assert
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void CombineSingleTemplateAndLayoutWithJsonDataCorrectly()
        {
            // arrange
            const string layoutContent = "<html><body>@RenderBody()</body></html>";
            const string productContent = @"@{Layout = ""layout"";}Hello, @Model.Description";

            const string simpleProductData = "{\"Description\":\"Simple Product Data\"}";
            const string expectedResult = "<html><body>Hello, Simple Product Data</body></html>";

            var mockContentRepository = new Mock<IContentRepository>();
            mockContentRepository.Setup(c => c.GetContent(It.Is<string>(n => n.Equals("layout")))).Returns(layoutContent);
            mockContentRepository.Setup(c => c.GetContent(It.Is<string>(n => n.Equals("product")))).Returns(productContent);

            var mockDataRepository = new Mock<IDataRepository>();
            mockDataRepository.Setup(d => d.GetData(It.IsAny<string>())).Returns(simpleProductData);

            // act
            var renderer = new RenderHtmlPage(mockContentRepository.Object, mockDataRepository.Object);
            var result = renderer.BuildContentResult("product", "1");

            // assert
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void CombineSingleTemplateAndLayoutAndSingleIncludeWithJsonDataCorrectly()
        {
            // arrange
            const string layoutContent = "<html><body>@RenderBody()</body></html>";
            const string productContent = @"@{Layout = ""layout"";}Hello, @Model.Title<br />@Include(""footer"")";
            const string footerContent = "Detail: @Model.Description";

            const string simpleProductData = "{\"Description\":\"Simple Product Data\", \"Title\":\"A nice test product\"}";
            const string expectedResult = "<html><body>Hello, A nice test product<br />Detail: Simple Product Data</body></html>";

            var mockContentRepository = new Mock<IContentRepository>();
            mockContentRepository.Setup(c => c.GetContent(It.Is<string>(n => n.Equals("layout")))).Returns(layoutContent);
            mockContentRepository.Setup(c => c.GetContent(It.Is<string>(n => n.Equals("product")))).Returns(productContent);
            mockContentRepository.Setup(c => c.GetContent(It.Is<string>(n => n.Equals("footer")))).Returns(footerContent);

            var mockDataRepository = new Mock<IDataRepository>();
            mockDataRepository.Setup(d => d.GetData(It.IsAny<string>())).Returns(simpleProductData);

            // act
            var renderer = new RenderHtmlPage(mockContentRepository.Object, mockDataRepository.Object);
            var result = renderer.BuildContentResult("product", "1");

            // assert
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void CombineSingleTemplateAndLayoutAndMultipleIncludesWithJsonDataCorrectly()
        {
            // arrange
            const string layoutContent = @"<html><body>@Include(""header"") @RenderBody()</body></html>";
            const string productContent = @"@{Layout = ""layout"";}Hello, @Model.Title<br />@Include(""footer"")";
            const string headerContent = "Greetings!<br />";
            const string footerContent = "Detail: @Model.Description";

            const string simpleProductData = "{\"Description\":\"Simple Product Data\", \"Title\":\"A nice test product\"}";
            const string expectedResult = "<html><body>Greetings!<br /> Hello, A nice test product<br />Detail: Simple Product Data</body></html>";

            var mockContentRepository = new Mock<IContentRepository>();
            mockContentRepository.Setup(c => c.GetContent(It.Is<string>(n => n.Equals("layout")))).Returns(layoutContent);
            mockContentRepository.Setup(c => c.GetContent(It.Is<string>(n => n.Equals("product")))).Returns(productContent);
            mockContentRepository.Setup(c => c.GetContent(It.Is<string>(n => n.Equals("footer")))).Returns(footerContent);
            mockContentRepository.Setup(c => c.GetContent(It.Is<string>(n => n.Equals("header")))).Returns(headerContent);

            var mockDataRepository = new Mock<IDataRepository>();
            mockDataRepository.Setup(d => d.GetData(It.IsAny<string>())).Returns(simpleProductData);

            // act
            var renderer = new RenderHtmlPage(mockContentRepository.Object, mockDataRepository.Object);
            var result = renderer.BuildContentResult("product", "1");

            // assert
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void CombineSingleTemplateAndLayoutAndMultipleNestedIncludesWithJsonDataCorrectly()
        {
            // arrange
            const string layoutContent = @"<html><body>@Include(""header"") @RenderBody()</body></html>";
            const string productContent = @"@{Layout = ""layout"";}Hello, @Model.Title<br />@Include(""footer"")";
            const string subContent = "@Model.Description";
            const string headerContent = "Greetings!<br />";
            const string footerContent = @"Detail: @Include(""subContent"")";

            const string simpleProductData = "{\"Description\":\"Simple Product Data\", \"Title\":\"A nice test product\"}";
            const string expectedResult = "<html><body>Greetings!<br /> Hello, A nice test product<br />Detail: Simple Product Data</body></html>";

            var mockContentRepository = new Mock<IContentRepository>();
            mockContentRepository.Setup(c => c.GetContent(It.Is<string>(n => n.Equals("layout")))).Returns(layoutContent);
            mockContentRepository.Setup(c => c.GetContent(It.Is<string>(n => n.Equals("product")))).Returns(productContent);
            mockContentRepository.Setup(c => c.GetContent(It.Is<string>(n => n.Equals("footer")))).Returns(footerContent);
            mockContentRepository.Setup(c => c.GetContent(It.Is<string>(n => n.Equals("header")))).Returns(headerContent);
            mockContentRepository.Setup(c => c.GetContent(It.Is<string>(n => n.Equals("subContent")))).Returns(subContent);

            var mockDataRepository = new Mock<IDataRepository>();
            mockDataRepository.Setup(d => d.GetData(It.IsAny<string>())).Returns(simpleProductData);

            // act
            var renderer = new RenderHtmlPage(mockContentRepository.Object, mockDataRepository.Object);
            var result = renderer.BuildContentResult("product", "1");

            // assert
            Assert.AreEqual(result, expectedResult);
        }
    }
}
