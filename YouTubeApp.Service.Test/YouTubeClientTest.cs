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
            var configuration = new Mock<IConfiguration>();

            var apiKeySection = new Mock<IConfigurationSection>();
            apiKeySection.Setup(a => a.Value).Returns("AIzaSyAy_0a0bVHWj60mHEmPIK-lS_Ip0dAmQcE");

            var clientApplicationNameSection = new Mock<IConfigurationSection>();
            clientApplicationNameSection.Setup(a => a.Value).Returns("YouTubeApp");

            var youTubeConfigSection = new Mock<IConfigurationSection>();
            youTubeConfigSection.Setup(a => a.GetSection("APIKEY")).Returns(apiKeySection.Object);
            youTubeConfigSection.Setup(a => a.GetSection("ClientApplicationName")).Returns(clientApplicationNameSection.Object);

            configuration.Setup(a => a.GetSection("YouTubeConfig")).Returns(youTubeConfigSection.Object);

            _youtubeClient = new YouTubeClient(configuration.Object);
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
