using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YouTubeApp.Services;

namespace YouTubeApp.Service.Test
{
    [TestFixture]
    public class YouTubeClientTest
    {
        private IYouTubeClient _youtubeClient;



        [SetUp]
        public void Setup()
        {
            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "APIKEY")]).Returns("AIzaSyAy_0a0bVHWj60mHEmPIK-lS_Ip0dAmQcE");
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "ClientApplicationName")]).Returns("YouTubeApp");

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(a => a.GetSection(It.Is<string>(s => s == "YouTubeConfig"))).Returns(mockConfSection.Object);
            mockConfiguration.SetupGet(a => a.GetSection(It.Is<string>(s => s == "YouTubeConfig"))).Returns(mockConfSection.Object);
            _youtubeClient = new YouTubeClient(mockConfiguration.Object);
        }

        [Test]
        public async Task SearchAsync_Query_Is_Null_Test()
        {
            try
            {
                await _youtubeClient.SearchAsync(null, 10);
            }
            catch(ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "query");
            }
        }
    }
}
